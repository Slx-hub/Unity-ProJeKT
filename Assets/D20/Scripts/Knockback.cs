using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent<Rigidbody>(out var rigidbody)) return;

        var dir = (collision.transform.position - transform.position).normalized;
        rigidbody.AddForce(dir * knockbackForce, ForceMode.Impulse);
    }
}
