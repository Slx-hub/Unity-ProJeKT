using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class EventBroker<T>
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

public record OnNetworkCreateEvent(NetworkBehaviour Root);

public record OnHitValueEvent(string HitValue, string Color)
{
    public OnHitValueEvent(string HitValue) : this(HitValue, null) { }
}



namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}