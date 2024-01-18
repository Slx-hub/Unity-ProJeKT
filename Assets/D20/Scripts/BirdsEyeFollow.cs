using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BirdsEyeFollow : MonoBehaviour
{
    public GameObject target;
    public Transform CameraRotation;
    public float height;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        transform.position = target.transform.position + Vector3.up * height;
        transform.rotation = Quaternion.Euler(90, CameraRotation.rotation.eulerAngles.y, 0);
    }
}
