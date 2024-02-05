using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float timeToExpand = 0.3f;
    public float timeToFade = 0.3f;
    public float maxSize = 4;
    public MeshRenderer Mesh;

    private float timePassed = 0;

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed < timeToExpand)
            transform.localScale = Vector3.one * maxSize * (timePassed / timeToExpand);
        else if (timePassed < timeToFade + timeToFade)
            Mesh.material.SetFloat("_Fade", 1 - timePassed / (timeToFade + timeToFade));
        else
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var entity = collision.gameObject.GetComponent<Entity>();
        entity?.Hurt(20);
    }
}
