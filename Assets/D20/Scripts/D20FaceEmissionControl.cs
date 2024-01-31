using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class D20FaceEmissionControl : MonoBehaviour
{
    private Mesh m_mesh;
    private int[] valueToFaceLUT = new int[] { 10, 6, 1, 16, 18, 13, 9, 3, 12, 4, 15, 5, 19, 14, 8, 2, 0, 17, 11, 7 };
    private List<int> highlightedValues = new();

    public AnimationCurve IntensityCurve;
    public float TopSpeed = 5.0f;
    public Color LightColor;
    public Image UIGlowBG;

    private Light PowerLight;

    MeshRenderer LinkedMR;
    Rigidbody LinkedRB;

    private List<float> angularVelocities = new();

    // Start is called before the first frame update
    void Start()
    {

        LinkedMR = GetComponent<MeshRenderer>();
        LinkedRB = GetComponent<Rigidbody>();
        PowerLight = this.AddComponent<Light>();
        PowerLight.color = LightColor;

        m_mesh = GetComponent<MeshFilter>().mesh;
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

        for (int i = 0; i < origTriangles.Length; i++)
        {
            newTriangles[i] = i;
            newVerticies[i] = origVertices[origTriangles[i]];
            newUVs[i] = origUVs[origTriangles[i]];
            newUV2s[i] = Vector2.zero;
        }


        m_mesh.vertices = newVerticies;
        m_mesh.uv = newUVs;
        m_mesh.uv2 = newUV2s;
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
        angularVelocities.Add(LinkedRB.angularVelocity.magnitude);
        if (angularVelocities.Count > 50)
            angularVelocities.RemoveAt(0);

        var intensity = IntensityCurve.Evaluate(angularVelocities.Average() / TopSpeed);
        LinkedMR.material.SetFloat("_BorderEmissionIntensity", intensity);

        var isPowered = angularVelocities.Average() > 3;
        PowerLight.enabled = isPowered;
        if (UIGlowBG != null)
            UIGlowBG.enabled = isPowered;
    }

    public bool IsValueActive(int val)
    {
        return highlightedValues.Contains(val);
    }

    public void HighlightEvenValues()
    {
        var selectedValues = Enumerable.Range(1, 10).Select(x => x * 2).ToArray();
        HighlightValues(selectedValues);
    }
    public void HighlightOddValues()
    {
        var selectedValues = Enumerable.Range(0, 10).Select(x => x * 2 + 1).ToArray();
        HighlightValues(selectedValues);
    }

    public void HighlightRandomValues()
    {
        int numberFaces = UnityEngine.Random.Range(1, 20);
        var rnd = new System.Random();
        var selectedValues = Enumerable.Range(1, 20).OrderBy(x => rnd.Next()).Take(numberFaces).ToArray();
        HighlightValues(selectedValues);
    }

    public void ClearHighlight()
    {
        HighlightValues(new int[0]);
    }

    public void HighlightValues(int[] values)
    {
        var uvs = m_mesh.uv2;
        var triangles = m_mesh.triangles;
        Array.Fill(uvs, Vector2.zero);
        
        highlightedValues.Clear();
        highlightedValues.AddRange(values);
        foreach (var value in values)
        {
            uvs[triangles[valueToFaceLUT[value - 1] * 3]] = Vector2.one;
            uvs[triangles[valueToFaceLUT[value - 1] * 3 + 1]] = Vector2.one;
            uvs[triangles[valueToFaceLUT[value - 1] * 3 + 2]] = Vector2.one;
        }
        m_mesh.uv2 = uvs;
    }
}
