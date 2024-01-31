using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EventBroker<T> where T : EventType
{
    private static List<Action<T>> listeners = new();

    public static void SubscribeChannel(Action<T> listener)
    {
        listeners.Add(listener);
    }

    public static void UnsubscribeChannel(Action<T> listener)
    {
        listeners.Remove(listener);
    }

    public static void PublishEvent(T raisedEvent)
    {
        listeners.ForEach(l => l(raisedEvent));
    }
}

public class EventChannel<T> where T : EventType
{

}

public interface EventType
{

}

public class OnNetworkCreateEvent : EventType
{
    public NetworkBehaviour cause;
    public bool isOwner;

    public OnNetworkCreateEvent(NetworkBehaviour cause, bool isOwner)
    {
        this.cause = cause;
        this.isOwner = isOwner;
    }
}
