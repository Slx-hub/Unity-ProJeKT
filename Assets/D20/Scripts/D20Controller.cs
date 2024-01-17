using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.DefaultInputActions;

public class D20Controller : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    public int CurrentFaceValue { get; private set; }

    private Rigidbody Rigidbody;
    private readonly Dictionary<Vector3, int> FaceToValueLUT = new()
    {
        { new Vector3(0.15f, -0.46f, 0.63f)  , 11 },
        { new Vector3(-0.63f, -0.46f, 0.15f) , 1 },
        { new Vector3(-0.24f, 0.74f, -0.15f) , 12 },
        { new Vector3(0.24f, 0.74f, 0.15f)   , 2 },
        { new Vector3(-0.39f, -0.28f, 0.63f) , 13 },
        { new Vector3(-0.15f, -0.46f, -0.63f), 3 },
        { new Vector3(0.78f, 0.00f, 0.15f)   , 14 },
        { new Vector3(0.48f, 0.00f, 0.63f)   , 4 },
        { new Vector3(-0.63f, 0.46f, 0.15f)  , 15 },
        { new Vector3(-0.39f, 0.28f, 0.63f)  , 5 },
        { new Vector3(0.39f, -0.28f, -0.63f) , 16 },
        { new Vector3(0.63f, -0.46f, -0.15f) , 6 },
        { new Vector3(-0.48f, 0.00f, -0.63f) , 17 },
        { new Vector3(-0.78f, 0.00f, -0.15f) , 7 },
        { new Vector3(0.15f, 0.46f, 0.63f)   , 18 },
        { new Vector3(0.39f, 0.28f, -0.63f)  , 8 },
        { new Vector3(-0.24f, -0.74f, -0.15f), 19 },
        { new Vector3(0.24f, -0.74f, 0.15f)  , 9 },
        { new Vector3(0.63f, 0.46f, -0.15f)  , 20 },
        { new Vector3(-0.15f, 0.46f, -0.63f) , 10 },
    };

    private List<float> AngularVelocities = new();

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        AngularVelocities.Add(Rigidbody.angularVelocity.magnitude);
        if (AngularVelocities.Count > 25)
            AngularVelocities.RemoveAt(0);

        float maxDot = 0.0f;
        Vector3 localUp = transform.InverseTransformVector(Vector3.up);
        foreach (var entry in FaceToValueLUT)
        {
            if (Vector3.Dot(entry.Key, localUp) > maxDot)
            {
                maxDot = Vector3.Dot(entry.Key, localUp);
                CurrentFaceValue = entry.Value;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        IsGrounded = true;
    }


    void OnTriggerExit(Collider other)
    {
        IsGrounded = false;
    }

    public override string ToString()
    {
        return "> D20Controller:" +
            "\n  Current value:\t\t" + CurrentFaceValue.ToString() +
            "\n  Is on ground:\t\t" + IsGrounded.ToString() +
            "\n  Smooth angular V:\t" + AngularVelocities.Average() +
            "\n  Dice velocity:\t\t" + Rigidbody.velocity.magnitude;
    }
}
