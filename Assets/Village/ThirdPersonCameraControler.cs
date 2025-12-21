using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ThirdPersonCameraControler : MonoBehaviour
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
        cameraTransform = transform;
        Target = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.transform;
    }

    void Update()
    {
        UpdateMouse();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (Target == null)
            return;

        var targetPos = Target.position + Vector3.up * VerticalOffset - transform.forward * HorizontalOffset;
        //transform.position = Vector3.Lerp(transform.position, targetPos, Lerp * Time.deltaTime);
        transform.position = targetPos;
    }

    private void UpdateMouse()
    {
        CurrentRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
        CurrentRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        CurrentRotation.x = Mathf.Repeat(CurrentRotation.x, 360);
        CurrentRotation.y = Mathf.Clamp(CurrentRotation.y, LowerAngularLimit, UpperAngularLimit);
        transform.rotation = Quaternion.Euler(CurrentRotation.y, CurrentRotation.x, 0);
    }
}
