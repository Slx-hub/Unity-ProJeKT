using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class SimpleMovmentController : NetworkBehaviour
{
    public float MoveForce = 1f;
    public float JumpPower = 1f;
    public float DashPower = 1f;

    private DefaultActionsWrapper Actions;
    private Rigidbody Rigidbody;
    private SimpleJumpController JumpController;
    public Transform CameraTransform;

    private Vector3 ForceVector;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Actions = new DefaultActionsWrapper();

        Actions.WASD.Jump.performed += OnJump;
        Actions.WASD.Dash.performed += OnDash;

        Actions.WASD.ReloadScene.performed +=
            c => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        JumpController = GetComponent<SimpleJumpController>();
        CameraTransform = Camera.main.transform;        
    }

    public override void OnNetworkSpawn()
    {
        EventBroker<OnNetworkCreateEvent>.PublishEvent(new OnNetworkCreateEvent(this));
        if (!IsOwner)
        {
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        Vector2 moveVector = Actions.WASD.Move.ReadValue<Vector2>();
        Quaternion quat = Quaternion.AngleAxis(CameraTransform.eulerAngles.y, Vector3.up);
        ForceVector = quat * new Vector3(moveVector.x, 0, moveVector.y) * MoveForce;

        Rigidbody.AddForce(ForceVector);
    }

    private void Update()
    {
        ShowCursorOnAlt();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        JumpController.OnJump(JumpPower);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        var direction = Actions.WASD.Move.ReadValue<Vector2>();
        if (!direction.Equals(Vector2.zero))
        {
            var quat = Quaternion.AngleAxis(CameraTransform.eulerAngles.y, Vector3.up);
            var moveDir = Actions.WASD.Move.ReadValue<Vector2>() * DashPower;
            var dashVector = quat * new Vector3(moveDir.x, 0, moveDir.y);
            JumpController.OnDash(dashVector);
        }
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
}
