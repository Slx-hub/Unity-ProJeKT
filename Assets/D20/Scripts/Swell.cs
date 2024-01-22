using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swell : MonoBehaviour
{
    public Vector3 maxExtends;
    public Vector3 minExtends;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = minExtends;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.localScale, maxExtends) > 0.1f)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, maxExtends, speed * Time.deltaTime);
        }else
        {
            transform.localScale = minExtends;
            gameObject.SetActive(false);
        }
    }
}
