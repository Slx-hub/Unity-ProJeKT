using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerObserver : MonoBehaviour
{
    public GameObject Target;
    public Slider HealthSlider;
    public Slider JumpSlider;
    public Slider DashSlider;
    public TextMeshProUGUI JumpValueLabel;
    public Image UIGlowBG;

    void Start()
    {
        EventBroker<OnNetworkCreateEvent>.SubscribeChannel(SwitchTarget);
    }

    public void SwitchTarget(OnNetworkCreateEvent onEvent)
    {
        if (onEvent.Root.IsOwner)
        {
            Target = onEvent.Root.gameObject;
        }
    }

    private void FixedUpdate()
    {
        if (Target == null) return;

        var jumpScript = Target.GetComponent<JumpController>();
        var entityScript = Target.GetComponent<Entity>();
        if (entityScript is not null)
            HealthSlider.value = (float)entityScript.Health / entityScript.Health;
        JumpSlider.value = jumpScript.JumpCooldown;
        DashSlider.value = jumpScript.DashCooldown;
        JumpValueLabel.text = jumpScript.ValueThreshold > 0 ? ">" + jumpScript.ValueThreshold : "~";
        UIGlowBG.enabled = Target.GetComponent<D20Controller>().IsPowered;
    }
}
