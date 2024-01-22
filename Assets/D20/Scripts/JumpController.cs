using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpController : MonoBehaviour
{
    public Slider JumpSlider;
    public Slider DashSlider;
    public float CooldownTime = 1f;
    public int ValueThreshold = 5;

    private float JumpCooldown = 0;
    private float DashCooldown = 0;
    private bool IsEnergized = true;

    private D20Controller D20Controller;

    void Start()
    {
        D20Controller = GetComponent<D20Controller>();
        D20Controller.CollisionListener = OnCollision;
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
    }

    public bool OnJump()
    {
        if (JumpCooldown >= 1 && ValidatePreconditions())
        {
            JumpCooldown = 0;
            return true;
        }
        return false;
    }
    public bool OnDash()
    {
        if (DashCooldown >= 1 && ValidatePreconditions())
        {
            DashCooldown = 0;
            return true;
        }
        return false;
    }

    private void OnCollision()
    {
        IsEnergized = true;
    }

    private bool ValidatePreconditions()
    {
        var result = D20Controller.IsGrounded || IsEnergized && D20Controller.IsPowered;
        IsEnergized |= D20Controller.IsGrounded;

        if (result && !D20Controller.IsGrounded && D20Controller.CurrentFaceValue <= ValueThreshold)
        {
            IsEnergized = false;
            JumpCooldown = 0;
            DashCooldown = 0;
        }
        return result;
    }
}
