using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private float _hudRefreshRate = 0.2f;

    private Text text;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            text.text = fps + " FPS";
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
    }
}
