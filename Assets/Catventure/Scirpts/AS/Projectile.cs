using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    public float LifeTime;

    public Action<Transform> transformCallback;
    public Action<Vector3> locationCallback;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * Speed *Time.deltaTime);

        timer += Time.deltaTime;
        if(LifeTime != 0f && timer > LifeTime) { Die(); }
    }

    private void OnCollisionEnter(Collision collision)
    {
        transformCallback(collision.transform);
        GameObject.DestroyImmediate(this);
    }

    private void Die() { locationCallback(transform.position); GameObject.DestroyImmediate(this.gameObject); }
}
