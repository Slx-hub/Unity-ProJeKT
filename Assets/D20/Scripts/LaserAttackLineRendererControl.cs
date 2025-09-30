using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LaserAttackLineRendererControl : NetworkBehaviour
{
    private LineRenderer m_lr;

    public Material m_material;
    public NetworkVariable<Gradient> m_gradient;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ServerRpc]
    public void Init_ServerRPC(float sThickness, float eThickness)
    {
        if(IsServer)
            Init_ClientRPC(sThickness, eThickness);

        m_lr = GetComponent<LineRenderer>();

        m_lr.material = m_material;
        m_lr.startWidth= sThickness;
        m_lr.endWidth= eThickness;
        m_lr.colorGradient= m_gradient.Value;
    }

    [ClientRpc]
    public void Init_ClientRPC(float sThickness, float eThickness)
    {
        m_lr = GetComponent<LineRenderer>();

        m_lr.material = m_material;
        m_lr.startWidth = sThickness;
        m_lr.endWidth = eThickness;
        m_lr.colorGradient = m_gradient.Value;
    }
}
