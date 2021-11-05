using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : InstanceBase<TimerManager>
{
    public List<Timer> timerList = new List<Timer>(1000);
    private float lastStartupTime = 0f;

    public override void Init()
    {
        _instance = this;

        lastStartupTime = Time.realtimeSinceStartup;
    }

    public override void Clear(){
        timerList.Clear();
    }

    public override void DoUpdate()
    {        
        PluginUtilities.ProfilerBegin("TimerManager.DoUpdate");
        for (int i = 0; i < timerList.Count; i++)
        {
            var t = timerList[i];
            if (!t.IsRunning || t.DoUpdate(lastStartupTime))
            {
                if(i<timerList.Count)
                {
                    timerList.RemoveAt(i);
                    i--;
                }
            }
            else
            {
                timerList[i] = t;
            }
        }
        lastStartupTime = Time.realtimeSinceStartup;
        // wheel timer

        PluginUtilities.ProfilerEnd();
    }

    //callTime = -1代表无限循环
    public static Timer Create(Action<float> function, float delay, bool isUnscale = false, int callTime = 1)
    {
        var timer = Timer.NewInstance();
        timer.ApplyId();
        timer.function = function;
        timer.delay = delay;
        timer.callTime = callTime;
        timer.isUnscale = isUnscale; 

        return timer;
    }

    //callTime = -1代表无限循环
    public static Timer Start(Action<float> function, float delay, int callTime = 1)
    {
        var timer = Timer.NewInstance();
        timer.ApplyId();
        timer.Start(function, delay, callTime);

        var mar = Instance;
        for (int i = mar.timerList.Count - 1; i >= 0; i--)
        {
            var t = mar.timerList[i];
            if (t.guid == timer.guid)
            {
                mar.timerList.RemoveAt(i);
            }
        }

        mar.timerList.Add(timer);

        return timer;
    }

    //callTime = -1代表无限循环
    public static Timer StartUnScale(Action<float> function, float delay, int callTime = 1)
    {
        var timer = Timer.NewUnScaleInstance();
        timer.ApplyId();
        timer.Start(function, delay, callTime);

        var mar = Instance;
        for (int i = mar.timerList.Count - 1; i >= 0; i--)
        {
            var t = mar.timerList[i];
            if (t.guid == timer.guid)
            {
                mar.timerList.RemoveAt(i);
            }
        }

        mar.timerList.Add(timer);

        return timer;
    }

    public static void Stop(Timer timer, bool invokeFunction = false)
    {
        if (timer.guid <= 0)
        {
            LogUtils.LogError("并非TimerManager.Create创建的Timer，无法Stop");
            return;
        }
        var mar = Instance;
        for (int i = 0; i < mar.timerList.Count; i++)
        {
            var t = mar.timerList[i];
            if (t.guid == timer.guid)
            {
                t.Stop(invokeFunction);
                mar.timerList.RemoveAt(i);
                i--;
            }
        }
    }
}