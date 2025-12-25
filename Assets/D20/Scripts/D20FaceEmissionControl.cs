using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Rigidbody))]
public class D20FaceEmissionControl : NetworkBehaviour
{
    private Mesh m_mesh;
    private int[] valueToFaceLUT = new int[] { 10, 6, 1, 16, 18, 13, 9, 3, 12, 4, 15, 5, 19, 14, 8, 2, 0, 17, 11, 7 };
    private List<(int, float)> highlightedValues = new();
    private List<int> highlightedFaces = new();

    public AnimationCurve IntensityCurve;
    public float TopSpeed = 5.0f;
    public Color LightColor;
    public MeshRenderer LinkedMR;
    public float BaseValueIntensity = 0.5f;

    private Light PowerLight;
    private D20Controller D20Controller;

    // Start is called before the first frame update
    void Start()
    {
        D20Controller = GetComponent<D20Controller>();
        PowerLight = this.AddComponent<Light>();
        PowerLight.color = LightColor;

        m_mesh = LinkedMR.GetComponent<MeshFilter>().mesh;
        /*var uvList = new List<Vector2>();
        m_mesh.GetUVs(0, uvList);
        var uvs = new Vector2[uvList.Count];
        uvs[0] = Vector2.one;
        uvs[1] = Vector2.one;
        uvs[3] = Vector2.one;
        m_mesh.SetUVs(1, uvs);*/

        //Lets unwrap the verticies

        var origTriangles = m_mesh.triangles;
        var origVertices = m_mesh.vertices;
        var origUVs = m_mesh.uv;

        var newTriangles = new int[origTriangles.Length];
        var newVerticies = new Vector3[origTriangles.Length];
        var newUVs = new Vector2[origTriangles.Length];
        var newUV2s = new Vector2[origTriangles.Length];
        var newUV3s = new Vector2[origTriangles.Length];

        for (int i = 0; i < origTriangles.Length; i++)
        {
            newTriangles[i] = i;
            newVerticies[i] = origVertices[origTriangles[i]];
            newUVs[i] = origUVs[origTriangles[i]];
            newUV2s[i] = Vector2.zero;
            newUV3s[i] = Vector2.zero;
        }


        m_mesh.vertices = newVerticies;
        m_mesh.uv = newUVs;
        m_mesh.uv2 = newUV2s;
        m_mesh.uv3 = newUV3s;
        m_mesh.triangles = newTriangles;

        m_mesh.RecalculateNormals();
    }

    // Update is called once per frame
    public float maxTime = 0.5f;
    private float accumulatedTime = 0.0f;
    public int tface;
    void Update()
    {
        //if (accumulatedTime > maxTime) { accumulatedTime = 0.0f; HighlightRandomValues(); }
        //accumulatedTime+= Time.deltaTime;
    }

    void FixedUpdate()
    {
        var intensity = IntensityCurve.Evaluate(D20Controller.AngularVelocity / TopSpeed);
        LinkedMR.material.SetFloat("_BorderEmissionIntensity", intensity);

        PowerLight.enabled = D20Controller.IsPowered;
    }

    public bool IsValueActive(int val)
    {
        return highlightedValues.Exists(x => x.Item1 == val);
    }

    public void HighlightEvenValues()
    {
        var selectedValues = Enumerable.Range(1, 10).Select(x => x * 2).ToArray();
        HighlightValuesRpc(selectedValues);
    }
    public void HighlightOddValues()
    {
        var selectedValues = Enumerable.Range(0, 10).Select(x => x * 2 + 1).ToArray();
        HighlightValuesRpc(selectedValues);
    }

    public void HighlightRandomValues()
    {
        int numberFaces = UnityEngine.Random.Range(1, 20);
        var rnd = new System.Random();
        var selectedValues = Enumerable.Range(1, 20).OrderBy(x => rnd.Next()).Take(numberFaces).ToArray();
        HighlightValuesRpc(selectedValues);
    }
    public void ClearValueHighlight()
    {
        HighlightValuesRpc(new int[0]);
    }
    public void ClearFaceHighlight()
    {
        HighlightFacesRpc(new int[0]);
    }

    [Rpc(SendTo.Everyone)]
    public void HighlightValuesRpc(int[] values)
    {
        HighlightValuesRpc(values, values.Select(x => 1f).ToArray());        
    }

    [Rpc(SendTo.Everyone)]
    public void HighlightValuesRpc(int[] values, float[] intensities)
    {
        Debug.Assert(values.Length == intensities.Length);

        var uvs = m_mesh.uv2;
        var triangles = m_mesh.triangles;
        Array.Fill(uvs, Vector2.one * BaseValueIntensity);

        highlightedValues.Clear();
        highlightedValues.AddRange(values.Zip(intensities, (x,y) => (x,y)));

        for (int i = 0; i < values.Length; i++)
        {
            int value = values[i];
            float intensity = intensities[i];

            if (intensity < BaseValueIntensity)
                continue;
            uvs[triangles[valueToFaceLUT[value - 1] * 3]] = Vector2.one * intensity;
            uvs[triangles[valueToFaceLUT[value - 1] * 3 + 1]] = Vector2.one * intensity;
            uvs[triangles[valueToFaceLUT[value - 1] * 3 + 2]] = Vector2.one * intensity;
        }

        m_mesh.uv2 = uvs;
    }

    [Rpc(SendTo.Everyone)]
    public void HighlightFacesRpc(int[] faces)
    {
        var uvs = m_mesh.uv3;
        var triangles = m_mesh.triangles;
        Array.Fill(uvs, Vector2.zero);

        highlightedFaces.Clear();
        highlightedFaces.AddRange(faces);

        foreach (var face in faces)
        {
            uvs[triangles[valueToFaceLUT[face - 1] * 3]] = Vector2.one;
            uvs[triangles[valueToFaceLUT[face - 1] * 3 + 1]] = Vector2.one;
            uvs[triangles[valueToFaceLUT[face - 1] * 3 + 2]] = Vector2.one;
        }

        m_mesh.uv3 = uvs;
    }
}
