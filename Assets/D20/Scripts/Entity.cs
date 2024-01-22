using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int Health;
    public bool invulernalbe = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsAlive() { return Health > 0; }
    public void Hurt(int damage)
    {
        if (invulernalbe) return; 
        
        Health -= damage;

        if (IsAlive()) return;

        enabled= false;

        foreach(Transform child in transform)
        {
            if(!child.TryGetComponent<Rigidbody>(out _))
            {
                var r = child.AddComponent<Rigidbody>();
                r.mass = 1;
                child.SetParent(null);
            }
        }
    }
}
