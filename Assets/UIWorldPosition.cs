using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldPosition : MonoBehaviour
{
    public Vector3 worldPosition = Vector3.zero;
    public Rect canvasRect;
    public Camera cam;

    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform= GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasRect == null || cam == null)
            return;

        rectTransform.anchoredPosition = WorldToUIPosition(worldPosition, canvasRect, cam);
    }

    public Vector3 WorldToUIPosition(Vector3 wPos, Rect crt, Camera cam)
    {
        var viewortPos = cam.WorldToScreenPoint(wPos);
        var cScale = new Vector2(crt.width / Screen.width, crt.height / Screen.height);

        var screenPos = new Vector2(viewortPos.x * cScale.x, viewortPos.y * cScale.y) - new Vector2(crt.width, crt.height) * 0.5f;

        return screenPos;
    }
}
