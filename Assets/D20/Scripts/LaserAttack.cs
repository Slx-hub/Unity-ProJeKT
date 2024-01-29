using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderData;

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

        private List<GameObject> tidyUp = new List<GameObject>();
        private List<ParabolicTrajectory> m_pts = new List<ParabolicTrajectory>();
        private List<Vector3> m_targetOffsets = new List<Vector3>();
        public override void Use(AbilityControler ac, int roll, Transform target, Canvas canvas)
        {
            m_numsLasers = roll / 4;
            m_ac = ac;
            m_as = GetComponent<AudioSource>();
            m_c = canvas;
            m_target= target;

            for(int i = 0; i < m_numsLasers; i++)
            {
                m_ac.ec.AddEvent(i*timeBetweenLasers, LockOnTargetCallback, true);
            }
        }

        public void LockOnTargetCallback()
        {
            if (tidyUp.Count == 0)
            {
                for(int i = 0; i < m_numsLasers; i++)
                    m_ac.ec.AddEvent(timeBetweenFirstLockonAndFire, StartLaserCallback, true);
            }

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
            var go = GameObject.Instantiate(LineRendererPrefab, transform.position, Quaternion.identity, transform);
            var lr = go.GetComponent<LineRenderer>();
            var pt = go.AddComponent<ParabolicTrajectory>();

            lr.material = lrMaterial;
            lr.startWidth = lrThickness;
            lr.endWidth = lrThickness;
            lr.colorGradient = laserGradient;

            pt.origin = transform.position;
            pt.target = m_target.transform.position + m_targetOffsets[m_pts.Count];
            pt.addRemoveDelta = 0.5f;
            pt.addLatency = 0.01f;
            pt.segments = 20;

            m_pts.Add(pt);

            if (m_pts.Count == 1)
            {
                m_ac.ec.AddEvent(timeToAdvanceLeaser, AdvanceLaser, true);
                m_as.PlayOneShot(fireLaserAudio);
            }
        }

        public void AdvanceLaser()
        {
            m_pts.ForEach(pt => pt.Begin());
            
            m_ac.ec.AddEvent(laserHitTime, Hit, true);
        }

        public void Hit()
        {
            m_as.PlayOneShot(hitLaserAudio);
            m_ac.ec.AddEvent(laserTime, TidyUpLasers, true);
        }

        public void TidyUpLasers()
        {
            tidyUp.ForEach(go => GameObject.Destroy(go));
            tidyUp.Clear();

            m_pts.ForEach(pt => GameObject.Destroy(pt.gameObject));

            m_ac.ec.AddEvent(3f, Die, true);
        }

        public void Die()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
