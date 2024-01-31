using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsEyeFollow : MonoBehaviour
{
    public Transform target;
    public Transform CameraRotation;
    public float height;
    // Start is called before the first frame update
    void Start()
    {
        EventBroker<OnNetworkCreateEvent>.SubscribeChannel(SwitchTarget);
    }

    public void SwitchTarget(OnNetworkCreateEvent onEvent)
    {
        if (onEvent.Root.IsOwner)
        {
            target = onEvent.Root.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        transform.position = target.transform.position + Vector3.up * height;
        transform.rotation = Quaternion.Euler(90, CameraRotation.rotation.eulerAngles.y, 0);
    }
}
