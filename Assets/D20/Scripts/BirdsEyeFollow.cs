using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsEyeFollow : MonoBehaviour
{
    public GameObject target;
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
    }
}
