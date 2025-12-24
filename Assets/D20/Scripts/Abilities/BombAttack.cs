using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BombAttack : Ability
{
    public GameObject Explosion;
    public float FirePower = 20;
    private int damage;
    private Vector3 direction;

    public override void ComboComplete(AbilityControler ac, int roll, Transform target, Vector3 direction, Canvas canvas, bool comboComplete)
    {
        damage = roll;
        Fire(ac.transform, direction);
    }

    public override void ComboFailed(AbilityControler ac, int roll, Transform target, Vector3 direction, Canvas canvas, bool comboComplete)
    {
        base.ComboFailed(ac, roll, target, direction, canvas, comboComplete);
    }

    private void Fire(Transform owner, Vector3 dir)
    {
        transform.position += dir * (owner.lossyScale.x / 2 + 0.8f);
        Vector3 right = new(dir.z, 0, -dir.x);

        var velocity = owner.GetComponent<Rigidbody>().linearVelocity;
        velocity += Quaternion.AngleAxis(-30, right) * dir * FirePower;
        GetComponent<Rigidbody>().linearVelocity += velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsOwner)
        {
            Explode();
        }
    }

    private void Explode()
    {
        var explosion = GameObject.Instantiate(Explosion, transform.position, Quaternion.identity);
        explosion.GetComponent<NetworkObject>().Spawn(true);
        explosion.GetComponent<Explosion>().Damage = damage;
        Destroy(gameObject);
    }
}
