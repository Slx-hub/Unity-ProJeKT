using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMaker : MonoBehaviour
{
    private List<Vector3> path = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Reset()
    {
        path.Clear();
        path.Add(transform.position);

        foreach (Transform t in transform)
            path.Add(t.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        for(int i = 0; i < path.Count; i++)
        {
            Gizmos.DrawLine(path[i], path[(i+1)%path.Count]);
        }
    }
}
