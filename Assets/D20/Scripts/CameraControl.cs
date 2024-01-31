using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform Target;
    public float Lerp = 10;
    public float MouseSensitivity = 1;
    public float UpperAngularLimit = 80;
    public float LowerAngularLimit = -30;
    public float VerticalOffset = 1.5f;
    public float HorizontalOffset = 4;

    private Vector2 CurrentRotation;
    private Transform cameraTransform;

    void Awake()
    {
        cameraTransform = transform.GetChild(0);

        EventBroker<OnNetworkCreateEvent>.SubscribeChannel(SwitchTarget);
    }

    public void SwitchTarget(OnNetworkCreateEvent onEvent)
    {
        if ( onEvent.Root.IsOwner)
        {
            Target = onEvent.Root.transform;
        }
    }

    void Update()
    {
        UpdatePosition();
        UpdateMouse();
        CollideWithScene();
    }

    private void UpdatePosition()
    {
        var targetPos = Target.position + Vector3.up * VerticalOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, Lerp * Time.deltaTime);
    }

    private void UpdateMouse()
    {
        CurrentRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
        CurrentRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        CurrentRotation.x = Mathf.Repeat(CurrentRotation.x, 360);
        CurrentRotation.y = Mathf.Clamp(CurrentRotation.y, LowerAngularLimit, UpperAngularLimit);
        transform.rotation = Quaternion.Euler(CurrentRotation.y, CurrentRotation.x, 0);
    }

    private void CollideWithScene()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.forward, out hit, HorizontalOffset))
        {
            cameraTransform.localPosition = new Vector3(0, 0, -hit.distance);
        }
        else
        {
            cameraTransform.localPosition = new Vector3(0, 0, -HorizontalOffset);
        }
    }
}
