using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.XR;

public class Entity : NetworkBehaviour
{
    //Client will need to check health and it should always be precise
    private NetworkVariable<int> n_health;
    public int Health
    {
        get { return n_health.Value; }
        private set { n_health.Value = value; }
    }
    public int I_Health;

    private NetworkVariable<int> n_maxHealth;
    public int MaxHealth
    {
        get { return n_maxHealth.Value; }
        private set { n_maxHealth.Value = value; }
    }

    //Does not need to be NetworkVariable because invulernalbe checks are done only serverside
    public bool invulernalbe = false;

    // Start is called before the first frame update
    void Awake()
    {
        n_health = new NetworkVariable<int>(I_Health);
        n_maxHealth = new NetworkVariable<int>(Health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Server has authority

    public bool IsAlive() { return n_health.Value > 0; }


    [Rpc(SendTo.Server)]
    public void HurtRpc(int damage, bool force = false, RpcParams rpcParams = default)
    {
        if (!force && invulernalbe) return;

        n_health.Value -= damage;
        TakeDamageRpc(damage, RpcTarget.Single(rpcParams.Receive.SenderClientId, RpcTargetUse.Temp));

        if (IsAlive()) return;

        ExplodeRpc();
    }

    //Damage should only be displayed by the client that actually did the damage
    [Rpc(SendTo.SpecifiedInParams)]
    private void TakeDamageRpc(int damage, RpcParams rpcParams)
    {
        EventBroker<OnHitValueEvent>.PublishEvent(new(damage.ToString(), "red"));
    }

    //Entity component should be disabled on every client.
    [Rpc(SendTo.Everyone)]
    public void DisableEntityRpc()
    {
        enabled = false;
    }

    [Rpc(SendTo.Everyone)]
    private void ExplodeRpc()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (!child.TryGetComponent<Rigidbody>(out _))
            {
                var r = child.AddComponent<Rigidbody>();
                r.mass = 1;
                r.AddForce(new Vector3(Random.value, Random.value, Random.value) * 100f);
                child.SetParent(null);
                i--;
            }
        }

        if (!TryGetComponent<Rigidbody>(out _))
        {
            var r = transform.AddComponent<Rigidbody>();
            r.mass = 1;
            r.AddForce(new Vector3(Random.value, Random.value, Random.value) * 100f);
            transform.SetParent(null);
        }
    }
}
