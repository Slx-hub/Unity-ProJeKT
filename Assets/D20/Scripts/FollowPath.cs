using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public PathMaker pathMaker;
    public bool followPath = false;
    public float speed;

    public int targetPositionIndex;
    // Start is called before the first frame update
    void Start()
    {
        targetPositionIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!followPath || targetPositionIndex == -1) return;

        var targetPosition = pathMaker.GetPositionByIndex(targetPositionIndex);

        if(Vector3.Distance(targetPosition, transform.position) > 0.1f)
        {
            //transform.Translate((targetPosition - transform.position).normalized * speed * Time.deltaTime);
            transform.position=Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            targetPositionIndex = (targetPositionIndex+ 1) % pathMaker.PathLength();
        }
    }

    public void Follow() { followPath = true; targetPositionIndex = 0; }
    public void StopFollow() { followPath = false; targetPositionIndex = -1; }
    public void SetTargetPositionByIndex(int index)
    {
        targetPositionIndex = index;
    }

    private void OnDrawGizmos()
    {
        if(targetPositionIndex== -1) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(pathMaker.GetPositionByIndex(targetPositionIndex), 2.5f);
    }
}
