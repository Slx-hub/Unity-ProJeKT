using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Netcode;
using UnityEngine;

[ExecuteInEditMode]
public class ParabolicTrajectory : NetworkBehaviour
{
    // Start is called before the first frame update
    public NetworkVariable<Transform> originTransform;
    public NetworkVariable<Vector3> target;
    public Vector3 anchor;
    public NetworkVariable<int> segments;
    public NetworkVariable<float> addLatency = new(0.1f);
    public NetworkVariable<float> addRemoveDelta = new(1f);

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

        if(latencyTimer > addLatency.Value)
        {
            latencyTimer = 0f;

            if (currentSegments < segments.Value)
            {
                if (currentSegments == 0)
                    AddPoint();
                AddPoint();
            }

            if (innerTimer > addRemoveDelta.Value)
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
    public void Init(Transform transform, Vector3 targetLocation, float delta, float latency, int segs)
    {
        originTransform = new(transform);
        target = new(targetLocation);
        addRemoveDelta= new(delta);
        addLatency= new(latency);
        segments = new(segs);
    }

    [ServerRpc]
    public void Begin_ServerRPC()
    {
        if (IsServer)
            Begin_ClientRPC();
        Begin();
    }

    [ClientRpc]
    public void Begin_ClientRPC()
    {
        Begin();
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

        var dirx = (target.Value - originTransform.Value.transform.position).normalized;
        var dirz = Quaternion.AngleAxis(90f, Vector3.up) * dirx;
        var diry = Vector3.Cross(dirz, dirx);

        var len = 0.5f * (target.Value - originTransform.Value.transform.position).magnitude;
        var center = 0.5f * (target.Value + originTransform.Value.transform.position);
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
        var t = (float)((float)currentSegments / (float)segments.Value);

        Vector3[] v3s = new Vector3[m_lineRenderer.positionCount];
        var c = m_lineRenderer.GetPositions(v3s);
        var poss = v3s.ToList();

        poss.Add(CalculatePoint(transform.position, target.Value, t));

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
