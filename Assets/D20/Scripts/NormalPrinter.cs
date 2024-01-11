using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPrinter : MonoBehaviour
{
    private List<Vector3> FaceCenters = new List<Vector3>();
    private List<Vector3> FaceNormals = new List<Vector3>();

    void Start()
    {
        var normals = GetComponent<MeshFilter>().mesh.normals;
        var vertices = GetComponent<MeshFilter>().mesh.vertices;
        var triangles = GetComponent<MeshFilter>().mesh.triangles;

        for (int triangle = 0; triangle < triangles.Length / 3; triangle++) {
            var P1 = vertices[triangles[triangle * 3]];
            var P2 = vertices[triangles[triangle * 3 + 1]];
            var P3 = vertices[triangles[triangle * 3 + 2]];


            FaceCenters.Add((P1 + P2 + P3) / 3);


            P1 = normals[triangles[triangle * 3]];
            P2 = normals[triangles[triangle * 3 + 1]];
            P3 = normals[triangles[triangle * 3 + 2]];


            FaceNormals.Add((P1 + P2 + P3) / 3);
        }

        string String = "";

        foreach (var face in FaceNormals)
        {
            String += face.ToString() + "\n";
        }
        Debug.Log(String);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < FaceCenters.Count; i++)
            Gizmos.DrawLine(transform.position + FaceCenters[i], transform.position + FaceCenters[i] + FaceNormals[i]);
    }
}
