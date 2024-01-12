using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class CatController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Animator animator;

    public float WalkSpeed;
    public float RunSpeed;
    public float TurnSpeed;

    public float WalkAccel;
    public float RunAccel;

    public float JumpForce;

    public bool Running;
    public bool Grounded;
    public bool Jumped;

    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;

        Grounded = CheckIfGrounded();
        if(Jumped && Grounded)
        {
            Jumped = false;
            animator.SetBool("Jump", false);
        }

        Running = false;
        if (keyboard.leftShiftKey.isPressed)
        {
            Running = true;
        }

        float vel = Vector3.Dot(transform.forward, rigidbody.velocity);
        float accel = Running ? RunAccel : WalkAccel;
        float maxSpeed = Running ? RunSpeed : WalkSpeed;

        if (vel < maxSpeed)
        {
            if (keyboard.wKey.isPressed)
            {
                rigidbody.AddForce(transform.forward * accel, ForceMode.Impulse);
            }
            if (keyboard.sKey.isPressed)
            {
                rigidbody.AddForce(-transform.forward * accel, ForceMode.Impulse);
            }
        }

        if (keyboard.aKey.isPressed)
        {
            transform.Rotate(Vector3.up, -TurnSpeed * Time.deltaTime);
        }
        if (keyboard.dKey.isPressed)
        {
            transform.Rotate(Vector3.up, TurnSpeed * Time.deltaTime);
        }
        if (Grounded && keyboard.spaceKey.isPressed)
        {
            Jumped = true;
            animator.SetBool("Jump", true);
            rigidbody.AddForce(JumpForce * Vector3.up, ForceMode.VelocityChange);
        }

        animator.SetFloat("Speed", vel);
        Speed = vel;
    }

    private bool CheckIfGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.5f);
    }
}
