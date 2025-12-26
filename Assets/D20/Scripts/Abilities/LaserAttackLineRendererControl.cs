using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LaserAttackLineRendererControl : MonoBehaviour
{
    private LineRenderer m_lr;

    public Material m_material;
    public Gradient m_gradient;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(float sThickness, float eThickness)
    {
        m_lr = GetComponent<LineRenderer>();

        m_lr.material = m_material;
        m_lr.startWidth= sThickness;
        m_lr.endWidth= eThickness;
        m_lr.colorGradient= m_gradient;
    }
}
