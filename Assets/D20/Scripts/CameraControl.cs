using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform Target;
    public float XYDampening = 1;
    public float ZDampening = 1;
    public float MouseSensitivity = 1;
    public float UpperAngularLimit = 80;
    public float LowerAngularLimit = -30;

    private float ZOffset;
    private Vector2 CurrentRotation;

    void Awake()
    {
        ZOffset = transform.position.y - Target.position.y;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        transform.position = Target.position + Vector3.up * ZOffset;
        CurrentRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
        CurrentRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        CurrentRotation.x = Mathf.Repeat(CurrentRotation.x, 360);
        CurrentRotation.y = Mathf.Clamp(CurrentRotation.y, LowerAngularLimit, UpperAngularLimit);
        transform.rotation = Quaternion.Euler(CurrentRotation.y, CurrentRotation.x, 0);
    }
}