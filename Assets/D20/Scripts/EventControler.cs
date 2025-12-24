using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventControler : MonoBehaviour
{
    public List<(float, Action)> eventList = new();

    public float innerTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        var comboController = GetComponent<ComboController>();

    }

    // Update is called once per frame
    void Update()
    {
        if(eventList.Count > 0) 
        {
            var happeningEvents = eventList.Where(e => e.Item1 < innerTimer).ToList();

            if (happeningEvents.Any(x => x.Item2 != null))
            {
                happeningEvents.Where(x => x.Item2 != null).ToList().ForEach(e => e.Item2());
                eventList.RemoveAll(e => happeningEvents.Contains(e));
            }
        }

        innerTimer += Time.deltaTime;
    }

    public void AddEvent(float time, Action action, bool relative = false)
    {
        eventList.Add((relative ? innerTimer + time : time, action));
    }
}
