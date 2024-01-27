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

    private float ZOffset;
    private Vector2 CurrentRotation;
    private Transform camera;
    private float cameraDistance;

    void Awake()
    {
        ZOffset = transform.position.y - Target.position.y;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        camera = transform.GetChild(0);
        cameraDistance = camera.localPosition.z;
    }

    void Update()
    {
        UpdatePosition();
        UpdateMouse();
        CollideWithScene();
        ShowCursorOnAlt();
    }

    private void UpdatePosition()
    {
        var targetPos = Target.position + Vector3.up * ZOffset;
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
        if (Physics.Raycast(transform.position, -transform.forward, out hit, Mathf.Abs(cameraDistance)))
        {
            camera.localPosition = new Vector3(0, 0, -hit.distance);
        }
        else
        {
            camera.localPosition = new Vector3(0, 0, cameraDistance);
        }
    }

    private void ShowCursorOnAlt()
    {
        if (!Application.isEditor)
            return;

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
