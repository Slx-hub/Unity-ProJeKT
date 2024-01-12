using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class D20Controller : MonoBehaviour
{
    DefaultActionsWrapper m_actions;
    Rigidbody m_Rigidbody;
    Collider m_jumpBoundary;
    public float m_RollingPower = 1f;
    public float m_JumpForce = 1f;
    bool isGrounded = true;

    public GUIStyle style;

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

    private List<float> angularVelocities = new();

    void Awake()
    {
        m_actions = new DefaultActionsWrapper();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_jumpBoundary = GetComponent<Collider>();

        m_actions.WASD.Jump.performed += OnJump;
    }

    void FixedUpdate()
    {
        Vector2 moveVector = m_actions.WASD.Move.ReadValue<Vector2>() * m_RollingPower;
        m_Rigidbody.AddForce(moveVector.x, 0, moveVector.y);

        angularVelocities.Add(m_Rigidbody.angularVelocity.magnitude);
        if (angularVelocities.Count > 25)
            angularVelocities.RemoveAt(0);

        float maxDot = 0.0f;
        Vector3 localUp = transform.InverseTransformVector(Vector3.up);
        foreach (var entry in FaceValueLUT)
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
        if (isGrounded && angularVelocities.Average() > 3)
        {
            m_Rigidbody.velocity += Vector3.up * (5 + CurrentFaceValue / 3);
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
                + "\n\n" + isGrounded.ToString();

            GUI.Box(new Rect(5, 5, 200, 200), debugText);


            
            GUI.Box(new Rect(Screen.width / 2, Screen.height -150, 150, 150), CurrentFaceValue.ToString(), style);
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
        isGrounded = true;
    }


    void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }
}
