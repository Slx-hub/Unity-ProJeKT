using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastModule : AbilityModule
{
    public float CasTime;
    private float timer;
    private bool started = false;

    // Update is called once per frame
    void Update()
    {
        if (started) timer += Time.deltaTime;
        if( timer > CasTime) { ModuleState = ModuleState.Success; started = false; }
    }

    public override void Execute()
    {
        timer = 0f;
        started = true;
        ModuleState = ModuleState.Wait;

        base.Execute();
    }
}
