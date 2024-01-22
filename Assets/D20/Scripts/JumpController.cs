using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpController : MonoBehaviour
{
    public Slider JumpSlider;
    public Slider DashSlider;
    public AudioSource AudioSource;
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
    }

    private void PlaySound(AudioClip clip)
    {
        AudioSource.pitch = Random.Range(0.9f, 1.1f);
        AudioSource.volume = Random.Range(0.8f, 1f);

        AudioSource.PlayOneShot(clip);
    }

    private bool ValidatePreconditions()
    {
        var result = D20Controller.IsGrounded || IsEnergized && D20Controller.IsPowered;
        IsEnergized |= D20Controller.IsGrounded;

        if (result && !D20Controller.IsGrounded && D20Controller.CurrentFaceValue <= ValueThreshold)
        {
            PlaySound(SoundManager.GetSoundByName("poop"));
            IsEnergized = false;
            JumpCooldown = 0;
            DashCooldown = 0;
        }
        return result;
    }
}
