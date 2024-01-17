using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class D20InputController : MonoBehaviour
{
    public float RollingPower = 1f;
    public float Torque = 1f;
    public float DashPower = 1f;

    private DefaultActionsWrapper Actions;
    private Rigidbody Rigidbody;
    private D20Controller Controller;

    private Vector3 ForceVector;
    private Vector3 TorqueVector;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Controller = GetComponent<D20Controller>();
    }

    void Awake()
    {
        Actions = new DefaultActionsWrapper();

        Actions.WASD.Jump.performed += OnJump;
        Actions.WASD.Dash.performed += OnDash;
    }

    void FixedUpdate()
    {
        Vector2 moveVector = Actions.WASD.Move.ReadValue<Vector2>();
        ForceVector = new Vector3(moveVector.x, 0, moveVector.y) * RollingPower;
        TorqueVector = new Vector3(moveVector.y, 0f, -moveVector.x) * Torque;

        Rigidbody.AddForce(ForceVector);
        Rigidbody.AddTorque(TorqueVector);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (Controller.IsGrounded)
        {
            Rigidbody.velocity += Vector3.up * (5 + Controller.CurrentFaceValue / 3);
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (!Actions.WASD.Move.ReadValue<Vector2>().Equals(Vector2.zero))
        {
            Vector2 dashVector = Actions.WASD.Move.ReadValue<Vector2>() * DashPower * (Controller.CurrentFaceValue / 2);
            Rigidbody.AddForce(dashVector.x, 0, dashVector.y);
        }
    }

    private void OnFire()
    {

    }

    private void OnComboEvent()
    {
        /*
        if (isGrounded)
        {
            if (!emissionController.IsValueActive(CurrentFaceValue))
            {
                valueShelf.AddValueToShelf("<color=red>" + CurrentFaceValue.ToString() + "</color>");
                emissionController.NextPattern();
                return;
            }
            Rigidbody.velocity += Vector3.up * (5 + CurrentFaceValue / 3);
            valueShelf.AddValueToShelf(CurrentFaceValue.ToString());
            emissionController.NextPattern();
        }
        */
    }

    void OnEnable()
    {
        Actions.WASD.Enable();
    }
    void OnDisable()
    {
        Actions.WASD.Disable();
    }

    public override string ToString()
    {
        return "> D20InputController:" +
            "\n  Move vector:\t\t" + Actions.WASD.Move.ReadValue<Vector2>() +
            "\n  Force vector:\t\t" + ForceVector +
            "\n  Torque vector:\t\t" + TorqueVector;
    }
}
