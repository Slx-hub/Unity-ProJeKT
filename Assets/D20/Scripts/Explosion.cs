using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float timeToExpand = 0.3f;
    public float timeToFade = 0.3f;
    public float minSize = 4;
    public float sizeMultiplier = 0.3f;
    public float damageMultiplier = 0.5f;
    public int Damage { get; set; }

    private float timePassed = 0;
    private List<Entity> damagedEntities = new();
    private MeshRenderer Mesh;

    private void Start()
    {
        Mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed < timeToExpand)
            transform.localScale = Vector3.one * (minSize + Damage * sizeMultiplier * (timePassed / timeToExpand));
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
        if (entity != null && !damagedEntities.Contains(entity))
        {
            var actualDamage = (int)(Damage * (1 - Mathf.Min(1, timePassed / timeToExpand) * damageMultiplier));
            damagedEntities.Add(entity);
            entity.HurtRpc(actualDamage);
            Debug.Log($"BAM! {actualDamage} damage!");
        }
    }
}
