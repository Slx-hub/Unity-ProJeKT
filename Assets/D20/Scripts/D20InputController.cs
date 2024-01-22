using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class D20InputController : MonoBehaviour
{
    public Transform AimDirection;
    public float RollingPower = 1f;
    public float Torque = 1f;
    public float DashPower = 1f;

    private DefaultActionsWrapper Actions;
    private Rigidbody Rigidbody;
    private D20Controller Controller;
    private ComboController ComboController;
    private JumpController JumpController;
    private UIValueHitControl uivhc;

    private Vector3 ForceVector;
    private Vector3 TorqueVector;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Controller = GetComponent<D20Controller>();
        ComboController = GetComponent<ComboController>();
        uivhc = GetComponent<UIValueHitControl>();
        JumpController = GetComponent<JumpController>();
    }

    void Awake()
    {
        Actions = new DefaultActionsWrapper();

        Actions.WASD.Jump.performed += OnJump;
        Actions.WASD.Dash.performed += OnDash;
        Actions.WASD.Fire.performed += OnFire;

        Actions.WASD.Ability1.performed += OnAbility1;
        Actions.WASD.Ability2.performed += OnAbility2;
        Actions.WASD.Ability3.performed += OnAbility3;
    }

    void FixedUpdate()
    {
        Vector2 moveVector = Actions.WASD.Move.ReadValue<Vector2>();
        Quaternion quat = Quaternion.AngleAxis(AimDirection.eulerAngles.y, Vector3.up);
        ForceVector = quat * new Vector3(moveVector.x, 0, moveVector.y) * RollingPower;
        TorqueVector = quat * new Vector3(moveVector.y, 0f, -moveVector.x) * Torque;

        Rigidbody.AddForce(ForceVector);
        Rigidbody.AddTorque(TorqueVector);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (JumpController.OnJump())
        {
            Rigidbody.velocity += Vector3.up * (5 + Controller.CurrentFaceValue / 3);
            uivhc.HitValue("<color=#555555>" + Controller.CurrentFaceValue.ToString() + "</color>");
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (!Actions.WASD.Move.ReadValue<Vector2>().Equals(Vector2.zero) && JumpController.OnDash())
        {
            Quaternion quat = Quaternion.AngleAxis(AimDirection.eulerAngles.y, Vector3.up);
            Vector2 dashVector = Actions.WASD.Move.ReadValue<Vector2>() * DashPower * (Controller.CurrentFaceValue / 2);
            Rigidbody.AddForce(quat * new Vector3(dashVector.x, 0, dashVector.y));
            uivhc.HitValue("<color=#555555>" + Controller.CurrentFaceValue.ToString() + "</color>");
        }
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        ComboController.ValidateRoll(Controller.CurrentFaceValue);
    }

    private void OnAbility1(InputAction.CallbackContext context)
    {
        ComboController.StartCombo("TestCombo1");
    }

    private void OnAbility2(InputAction.CallbackContext context)
    {
        ComboController.StartCombo("TestCombo2");
    }

    private void OnAbility3(InputAction.CallbackContext context)
    {
        ComboController.StartCombo("TestCombo3");
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
