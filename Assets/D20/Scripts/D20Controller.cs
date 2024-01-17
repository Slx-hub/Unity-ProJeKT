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

    public ValueShelf valueShelf;
    public D20FaceEmissionControl emissionController;
    public UIValueHitControl uivhc;

    private int CurrentFaceValue;
    private readonly Dictionary<Vector3, int> FaceValueLUT = new()
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

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            if (!emissionController.IsValueActive(CurrentFaceValue))
            {
                valueShelf.AddValueToShelf("<color=red>" + CurrentFaceValue.ToString() + "</color>");
                emissionController.NextPattern();
                return;
            }
            m_Rigidbody.velocity += Vector3.up * (5 + CurrentFaceValue / 3);
            valueShelf.AddValueToShelf(CurrentFaceValue.ToString());
            uivhc.HitValue("<color=green>" + CurrentFaceValue.ToString() + "</color>");
            emissionController.NextPattern();
        }
    }
    private void OnDash(InputAction.CallbackContext context)
    {
        Vector2 dashVector = m_actions.WASD.Move.ReadValue<Vector2>() * m_DashPower * (CurrentFaceValue / 2);

        if (!dashVector.Equals(Vector2.zero))
        {
            if (!emissionController.IsValueActive(CurrentFaceValue))
            {
                valueShelf.AddValueToShelf("<color=red>" + CurrentFaceValue.ToString() + "</color>");
                emissionController.NextPattern();
                return;
            }

            m_Rigidbody.AddForce(dashVector.x, 0, dashVector.y);
            valueShelf.AddValueToShelf(CurrentFaceValue.ToString());
            uivhc.HitValue("<color=green>" + CurrentFaceValue.ToString() + "</color>");
            emissionController.NextPattern();
        }
    }

    void OnGUI()
    {
        if (Application.isEditor)
        {
            var debugText = m_Rigidbody.angularVelocity.ToString()
                + "\n" + angularVelocities.Average()
                + "\n\n" + m_actions.WASD.Move.ReadValue<Vector2>().ToString()
                + "\n" + m_Rigidbody.velocity.magnitude
                + "\n\n" + isGrounded.ToString()
                + "\n\n" + CurrentFaceValue.ToString();

            GUI.Box(new Rect(5, 5, 200, 200), debugText);
        }
    }

    void OnEnable()
    {
        m_actions.WASD.Enable();
    }
    void OnDisable()
    {
        m_actions.WASD.Disable();
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
