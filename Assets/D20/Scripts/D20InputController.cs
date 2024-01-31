using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class D20InputController : NetworkBehaviour
{
    public float RollingPower = 1f;
    public float Torque = 1f;
    public float JumpPower = 1f;
    public float DashPower = 1f;

    private DefaultActionsWrapper Actions;
    private Rigidbody Rigidbody;
    private D20Controller Controller;
    private ComboController ComboController;
    private JumpController JumpController;
    private UIValueHitControl uivhc;
    private Transform CameraTransform;

    private Vector3 ForceVector;
    private Vector3 TorqueVector;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Controller = GetComponent<D20Controller>();
        ComboController = GetComponent<ComboController>();
        uivhc = GetComponent<UIValueHitControl>();
        JumpController = GetComponent<JumpController>();
        CameraTransform = Camera.main.transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Actions = new DefaultActionsWrapper();

        Actions.WASD.Jump.performed += OnJump;
        Actions.WASD.Dash.performed += OnDash;
        Actions.WASD.Fire.performed += OnFire;

        Actions.WASD.Ability1.performed += OnAbility1;
        Actions.WASD.Ability2.performed += OnAbility2;
        Actions.WASD.Ability3.performed += OnAbility3;

        Actions.WASD.ReloadScene.performed += 
            c => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public override void OnNetworkSpawn()
    {
        EventBroker<OnNetworkCreateEvent>.PublishEvent(new OnNetworkCreateEvent(this, IsOwner));
        if (!IsOwner)
        {
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        Vector2 moveVector = Actions.WASD.Move.ReadValue<Vector2>();
        Quaternion quat = Quaternion.AngleAxis(CameraTransform.eulerAngles.y, Vector3.up);
        ForceVector = quat * new Vector3(moveVector.x, 0, moveVector.y) * RollingPower;
        TorqueVector = quat * new Vector3(moveVector.y, 0f, -moveVector.x) * Torque;

        Rigidbody.AddForce(ForceVector);
        Rigidbody.AddTorque(TorqueVector);
    }

    private void Update()
    {
        ShowCursorOnAlt();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (JumpController.OnJump(JumpPower * (5 + Controller.CurrentFaceValue / 3)))
            uivhc.HitValue("<color=#555555>" + Controller.CurrentFaceValue.ToString() + "</color>");
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        var direction = Actions.WASD.Move.ReadValue<Vector2>();
        if (!direction.Equals(Vector2.zero))
        {
            var quat = Quaternion.AngleAxis(CameraTransform.eulerAngles.y, Vector3.up);
            var moveDir = Actions.WASD.Move.ReadValue<Vector2>() * DashPower * (Controller.CurrentFaceValue / 2);
            var dashVector = quat * new Vector3(moveDir.x, 0, moveDir.y);
            if (JumpController.OnDash(dashVector))
                uivhc.HitValue("<color=#555555>" + Controller.CurrentFaceValue.ToString() + "</color>");
        }
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (Controller.IsPowered)
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

    private void ShowCursorOnAlt()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void OnEnable()
    {
        Actions?.WASD.Enable();
    }
    void OnDisable()
    {
        Actions?.WASD.Disable();
    }

    public override string ToString()
    {
        return "> D20InputController:" +
            "\n  Move vector:\t\t" + Actions?.WASD.Move.ReadValue<Vector2>() +
            "\n  Force vector:\t\t" + ForceVector +
            "\n  Torque vector:\t\t" + TorqueVector;
    }
}
