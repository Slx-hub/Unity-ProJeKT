using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class D20EmissionCtrl : MonoBehaviour
{
    public Gradient EmissiveGradient;
    public Light PowerLight;
    public float TopSpeed = 5.0f;

    MeshRenderer LinkedMR;
    Rigidbody LinkedRB;

    private D20Controller D20Controller;

    // Start is called before the first frame update
    void Start()
    {
        LinkedMR = GetComponent<MeshRenderer>();
        LinkedRB = GetComponent<Rigidbody>();
        D20Controller = GetComponent<D20Controller>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var color = EmissiveGradient.Evaluate(D20Controller.AngularVelocity / TopSpeed);
        LinkedMR.material.SetColor("_EmissionColor", color);

        PowerLight.enabled = D20Controller.IsPowered;
    }
}
