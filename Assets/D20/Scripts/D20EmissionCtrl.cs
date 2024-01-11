using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class D20EmissionCtrl : MonoBehaviour
{
    [SerializeField] Gradient EmissiveGradient;
    public float TopSpeed = 6.0f;

    MeshRenderer LinkedMR;
    Rigidbody LinkedRB;

    // Start is called before the first frame update
    void Start()
    {
        LinkedMR = GetComponent<MeshRenderer>();
        LinkedRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var color = EmissiveGradient.Evaluate(LinkedRB.angularVelocity.magnitude / TopSpeed);
        LinkedMR.material.SetColor("_EmissionColor", color);
    }
}
