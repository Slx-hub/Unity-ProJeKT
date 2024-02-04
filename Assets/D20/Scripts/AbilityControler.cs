using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityControler : MonoBehaviour, ComboListener
{
    public Ability ab1;
    public Ability ab2;
    public Ability ab3;

    private Ability selectedAbility;

    private EventControler ec;
    public Canvas UICanvas;

    private Ability currentActiveAbility;
    public ShowNearestEntity sne { get; private set; }

    internal void UseAbility1()
    {
        selectedAbility = ab1;
    }
    internal void UseAbility2()
    {
        selectedAbility = ab2;
    }
    internal void UseAbility3()
    {
        selectedAbility = ab3;
    }

    // Start is called before the first frame update
    void Start()
    {
        ec= transform.AddComponent<EventControler>();
        sne = GetComponent<ShowNearestEntity>();
        var comboController = GetComponent<ComboController>();
        comboController.ComboListener = this;
    }

    Transform GetTargetNullable()
    {
        return sne?.currentTarget?.transform;
    }

    public EventControler GetEventControler() { return ec; }

    public void OnComboStageAdvance(int roll)
    {
        if (selectedAbility is not null && selectedAbility.FiresOnComboAdvance)
        {
            var parent = selectedAbility.AttachToParent ? transform : null;
            currentActiveAbility = GameObject.Instantiate(ab1, transform.position, Quaternion.identity, parent);
            currentActiveAbility.ComboAdvanced(this, roll, GetTargetNullable(), UICanvas);
        }
    }

    public void OnComboComplete(int total)
    {
        if (selectedAbility is not null && selectedAbility.FiresOnComboComplete)
        {
            var parent = selectedAbility.AttachToParent ? transform : null;
            currentActiveAbility = GameObject.Instantiate(ab1, transform.position, Quaternion.identity, parent);
            currentActiveAbility.ComboComplete(this, total, GetTargetNullable(), UICanvas);
        }
    }
}
