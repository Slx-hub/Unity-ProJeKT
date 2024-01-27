using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class NotifyTimer : MonoBehaviour
{
    private TimerController timer;
    public void AddListener(TimerController controller)
    {
        timer = controller;
    }
    protected void NotifyFinished()
    {
        timer.FinishTimer();
    }
}

public class TimerController : MonoBehaviour
{
    public bool ActivateTimer = true;
    public NotifyTimer TimerNotifier;

    private float passedTime = 0;
    private bool paused = false;
    private TextMeshProUGUI DisplayText;

    void Start()
    {
        if (!ActivateTimer)
            gameObject.SetActive(false);
        DisplayText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        TimerNotifier.AddListener(this);
    }

    void FixedUpdate()
    {
        if (!paused)
        {
            passedTime += Time.deltaTime;
            DisplayText.text = GetFormattedString(passedTime);
        }
    }

    public void FinishTimer()
    {
        paused = true;
    }

    private string GetFormattedString(float time)
    {
        int millis = (int)((time % 1) * 100);
        int seconds = (int)Mathf.Floor(time) % 60;
        int minutes = (int)time / 60;
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, millis);
    }
}
