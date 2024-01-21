using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisingFloor : MonoBehaviour
{
    public float yS;
    public float yE;
    public float yV;

    public bool raised = false;

    public bool autoMove = false;
    public float interval = 5f;

    private bool changeState = false;
    private float innerTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (!autoMove) return;

        if(innerTimer > interval)
        {
            innerTimer = 0f;
            ChangeState();
        }

        innerTimer += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!changeState) return;

        if (raised)
        {
            transform.Translate(-transform.up * yV * Time.deltaTime);
            if (transform.position.y < yS)
            {
                raised = false;
                changeState = false;
                transform.position = new Vector3(transform.position.x, yS, transform.position.z);
            }
        }
        else
        {
            transform.Translate(transform.up * yV * Time.deltaTime);
            if (transform.position.y > yE)
            {
                raised = true;
                changeState = false;
                transform.position = new Vector3(transform.position.x, yE, transform.position.z);
            }
        }
    }

    public void Raise()
    {
        if (raised) return;

        changeState = true;
    }

    public void Lower()
    {
        if (!raised) return;

        changeState = true;
    }

    public void ChangeState()
    {
        if (changeState) return;

        changeState = true;
    }

    public bool IsRaised() { return raised; }
    public bool IsChaningState() { return changeState; }
    public void EnableAutoMove() { autoMove = true; }
    public void DisableAutoMove() { autoMove = false; }
}
