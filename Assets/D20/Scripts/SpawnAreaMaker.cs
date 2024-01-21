using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class SpawnAreaMaker : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Vector3> points;
    public List<int> triangles;
    public List<float> weights;

    private List<Vector3> testPoints;

    void Start()
    {
    }

    private void Reset()
    {
        InitSpawnArea();

        testPoints = new List<Vector3>();

        for (int i = 0; i < 1000; i++)
            testPoints.Add(GetRandomPointInSpawnArea());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < triangles.Count; i += 3)
        {
            var p1 = points[triangles[i]];
            var p2 = points[triangles[i + 1]];
            var p3 = points[triangles[i + 2]];

            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p1);
        }

        Gizmos.color = Color.red;

        if (testPoints == null) return;

        foreach (var tp in testPoints)
        {
            Gizmos.DrawSphere(tp, 0.125f);
        }
    }

    public Vector3 GetRandomPointInSpawnArea()
    {
        var rnd = Random.value;
        var triangleIndex = -1;
        var currentSum = 0f;

        for(int i = 0; i< weights.Count; i++)
        {
            currentSum += weights[i];
            if(rnd <= currentSum)
            {
                triangleIndex= i;
                break;
            }
        }

        var p1 = points[triangles[triangleIndex * 3]];
        var p2 = points[triangles[triangleIndex * 3 + 1]];
        var p3 = points[triangles[triangleIndex * 3 + 2]];

        var a = p2 - p1;
        var b = p3 - p1;

        var u = Random.value;
        var v = Random.value;
        var pr = new Vector3();

        if(u + v <= 1f)
        {
            pr = a * u + b * v;
        }else
        {

            pr = a * (1f - u) + b * (1f - v);
        }

        return pr + p1;
    }

    private void InitSpawnArea()
    {
        points = new List<Vector3>();

        var meshCollider = GetComponent<MeshCollider>();
        var mesh = meshCollider.sharedMesh;

        var consideredVertexIndexLookUp = new int[mesh.vertices.Length];
        var consideredVertexIndexList = new List<int>();
        var consideredTriangleList = new List<int>();

        var middleHeigth = mesh.vertices.Average(v => v.y);

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            if (mesh.vertices[i].y < middleHeigth) continue;

            consideredVertexIndexLookUp[i] = points.Count();
            points.Add(transform.TransformPoint(mesh.vertices[i]));
            consideredVertexIndexList.Add(i);
        }

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            int t1 = mesh.triangles[i];
            int t2 = mesh.triangles[i + 1];
            int t3 = mesh.triangles[i + 2];

            if (!consideredVertexIndexList.Contains(t1)
                || !consideredVertexIndexList.Contains(t2)
                || !consideredVertexIndexList.Contains(t3))
                continue;

            triangles.Add(consideredVertexIndexLookUp[t1]);
            triangles.Add(consideredVertexIndexLookUp[t2]);
            triangles.Add(consideredVertexIndexLookUp[t3]);
        }

        CalculateWeigths();
    }

    private void CalculateWeigths()
    {
        List<float> areaList = new List<float>();

        for (int i = 0; i < triangles.Count; i += 3)
        {
            var p1 = points[triangles[i]];
            var p2 = points[triangles[i + 1]];
            var p3 = points[triangles[i + 2]];

            var d1 = p2 - p1;
            var d2 = p3 - p1;

            var l1 = d1.magnitude;
            var l2 = d2.magnitude;

            var angle = Mathf.Acos(Vector3.Dot(d1, d2) / (l1 * l2));

            areaList.Add(0.5f * l1 * l2 * angle);
        }

        var totalArea = areaList.Sum();

        for(int i = 0; i < areaList.Count; i++) 
        {
            weights.Add(areaList[i] / totalArea);
        }
    }
}