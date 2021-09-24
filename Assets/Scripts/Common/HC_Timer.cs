using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HC_Timer
{
    public event System.Action OnTimerEnd;

    float timerTime = 0f;
    float remainingTime = 0f;
    bool timerActive = false;

    public float TimerTime => timerTime;
    public float RemainingTime => remainingTime;

    public void RunTimer(float time)
    {
        timerTime = time;
        remainingTime = time;
    }

    

}
