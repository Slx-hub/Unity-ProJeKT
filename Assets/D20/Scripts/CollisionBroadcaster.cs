using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionBroadcaster : MonoBehaviour
{
    public Action<Collider, bool> onTrigger;

    void OnTriggerEnter(Collider other)
    {
        onTrigger?.Invoke(other, true);
    }

    void OnTriggerExit(Collider other)
    {
        onTrigger?.Invoke(other, false);
    }
}
