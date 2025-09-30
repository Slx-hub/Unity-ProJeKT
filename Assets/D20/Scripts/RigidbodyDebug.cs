using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RigidbodyDebug : MonoBehaviour
{
    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody= GetComponent<Rigidbody>();
    }

    private void Reset()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();

        var vel = rigidbody.linearVelocity;
        var aVel = rigidbody.angularVelocity;

        Gizmos.color = Color.green;
        DrawArrow(transform.position, transform.position + vel);
        Gizmos.color = Color.red;
        DrawArrow(transform.position, transform.position + aVel);
    }

    private void DrawArrow(Vector3 lStart, Vector3 lEnd)
    {
        var lLen = (lEnd - lStart).magnitude;
        var lDir = (lEnd - lStart).normalized;
        var cDir = Camera.main.transform.forward;
        var oDir = Camera.main.transform.right;

        var leftArrowPoint = lStart + lDir * lLen * 0.875f + oDir * lLen * 0.125f;
        var rightArrowPoint = lStart + lDir * lLen * 0.875f - oDir * lLen * 0.125f;

        Gizmos.DrawLine(lStart, lEnd);
        Gizmos.DrawLine(lEnd, leftArrowPoint);
        Gizmos.DrawLine(lEnd, rightArrowPoint);
    }
}
