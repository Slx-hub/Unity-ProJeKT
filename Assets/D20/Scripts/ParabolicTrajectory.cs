using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteInEditMode]
public class ParabolicTrajectory : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 origin;
    public Vector3 target;
    public Vector3 anchor;
    public int segments;
    public float addLatency = 0.1f;
    public float addRemoveDelta = 1f;

    private LineRenderer m_lineRenderer;
    public int currentSegments;

    private float a;
    private float b;
    private float c;

    public float innerTimer;
    public float latencyTimer;
    public bool started = false;
    public bool begin = false;

    void Start()
    {
        m_lineRenderer= GetComponent<LineRenderer>();
        currentSegments = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (begin) Begin();

        if (!started) return;

        if(latencyTimer > addLatency)
        {
            latencyTimer = 0f;

            if (currentSegments < segments)
            {
                if (currentSegments == 0)
                    AddPoint();
                AddPoint();
            }

            if (innerTimer > addRemoveDelta)
            {
                RemovePoint();

                if (m_lineRenderer.positionCount <= 0)
                {
                    Stop();
                }
            }
        }

        innerTimer += Time.deltaTime;
        latencyTimer += Time.deltaTime;
    }

    public void Begin()
    {
        begin = false;
        started = true;
        m_lineRenderer.positionCount = 0;
        m_lineRenderer.SetPositions(new Vector3[] { });

        innerTimer = 0f;
        latencyTimer = 0f;

        currentSegments = 0;

        var dirx = (target - origin).normalized;
        var dirz = Quaternion.AngleAxis(90f, Vector3.up) * dirx;
        var diry = Vector3.Cross(dirz, dirx);

        var len = 0.5f * (target - origin).magnitude;
        var center = 0.5f * (target + origin);
        var a = Random.Range(0, len*2f) - len;
        var b = Random.Range(0, len * 2f) - len;

        anchor = center + diry * a + dirz * b;
    }

    public void Stop()
    {
        started = false;
        m_lineRenderer.positionCount = 0;
        m_lineRenderer.SetPositions(new Vector3[] { });

        innerTimer = 0f;
        latencyTimer = 0f;

        currentSegments = 0;
    }

    public void AddPoint()
    {
        var t = (float)((float)currentSegments / (float)segments);

        Vector3[] v3s = new Vector3[m_lineRenderer.positionCount];
        var c = m_lineRenderer.GetPositions(v3s);
        var poss = v3s.ToList();

        poss.Add(CalculatePoint(transform.position, target, t));

        m_lineRenderer.positionCount = poss.Count;
        m_lineRenderer.SetPositions(poss.ToArray());

        currentSegments++;
    }

    public void RemovePoint()
    {
        Vector3[] v3s = new Vector3[m_lineRenderer.positionCount];
        var c = m_lineRenderer.GetPositions(v3s);
        var poss = v3s.Skip(1).ToList();

        m_lineRenderer.positionCount = poss.Count;
        m_lineRenderer.SetPositions(poss.ToArray());
    }

    private Vector3 CalculatePoint(Vector3 from, Vector3 to, float t)
    {
        return anchor + (1f - t) * (1f - t) * (from - anchor) + t * t * (to - anchor);
    }
}
