using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OverlapingSphereTest : MonoBehaviour
{
    public float BigSphereRadius = 5f;
    public float SmallSphereCount = 24;
    public float SmallSphereGrowth = 0.1f;
    public float SmallSphereInitialRadius = 0.1f;
    public float SmallSphereMaxRadius = 3f;
    public float SphereRadiiTreshhold = 0.1f;
    public LayerMask LayerMask;

    public struct OverlapingSphereHit
    {
        public Vector3 HitNormal;
        public Collider[] Colliders;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TestForCollisions(out OverlapingSphereHit oOLPH)
    {
        oOLPH = new OverlapingSphereHit() { HitNormal = Vector3.zero, Colliders=new Collider[] { } };
        var colls = Physics.OverlapSphere(transform.position, BigSphereRadius, LayerMask).ToList();

        if (colls.Count == 0)
            return false;

        var BigSphereHalfRadius = BigSphereRadius * 0.5f;

        List<(Vector3 c, float r, Collider coll)> result = new();

        for(int i = 0; i < SmallSphereCount; i++)
        {
            var t = (float)i / (float)SmallSphereCount;
            Vector3 sphereCenter = transform.position 
                + BigSphereHalfRadius * Mathf.Cos(Mathf.PI * 2f * t) * Vector3.left 
                - BigSphereHalfRadius * Mathf.Sin(Mathf.PI * 2f * t) * Vector3.forward;

            for(float r = SmallSphereInitialRadius; r < SmallSphereMaxRadius; r+=SmallSphereGrowth)
            {
                var sColls = Physics.OverlapSphere(sphereCenter, r, LayerMask).ToList();

                if(sColls.Count == 0) continue;

                result.Add((sphereCenter, r, sColls.ElementAt(0)));

                break;
            }
        }

        if(result.Count == 0) return false;

        var hitNormal = Vector3.zero;
        var colliders = new Collider[] { };

        var smallestRadius = result.Min(t => t.r);
        var biggestRadius = smallestRadius + SphereRadiiTreshhold;

        result = result.Where(t => t.r <= biggestRadius && t.r >= smallestRadius).ToList();
        result.ForEach(t => hitNormal += (transform.position - t.c).normalized);
        hitNormal.Normalize();

        oOLPH = new OverlapingSphereHit() { HitNormal= hitNormal, Colliders = result.Select(t => t.coll).ToArray() };
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, BigSphereRadius);
        Gizmos.color = Color.white;

        var colls = Physics.OverlapSphere(transform.position, BigSphereRadius, LayerMask).ToList();

        if (colls.Count == 0)
            return;

        var BigSphereHalfRadius = BigSphereRadius * 0.5f;

        List<(Vector3 c, float r, Collider coll)> result = new();

        for (int i = 0; i < SmallSphereCount; i++)
        {
            var t = (float)i / (float)SmallSphereCount;
            Vector3 sphereCenter = transform.position
                + BigSphereHalfRadius * Mathf.Cos(Mathf.PI * 2f * t) * Vector3.left
                - BigSphereHalfRadius * Mathf.Sin(Mathf.PI * 2f * t) * Vector3.forward;

            for (float r = SmallSphereInitialRadius; r < SmallSphereMaxRadius; r += SmallSphereGrowth)
            {
                var sColls = Physics.OverlapSphere(sphereCenter, r, LayerMask).ToList();

                if (sColls.Count == 0) continue;

                result.Add((sphereCenter, r, sColls.ElementAt(0)));

                break;
            }
        }

        foreach(var x in result)
        {
            Gizmos.DrawSphere(x.c, 0.1f);
            Gizmos.DrawWireSphere(x.c, x.r);
        }

        if (result.Count == 0) return;

        var hitNormal = Vector3.zero;
        var colliders = new Collider[] { };

        var smallestRadius = result.Min(t => t.r);
        var biggestRadius = smallestRadius + SphereRadiiTreshhold;

        result = result.Where(t => t.r <= biggestRadius && t.r >= smallestRadius).ToList();
        result.ForEach(t => hitNormal += (transform.position - t.c).normalized);
        hitNormal.Normalize();

        Gizmos.color= Color.red;
        Gizmos.DrawLine(transform.position, transform.position + hitNormal * 5f);
    }
}
