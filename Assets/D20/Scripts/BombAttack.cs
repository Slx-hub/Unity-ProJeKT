using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttack : Ability
{
    public float FirePower = 20;
    private float damage;

    public override void ComboComplete(AbilityControler ac, int roll, Transform target, Canvas canvas)
    {
        Fire(ac.transform, roll);
    }

    private void Fire(Transform owner, int roll)
    {
        damage = roll;
        var camera = Camera.main.transform;
        transform.position += camera.forward * (owner.lossyScale.x / 2 + 0.8f);

        var velocity = owner.GetComponent<Rigidbody>().velocity;
        velocity += Quaternion.AngleAxis(-30, camera.right) * camera.forward * FirePower;
        GetComponent<Rigidbody>().velocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
