using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        Label.text = ValueThreshold > 0 ? ">" + ValueThreshold : "~";
    }

    public bool OnJump()
    {
        if (JumpCooldown >= 1 && ValidatePreconditions())
        {
            JumpCooldown = 0;
            PlaySound(SoundManager.GetSoundByName("boing"));
            return true;
        }
        return false;
    }
    public bool OnDash()
    {
        if (DashCooldown >= 1 && ValidatePreconditions())
        {
            DashCooldown = 0;
            PlaySound(SoundManager.GetSoundByName("woosh"));
            return true;
        }
        return false;
    }

    private void OnCollision()
    {
        IsEnergized = true;
        ValueThreshold = 0;
    }

    private void PlaySound(AudioClip clip)
    {

        D20Controller.AudioSource.pitch = 0.5f + (1 - D20Controller.CurrentFaceValue / 20f);
        D20Controller.AudioSource.volume = 0.25f + D20Controller.CurrentFaceValue / 30f;

        D20Controller.AudioSource.PlayOneShot(clip);
    }

    private bool ValidatePreconditions()
    {
        var result = D20Controller.IsGrounded || IsEnergized && D20Controller.IsPowered;

        if (result && !D20Controller.IsGrounded)
        {
            if (ValueThreshold < 15)
            {
                ValueThreshold += 5;
            }
            if (D20Controller.CurrentFaceValue <= ValueThreshold)
            {
                PlaySound(SoundManager.GetSoundByName("poop"));
                IsEnergized = false;
                JumpCooldown = 0;
                DashCooldown = 0;
            }
        }
        return result;
    }
}
