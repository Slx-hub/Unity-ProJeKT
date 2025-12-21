using UnityEngine;
using UnityEngine.Rendering;

public class OrbitAround : MonoBehaviour
{
    public Transform target;
    public float distance;
    public float speed;
    public float heigth; 

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position 
            + new Vector3(
                distance * Mathf.Sin(Time.time * speed),
                heigth,
                - distance * Mathf.Cos(Time.time * speed));
    }
}
