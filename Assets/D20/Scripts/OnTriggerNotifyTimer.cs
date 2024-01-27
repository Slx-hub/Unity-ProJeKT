using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerNotifyTimer : NotifyTimer
{
    private void OnTriggerEnter(Collider other)
    {
        NotifyFinished();
    }
}
