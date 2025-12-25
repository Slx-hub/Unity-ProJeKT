using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] respawns;

    // Start is called before the first frame update
    void Start()
    {
        EventBroker<OnNetworkCreateEvent>.SubscribeChannel(SetSpawnPoint);
        respawns = GameObject.FindGameObjectsWithTag("Respawn");
    }

    void SetSpawnPoint(OnNetworkCreateEvent onEvent)
    {
        if (onEvent.Root.IsOwner && respawns.Length > 0)
        {
            var target = respawns[Random.Range(0, respawns.Length)];
            onEvent.Root.transform.position = target.transform.position;
        }
    }
}
