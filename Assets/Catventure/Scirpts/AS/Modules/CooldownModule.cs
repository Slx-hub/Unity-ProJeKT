using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownModule : AbilityModule
{
    public float CoolDown;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    public override void Execute()
    {
        if (timer < CoolDown)
            ModuleState = ModuleState.Failed;

        timer = 0f;

        base.Execute();
    }
}
