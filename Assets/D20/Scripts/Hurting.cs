using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurting : MonoBehaviour
{
    public int damage;
    public List<Entity> collidingTransforms;
    public float interval = 1f;

    private float innerTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        collidingTransforms = new List<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collidingTransforms.Count > 0)
        {
            if(innerTimer > interval)
            {
                collidingTransforms.ForEach(e => e.Hurt(damage));
                innerTimer = 0f;
            }

            innerTimer += Time.deltaTime;
        }
        else
            innerTimer = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.TryGetComponent<Entity>(out var e))
            collidingTransforms.Add(e);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.TryGetComponent<Entity>(out var e)
            && collidingTransforms.Contains(e))
            collidingTransforms.Remove(e);
    }
}
