using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventControler))]
public class AbilityControler : MonoBehaviour
{
    public Ability ab1;


    public EventControler ec;
    public Canvas UICanvas;

    private Ability currentActiveAbility;
    private ShowNearestEntity sne;

    internal void NextRoll(int currentFaceValue)
    {
        currentActiveAbility = GameObject.Instantiate(ab1, transform.position, Quaternion.identity, transform);
        currentActiveAbility.Use(this, currentFaceValue, sne.currentTarget.transform, UICanvas);
    }

    internal void UseAbility1()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        ec= GetComponent<EventControler>();
        sne = GetComponent<ShowNearestEntity>();
        if(sne == null)
            enabled= false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
