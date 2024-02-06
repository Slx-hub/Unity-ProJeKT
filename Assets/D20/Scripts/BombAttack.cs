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
    private ulong ownerid;

    public override void ComboComplete(AbilityControler ac, int roll, Transform target, Canvas canvas, ulong ownerid)
    {
        this.ownerid = ownerid;
        Fire(ac.transform, roll);
    }

    private void Fire(Transform owner, int roll)
    {
        damage = roll;
        var camera = Camera.main.transform;
        transform.position += camera.forward * (owner.lossyScale.x / 2 + 0.8f);

        var velocity = owner.GetComponent<Rigidbody>().velocity;
        velocity += Quaternion.AngleAxis(-30, camera.right) * camera.forward * FirePower;
        GetComponent<Rigidbody>().velocity += velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        var explosion = GameObject.Instantiate(Explosion, transform.position, Quaternion.identity);
        explosion.GetComponent<NetworkObject>().SpawnWithOwnership(ownerid, true);
        explosion.GetComponent<Explosion>().Damage = damage;
        Destroy(gameObject);
    }
}
