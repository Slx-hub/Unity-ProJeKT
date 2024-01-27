using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public enum MoveState
{
    Ground, Wall, Air, Failed
}

public class JumpController : MonoBehaviour
{
    public Slider JumpSlider;
    public Slider DashSlider;
    public TextMeshProUGUI Label;
    public float CooldownTime = 1f;

    public float SphereCastRadius = 1f;

    private float JumpCooldown = 0;
    private float DashCooldown = 0;
    private int ValueThreshold = 0;
    private bool IsEnergized = true;

    private D20Controller D20Controller;
    private Rigidbody Rigidbody;
    private OverlapingSphereTest olst;

    //Only for debug porpuses
    private MoveState currentMoveState;
    private Vector3 wallJumpNormal = Vector3.zero;

    void Start()
    {
        D20Controller = GetComponent<D20Controller>();
        D20Controller.CollisionListener = OnCollision;
        Rigidbody = GetComponent<Rigidbody>();
        olst = GetComponent<OverlapingSphereTest>();
    }

    void FixedUpdate()
    {
        if (IsEnergized)
        {
            if (JumpCooldown < 1)
                JumpCooldown += Time.deltaTime / CooldownTime;
            if (DashCooldown < 1)
                DashCooldown += Time.deltaTime / CooldownTime;
        }
        JumpSlider.value = JumpCooldown;
        DashSlider.value = DashCooldown;
        Label.text = ValueThreshold > 0 ? ">" + ValueThreshold : "~";
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
            }
            else
                jumpVector = Vector3.up;

            Rigidbody.velocity += jumpVector * jumpPower;
            var quat = Quaternion.FromToRotation(Vector3.up, Vector3.right);
            Rigidbody.angularVelocity = quat * jumpVector * jumpPower;

            JumpCooldown = 0;
            PlaySound("boing", true);
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
            PlaySound("woosh", true);
            return true;
        }
        return false;
    }

    private void OnCollision()
    {
        IsEnergized = true;
        if (ValueThreshold > 0)
            PlaySound("woop", false);
        ValueThreshold = 0;
    }

    private Vector3 GetWallDirection()
    {
        var contactPoint = D20Controller.LastCollision.GetContact(0);
        return new Vector3 (contactPoint.normal.x, 0, contactPoint.normal.z).normalized * -1;
    }

    private void PlaySound(string clip, bool changePitch)
    {
        if (changePitch)
        {
            D20Controller.AudioSource.pitch = 0.5f + (1 - D20Controller.CurrentFaceValue / 20f);
            D20Controller.AudioSource.volume = 0.25f + D20Controller.CurrentFaceValue / 30f;
            D20Controller.AudioSource.PlayOneShot(SoundManager.GetSoundByName(clip));
        } else
        {
            D20Controller.PlaySoundRandomPitch(clip);
        }
        
    }

    private MoveState ValidatePreconditions()
    {
        if (D20Controller.IsGrounded)
        {
            currentMoveState = MoveState.Ground;
            return MoveState.Ground;
        }

        var isReady = IsEnergized && D20Controller.IsPowered;

        if (isReady)
        {
            if (olst.TestForCollisions(out var hit))
            {
                wallJumpNormal = hit.HitNormal;
                currentMoveState = MoveState.Wall;
                return MoveState.Wall;
            }
            else
                wallJumpNormal = Vector3.zero;

            if (D20Controller.CurrentFaceValue <= ValueThreshold)
            {
                PlaySound("poop", true);
                IsEnergized = false;
                JumpCooldown = 0;
                DashCooldown = 0;
                currentMoveState = MoveState.Failed;
                return MoveState.Failed;
            }
            if (ValueThreshold < 16)
            {
                ValueThreshold += 8;
            }
        }

        currentMoveState = isReady ? MoveState.Air : MoveState.Failed;
        return isReady ? MoveState.Air : MoveState.Failed;
    }

    public override string ToString()
    {
        return ">JumpControler:" +
            "\n\n MoveState:\t\t" + currentMoveState.ToString() +
            "\n Normal:\t\t" + wallJumpNormal.ToString() ;
    }
}
