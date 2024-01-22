using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(FollowPath))]
[RequireComponent(typeof(LookAt))]
[RequireComponent(typeof(LineRenderer))]
public class DroneControl : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float minLOSTime = 2f;
    public float minLockOnTime = 3f;
    public float coolDown = 20f;
    public float timeBetweenProjectiles = 0.25f;
    public int numProjectiles = 3;
    public Transform projectile;

    private FollowPath m_fpc;
    private LookAt m_lac;
    private LineRenderer m_lineRenderer;
    public float m_losTimer = 0f;
    public float m_lockOnTimer = 0f;
    public float m_coolDownTimer = 0f;
    private int m_spawnedProjectiles = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_fpc= GetComponent<FollowPath>();
        m_fpc.speed= speed;

        m_lac= GetComponent<LookAt>();
        m_lac.target= target;

        m_lineRenderer= GetComponent<LineRenderer>();
        m_lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) return;

        if (!m_fpc.followPath) m_fpc.Follow();
        if(!m_lac.lookAt) m_lac.lookAt = true;

        TestForLOS();

        if (m_losTimer > minLOSTime)
        {
            m_lineRenderer.enabled = true;
            m_lineRenderer.SetPositions(new Vector3[] { transform.position, target.transform.position });

            m_lockOnTimer += Time.deltaTime;

            if (m_lockOnTimer > minLockOnTime && m_coolDownTimer <= 0f)
            {
                Attack();
            }

        }
        else
        {
            m_lineRenderer.enabled = false;
            m_lockOnTimer = 0f;
            m_spawnedProjectiles = 0;
        }

        if (m_coolDownTimer > 0f)
            m_coolDownTimer -= Time.deltaTime;
    }

    private void Attack()
    {
        if(m_spawnedProjectiles < numProjectiles)
        {
            m_spawnedProjectiles++;
            m_coolDownTimer = timeBetweenProjectiles;

            var go = GameObject.Instantiate(projectile, transform.position, Quaternion.identity);
            go.GetComponent<ProjectileControl>().SetTarget(target);
        }else
        {
            m_coolDownTimer = coolDown;
            m_spawnedProjectiles = 0;
        }
    }

    private void TestForLOS()
    {
        if (Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out var raycastHit, float.PositiveInfinity))
        {
            if (raycastHit.collider.gameObject.name.Equals("D20"))
                m_losTimer += Time.deltaTime;
            else
                m_losTimer = 0f;
        }
        else
            m_losTimer = 0f;
    }
}
