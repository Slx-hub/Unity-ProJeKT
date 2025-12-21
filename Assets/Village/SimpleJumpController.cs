using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class SimpleJumpController : MonoBehaviour
{
    public float CooldownTime = 1f;

    public float SphereCastRadius = 1f;

    public float JumpCooldown { get; private set; }
    public float DashCooldown { get; private set; }
    public int ValueThreshold { get; private set; }

    public SimplePlayerControler simplePlayerControler;

    private Rigidbody Rigidbody;
    private OverlapingSphereTest olst;

    //Only for debug porpuses
    private MoveState currentMoveState;
    private Vector3 wallJumpNormal = Vector3.zero;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        olst = GetComponent<OverlapingSphereTest>();
    }

    void FixedUpdate()
    {
        if (JumpCooldown < 1)
            JumpCooldown += Time.deltaTime / CooldownTime;
        if (DashCooldown < 1)
            DashCooldown += Time.deltaTime / CooldownTime;
    }

    public bool OnJump(float jumpPower)
    {
        if (JumpCooldown >= 1 && ValidatePreconditions() is var state && !state.Equals(MoveState.Failed))
        {
            Vector3 jumpVector;
            if (state.Equals(MoveState.Wall))
            {
                //var normal = GetWallDirection();
                var normal = wallJumpNormal;
                jumpVector = new Vector3(normal.x, 1, normal.z);
                //Rigidbody.angularVelocity = Vector3.Reflect(Rigidbody.angularVelocity, normal);
                //var quat = Quaternion.FromToRotation(transform.up, -transform.right);
                var torqueVector = new Vector3(normal.z, 0f, -normal.x);
                Rigidbody.angularVelocity = torqueVector * jumpPower;
            }
            else
                jumpVector = Vector3.up;

            Rigidbody.linearVelocity += jumpVector * jumpPower;

            JumpCooldown = 0;
            return true;
        }
        return false;
    }

    public bool OnDash(Vector3 direction)
    {
        if (DashCooldown >= 1 && ValidatePreconditions() is var state && !state.Equals(MoveState.Failed))
        {
            Rigidbody.AddForce(direction);

            DashCooldown = 0;
            return true;
        }
        return false;
    }

    private void OnCollision()
    {
        ValueThreshold = 0;
    }

    private Vector3 GetWallDirection()
    {
        var contactPoint = simplePlayerControler.LastCollision.GetContact(0);
        return new Vector3 (contactPoint.normal.x, 0, contactPoint.normal.z).normalized * -1;
    }

    private MoveState ValidatePreconditions()
    {
        if (simplePlayerControler.IsGrounded)
        {
            currentMoveState = MoveState.Ground;
            return MoveState.Ground;
        }


        if (olst.TestForCollisions(out var hit))
        {
            wallJumpNormal = hit.HitNormal;
            currentMoveState = MoveState.Wall;
            return MoveState.Wall;
        }
        else
            wallJumpNormal = Vector3.zero;

        if (ValueThreshold < 16)
        {
            ValueThreshold += 8;
        }

        return MoveState.Air;
    }

    public override string ToString()
    {
        return ">JumpControler:" +
            "\n\n MoveState:\t\t" + currentMoveState.ToString() +
            "\n Normal:\t\t" + wallJumpNormal.ToString() ;
    }
}
