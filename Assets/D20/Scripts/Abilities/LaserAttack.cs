using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.D20.Scripts
{
    internal class LaserAttack : Ability
    {
        public GameObject LockOnIndicator;
        public GameObject LineRendererPrefab;
        public Gradient laserGradient;
        public Material lrMaterial;
        public float lrThickness;
        public int lrSegments;
        public int lrTailingSegemtns;

        public AudioClip lockOnAudio;
        public AudioClip fireLaserAudio;
        public AudioClip hitLaserAudio;
        public AudioClip denyAudio;

        public float timeBetweenLasers = 0.1f;
        public float timeBetweenFirstLockonAndFire = 1f;
        public float timeToAdvanceLeaser = 0.05f;
        public float laserHitTime = 5f;
        public float laserTime = 5f;
        private AbilityControler m_ac;
        private Transform m_target;
        private AudioSource m_as;
        private Canvas m_c;
        private int m_numsLasers;
        private int m_laserStep = 0;
        private int m_accHurt = 0;

        private List<GameObject> tidyUp = new List<GameObject>();
        private List<ParabolicTrajectory> m_pts = new List<ParabolicTrajectory>();
        private List<Vector3> m_targetOffsets = new List<Vector3>();

        public override void ComboStart(AbilityControler ac, int roll, Transform target, Vector3 direction, Canvas canvas, bool comboComplete)
            => Initalize(ac, roll, target, canvas);
        public override void ComboAdvanced(AbilityControler ac, int roll, Transform target, Vector3 direction, Canvas canvas, bool comboComplete)
            => LockOnLaser(roll);

        public override void ComboComplete(AbilityControler ac, int roll, Transform target, Vector3 direction, Canvas canvas, bool comboComplete)
            => ShotLasers(roll);

        public override void ComboFailed(AbilityControler ac, int roll, Transform target, Vector3 direction, Canvas canvas, bool comboComplete)
        {
            m_as.PlayOneShot(denyAudio);
            m_accHurt /= 4;
            if(m_numsLasers > 0)
                ShotLasers(roll);
            else
                m_ac.GetEventControler().AddEvent(10, Die);
        }

        private void Initalize(AbilityControler ac, int val, Transform target, Canvas canvas)
        {
            m_as = GetComponent<AudioSource>();
            m_ac = ac;
            m_c = canvas;
            m_target = target;
            m_laserStep = 1;
            m_numsLasers = 0;
            m_accHurt = 0;

            if (m_target == null)
            {
                m_as.PlayOneShot(denyAudio);
                m_ac.GetEventControler().AddEvent(10, Die);
                return;
            }

            LockOnLaser(20);

            LockOnLaser(20);

            ShotLasers(20);
        }

        private void LockOnLaser(int val)
        {
            m_numsLasers += val / (20 / m_laserStep) + 1;
            m_accHurt += val * m_laserStep;
             
            for (int i = 0; i < m_numsLasers; i++)
            {
                m_ac.GetEventControler().AddEvent(i * timeBetweenLasers, LockOnTargetCallback, true);
            }

            m_laserStep++;
        }

        private void ShotLasers(int val)
        {
            LockOnLaser(val);

            m_ac.GetEventControler().AddEvent(timeBetweenFirstLockonAndFire, StartLaserCallback, true);
        }
        public void LockOnTargetCallback()
        {
            var go = GameObject.Instantiate(LockOnIndicator, m_c.transform);
            var uiwpc = go.GetComponent<UIWorldPosition>();
            var crt = m_c.GetComponent<RectTransform>();

            uiwpc.worldPosition = m_target.transform.position +
                new Vector3(UnityEngine.Random.Range(-1f, 1f) * 0.75f,
                UnityEngine.Random.Range(-1f, 1f) * 0.75f,
                UnityEngine.Random.Range(-1f, 1f) * 0.75f);

            uiwpc.canvasRect = crt.rect;
            uiwpc.cam = Camera.main;

            m_targetOffsets.Add(uiwpc.worldPosition - m_target.transform.position);
            
            tidyUp.Add(go);
            m_as.PlayOneShot(lockOnAudio);
        }

        public void StartLaserCallback()
        {
            for (int i = 0; i < m_numsLasers; i++)
            {
                SpawnLaserRpc(i);
            }

            if (m_numsLasers > 0)
            {
                m_ac.GetEventControler().AddEvent(laserTime, TidyUpLasers, true);
            }
        }

        [Rpc(SendTo.Server)]
        private void SpawnLaserRpc(int i, RpcParams rpcParams = default)
        {
            var go = GameObject.Instantiate(LineRendererPrefab, transform.position, Quaternion.identity, transform);
            go.GetComponent<NetworkObject>().Spawn(true);

            SpawnLaserDoneRpc(i, go.GetComponent<NetworkObject>().NetworkObjectId, RpcTarget.Single(rpcParams.Receive.SenderClientId, RpcTargetUse.Temp));
        }

        [Rpc(SendTo.SpecifiedInParams)]
        private void SpawnLaserDoneRpc(int i, ulong laserId, RpcParams rpcParams)
        {
            NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(laserId, out var laserNetObj);

            var go = laserNetObj.gameObject;

            var lalr = go.GetComponent<LaserAttackLineRendererControl>();
            var pt = go.AddComponent<ParabolicTrajectory>();

            lalr.Init(lrThickness, lrThickness);
            pt.Init(Owner.transform, m_target.transform.position + m_targetOffsets[m_pts.Count], 0.5f, 0.01f, 20);

            m_ac.GetEventControler().AddEvent(timeToAdvanceLeaser + timeBetweenLasers * i, AdvanceLaser, true);

            m_pts.Add(pt);
        }

        public void AdvanceLaser()
        {
            if (m_pts.Count <= 0)
                return;

            m_pts[0].Begin();
            m_pts.RemoveAt(0);

            m_as.PlayOneShot(fireLaserAudio, 0.3f);
            m_ac.GetEventControler().AddEvent(laserHitTime, Hit, true);
        }

        public void Hit()
        {
            m_as.PlayOneShot(hitLaserAudio, 0.3f);
        }

        public void TidyUpLasers()
        {
            m_target.GetComponent<Entity>().HurtRpc(m_accHurt);

            tidyUp.ForEach(go => GameObject.Destroy(go));
            tidyUp.Clear();

            m_pts.ForEach(pt => GameObject.Destroy(pt.gameObject));
        }

        public void Die()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
