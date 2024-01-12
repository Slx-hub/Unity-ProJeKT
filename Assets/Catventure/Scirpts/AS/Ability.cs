using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AbilityState
{
    Waiting,
    Aborting,
    Verifying,
    Preparing,
    Activating,
    Executing,
    Finalazing,
    Terminating,
}

public class Ability : MonoBehaviour
{
    private AbilityState state;
    private Dictionary<AbilityState , List<AbilityModule>> modules;

    private List<Transform> targetTransforms = new();
    private List<Vector3> targetLocations = new();


    public Ability()
    {
        modules = new();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = AbilityState.Waiting;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == AbilityState.Waiting) { return; }
        if(!modules.ContainsKey(state)) { return; }
        if (modules[state].Any(m => m.ModuleState == ModuleState.Wait)) { return; }
        if (modules[state].Any(m => m.ModuleState == ModuleState.Failed)) { Abort(); return; }

        if (state == AbilityState.Aborting) { state = AbilityState.Waiting; }
        else { state = (AbilityState)(((int)state + 1) % Enum.GetNames(typeof(AbilityState)).Length); }

        if (modules.ContainsKey(state)) { modules[state].ForEach(m => m.Execute()); }
    }

    public void RegisterModule(AbilityModule ab)
    {
        if (!modules.ContainsKey(ab.targetAbilityState))
            modules[ab.targetAbilityState] = new();

        modules[ab.targetAbilityState].Add(ab);
        ab.ParentAbility = this;
    }

    private void Abort()
    {
        if (modules.ContainsKey(state)) { modules[state].ForEach(m => m.Abort()); } 
        state = AbilityState.Waiting;
    }

    public void Use()
    {
        state = AbilityState.Verifying;

        targetTransforms = new();
        targetLocations= new();
    }

    public void AddTargetTransforms(List<Transform> targetTransforms) { targetTransforms.AddRange(targetTransforms); }
    public void AddTargetLocations(List<Vector3> targetLocations) { targetLocations.AddRange(targetLocations);}
}
