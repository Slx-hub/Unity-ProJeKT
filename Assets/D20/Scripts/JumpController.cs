using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private float JumpCooldown = 0;
    private float DashCooldown = 0;
    private int ValueThreshold = 0;
    private bool IsEnergized = true;

    private D20Controller D20Controller;
    private Rigidbody Rigidbody;

    void Start()
    {
        D20Controller = GetComponent<D20Controller>();
        D20Controller.CollisionListener = OnCollision;
        Rigidbody = GetComponent<Rigidbody>();
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
                var normal = GetWallDirection();
                jumpVector = new Vector3(normal.x, 1, normal.z);
                Rigidbody.angularVelocity = Vector3.Reflect(Rigidbody.angularVelocity, normal);
            }
            else
                jumpVector = Vector3.up;
            Rigidbody.velocity += jumpVector * jumpPower;

            JumpCooldown = 0;
            PlaySound(SoundManager.GetSoundByName("boing"), true);
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
            PlaySound(SoundManager.GetSoundByName("woosh"), true);
            return true;
        }
        return false;
    }

    private void OnCollision()
    {
        IsEnergized = true;
        if (ValueThreshold > 0)
            PlaySound(SoundManager.GetSoundByName("woop"), false);
        ValueThreshold = 0;
    }

    private Vector3 GetWallDirection()
    {
        var contactPoint = D20Controller.LastCollision.GetContact(0);
        return new Vector3 (contactPoint.normal.x, 0, contactPoint.normal.z).normalized * -1;
    }

    private void PlaySound(AudioClip clip, bool changePitch)
    {
        if (changePitch)
        {
            D20Controller.AudioSource.pitch = 0.5f + (1 - D20Controller.CurrentFaceValue / 20f);
            D20Controller.AudioSource.volume = 0.25f + D20Controller.CurrentFaceValue / 30f;
        } else
        {
            D20Controller.AudioSource.pitch = 1f;
            D20Controller.AudioSource.volume = 1f;
        }

        D20Controller.AudioSource.PlayOneShot(clip);
    }

    private MoveState ValidatePreconditions()
    {
        if (D20Controller.IsGrounded)
            return MoveState.Ground;

        var isReady = IsEnergized && D20Controller.IsPowered;

        if (isReady)
        {
            if (D20Controller.IsContactingWall)
                return MoveState.Wall;

            if (D20Controller.CurrentFaceValue <= ValueThreshold)
            {
                PlaySound(SoundManager.GetSoundByName("poop"), true);
                IsEnergized = false;
                JumpCooldown = 0;
                DashCooldown = 0;
                return MoveState.Failed;
            }
            if (ValueThreshold < 16)
            {
                ValueThreshold += 8;
            }
        }
        return isReady ? MoveState.Air : MoveState.Failed;
    }
}
