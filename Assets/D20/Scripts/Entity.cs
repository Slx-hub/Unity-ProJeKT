using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.XR;

public class Entity : MonoBehaviour
{
    public int Health;
    public bool invulernalbe = false;
    public int MaxHealth { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = Health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsAlive() { return Health > 0; }
    public void Hurt(int damage, bool force = false)
    {
        if (!force && invulernalbe) return;
        Health -= damage;

        EventBroker<OnHitValueEvent>.PublishEvent(new(damage.ToString(), "red"));

        if (IsAlive()) return;

        enabled= false;

        for(int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if(!child.TryGetComponent<Rigidbody>(out _))
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
