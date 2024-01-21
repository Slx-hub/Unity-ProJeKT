using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowPath))]
[RequireComponent(typeof(LookAt))]
public class DroneControl : MonoBehaviour
{
    public Transform target;
    public float speed;

    private FollowPath m_fpc;
    private LookAt m_lac;
    // Start is called before the first frame update
    void Start()
    {
        m_fpc= GetComponent<FollowPath>();
        m_fpc.speed= speed;

        m_lac= GetComponent<LookAt>();
        m_lac.target= target;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) return;

        if (!m_fpc.followPath) m_fpc.Follow();
        if(!m_lac.lookAt) m_lac.lookAt = true;
    }
}
