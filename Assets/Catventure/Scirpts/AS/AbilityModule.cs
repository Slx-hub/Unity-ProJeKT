using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum ModuleState
{
    Ignore,
    Wait,
    Success,
    Failed
}

public class AbilityModule : MonoBehaviour
{
    public AbilityState targetAbilityState;

    private Ability _parentAbility;
    public Ability ParentAbility { get => _parentAbility; set => _parentAbility = value; }

    private ModuleState _moduleState;
    public ModuleState ModuleState { get => _moduleState; set => _moduleState = value; }

    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Ability>().RegisterModule(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Execute() { }

    public virtual void Abort() { }
}
