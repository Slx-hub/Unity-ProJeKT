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

    public void OnComboStageAdvance(int roll)
    {
        if (ValidateAbility() && abilities[selectedAbility].FiresOnComboAdvance)
        {
            if (abilities[selectedAbility].IsNetworkAbility)
                MultiplayerCastServerRpc(selectedAbility, roll, Camera.main.transform.forward, false);
            else
                CastAbility(roll, false);
        }
    }

    public void OnComboComplete(int total)
    {
        if (ValidateAbility() && abilities[selectedAbility].FiresOnComboComplete)
        {
            if (abilities[selectedAbility].IsNetworkAbility)
                MultiplayerCastServerRpc(selectedAbility, total, Camera.main.transform.forward, true);
            else
                CastAbility(total, true);
        }
    }

    private void CastAbility(int val, bool comboComplete)
    {
        var parent = abilities[selectedAbility].AttachToParent ? transform : null;
        currentActiveAbility = GameObject.Instantiate(abilities[selectedAbility], transform.position, Quaternion.identity, parent);
        //currentActiveAbility.GetComponent<NetworkObject>().Spawn(true);
        currentActiveAbility.ComboAdvanced(this, val, GetTargetNullable(), Camera.main.transform.forward, UICanvas, comboComplete);
    }

    [ServerRpc]
    private void MultiplayerCastServerRpc(int selectedAbility, int val, Vector3 direction, bool comboComplete)
    {
        currentActiveAbility = GameObject.Instantiate(abilities[selectedAbility], transform.position, Quaternion.identity);
        currentActiveAbility.GetComponent<NetworkObject>().Spawn(true);
        currentActiveAbility.ComboAdvanced(this, val, GetTargetNullable(), direction, UICanvas, comboComplete);
    }
}
