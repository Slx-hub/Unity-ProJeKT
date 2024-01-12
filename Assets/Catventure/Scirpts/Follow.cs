using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Target;
    public float Heigth;
    public float Distance;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = -Target.forward * Distance + Vector3.up * Heigth;
        transform.LookAt(Target);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Target);

        Vector3 targetPos = Target.position -Target.forward * Distance + Vector3.up * Heigth;
        Vector3 delta = targetPos - transform.position;

        transform.position += 0.25f * delta * Time.deltaTime;
    }
}
