using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class D20EmissionCtrl : MonoBehaviour
{
    /*[SerializeField]*/public Gradient EmissiveGradient;
    /*[SerializeField]*/public Light PowerLight;
    public float TopSpeed = 5.0f;

    MeshRenderer LinkedMR;
    Rigidbody LinkedRB;

    private List<float> angularVelocities = new();

    // Start is called before the first frame update
    void Start()
    {
        LinkedMR = GetComponent<MeshRenderer>();
        LinkedRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angularVelocities.Add(LinkedRB.angularVelocity.magnitude);
        if (angularVelocities.Count > 50)
            angularVelocities.RemoveAt(0);

        var color = EmissiveGradient.Evaluate(angularVelocities.Average() / TopSpeed);
        LinkedMR.material.SetColor("_EmissionColor", color);

        PowerLight.enabled = angularVelocities.Average() > 3;
    }
}
