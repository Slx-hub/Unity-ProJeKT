using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeamControl : MonoBehaviour
{
    public float speed;
    public float yOffset;

    private Vector3 m_targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, m_targetPosition) < 0.1f) { gameObject.SetActive(false); return; }

        transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, speed * Time.deltaTime);
    }

    public void Activate(Vector3 startPosition, Vector3 targetPosition)
    {
        gameObject.SetActive(true);
        transform.position = startPosition + yOffset * Vector3.up;
        m_targetPosition = targetPosition + yOffset * Vector3.up; ;
    }
}
