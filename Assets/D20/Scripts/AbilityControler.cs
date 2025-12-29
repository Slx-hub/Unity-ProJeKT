using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityControler : NetworkBehaviour, ComboListener
{
    public List<Ability> abilities;

    private int selectedAbility;

    private EventControler ec;
    public Canvas UICanvas;

    private Ability currentActiveAbility;
    public ShowNearestEntity sne { get; private set; }

    internal void UseAbility(int num)
    {
        selectedAbility = num;
    }

    // Start is called before the first frame update
    void Start()
    {
        ec= transform.AddComponent<EventControler>();
        sne = GameObject.FindAnyObjectByType<ShowNearestEntity>();
        var comboController = GetComponent<ComboController>();
        UICanvas = GameObject.Find("UI")?.GetComponent<Canvas>();
        comboController.ComboListener = this;
    }

    Transform GetTargetNullable()
    {
        return sne?.currentTarget?.transform;
    }

    public EventControler GetEventControler() { return ec; }

    private bool ValidateAbility()
    {
        return selectedAbility >= 0 && selectedAbility < abilities.Count;
    }

    public void OnComboStart()
    {
        if (ValidateAbility() && abilities[selectedAbility].FiresOnComboStart)
        {
            CastAbilityRpc(selectedAbility);
        }
    }

    public void OnComboStageAdvance(int roll)
    {
        if (ValidateAbility() && abilities[selectedAbility].FiresOnComboAdvance)
        {
            currentActiveAbility.ComboAdvanced(this, roll, GetTargetNullable(), Camera.main.transform.forward, UICanvas, false);
        }
    }

    public void OnComboComplete(int total)
    {
        if (ValidateAbility() && abilities[selectedAbility].FiresOnComboComplete)
        {
            currentActiveAbility.ComboComplete(this, total, GetTargetNullable(), Camera.main.transform.forward, UICanvas, true);
        }
    }

    public void OnComboFail(int roll)
    {
        if (ValidateAbility() && abilities[selectedAbility].FiresOnComboStart)
        {
            currentActiveAbility.ComboFailed(this, roll, GetTargetNullable(), Camera.main.transform.forward, UICanvas, false);
        }
    }

    private void CastAbility(int val, bool comboComplete)
    {
        var parent = abilities[selectedAbility].AttachToParent ? transform : null;
        currentActiveAbility = GameObject.Instantiate(abilities[selectedAbility], transform.position, Quaternion.identity, parent);
        currentActiveAbility.GetComponent<Ability>().Owner = gameObject;
    }

    [Rpc(SendTo.Server)]
    private void CastAbilityRpc(int selectedAbility, RpcParams rpcParams = default)
    {
        var newAbility = GameObject.Instantiate(abilities[selectedAbility], transform.position, Quaternion.identity);
        newAbility.GetComponent<NetworkObject>().Spawn(true);

        CastAbilityCompletedRpc(newAbility.NetworkObjectId, RpcTarget.Single(rpcParams.Receive.SenderClientId, RpcTargetUse.Temp));
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void CastAbilityCompletedRpc(ulong netObjId, RpcParams rpcParams)
    {            
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(netObjId, out var shouldBeAbility);

        currentActiveAbility = shouldBeAbility.GetComponent<Ability>();
        currentActiveAbility.Owner = gameObject;
        currentActiveAbility.ComboStart(this, -1, GetTargetNullable(), Camera.main.transform.forward, UICanvas, false);
    }
}
