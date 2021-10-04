using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HC_Timer 
{
    public event System.Action OnTimerEnd;

    float timerTime = 0f;
    float remainingTime = 0f;
    bool timerActive = false;
    TimerController owner;

    public float TimerTime => timerTime;
    public float RemainingTime => remainingTime;

    public void RunTimer(TimerController owner, float time, System.Action OnTimerEnd)
    {
        timerTime = time;
        remainingTime = time;
        timerActive = true;
        this.OnTimerEnd += OnTimerEnd;
        this.owner = owner;
        owner.OnUpdate += Update;
    }

    public void Update(float dTime)
    {
        if (timerActive)
        {
            remainingTime -= dTime;
            if (remainingTime <= 0f)
            {
                End();
            }
        }
    }

    public void Pause()
    {
        timerActive = false;
    }

    public void Resume()
    {
        timerActive = true;
    }

    void End()
    {
        owner.OnUpdate -= Update;
        owner = null;
        timerActive = false;
        OnTimerEnd?.Invoke();
    }
}

public class TimerController : SingletonMono<TimerController>
{
    public event System.Action<float> OnUpdate;

    public static HC_Timer RunTimer(float time, System.Action OnTimerEnd)
    {
        HC_Timer timer = new HC_Timer();
        timer.RunTimer(Instance, time, OnTimerEnd);        
        return timer;
    }

    private void Update()
    {
        OnUpdate?.Invoke(Time.deltaTime);
    }
}
