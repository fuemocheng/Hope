using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Timer
{
    public static int guidPool = 1;

    public int guid;
    public Action<float> function;
    public float delay;
    public float callTime;
    public float curDelay;
    public float curCallTime;
    public object key;
    public bool isUnscale;

    public bool IsRunning { get; private set; }
    public bool IsValid { get => delay > 0; }

    public float Normalize { get => curDelay / delay; }

    public static Timer NewInstance()
    {
        return new Timer
        {
            function = null,
            delay = 0,
            callTime = 0,
            curDelay = 0,
            curCallTime = 0,
            key = null,
            isUnscale = false,
            IsRunning = false,
        };
    }

    public static Timer NewUnScaleInstance()
    {
        return new Timer
        {
            function = null,
            delay = 0,
            callTime = 0,
            curDelay = 0,
            curCallTime = 0,
            key = null,
            isUnscale = true,
            IsRunning = false,
        };
    }

    /// <summary>
    /// 设置id
    /// </summary>
    public void ApplyId()
    {
        guid = guidPool++;
    }

    //delay单位为秒
    public void Start(Action<float> function, float delay, int callTime = 1)
    {
        this.function = function;
        this.delay = delay;
        this.callTime = callTime;
        curDelay = 0;
        curCallTime = 0;
        IsRunning = true;
    }

    //delay单位为秒
    public void Start(float delay, int callTime = 1)
    {
        this.delay = delay;
        this.callTime = callTime;
        curDelay = 0;
        curCallTime = 0;
        IsRunning = true;
    }

    //timer完毕返回true
    public bool DoUpdate(float lastStartupTime)
    {
        if (!IsRunning)
            return true;

        curDelay += isUnscale ? Time.realtimeSinceStartup - lastStartupTime : Time.deltaTime;

        if (curDelay >= delay)
        {
            try
            {
                function?.Invoke(curDelay);
            }
            catch (System.NullReferenceException exception)
            {
                LogUtils.LogError("空间名：" + exception.Source + "；" + '\n' +
                  "方法名：" + exception.TargetSite + '\n' +
                  "故障点：" + exception.StackTrace + '\n' +
                  "错误提示：" + exception.Message);
            }
            curCallTime++;
            if (callTime >= 0 && curCallTime >= callTime)
            {
                Stop(false);
                return true;
            }
            else
            {
                curDelay = 0;
            }
        }

        return false;
    }

    public void Restart()
    {
        if (IsRunning)
            Stop();

        Reset();
        IsRunning = true;
    }

    /// <summary>
    /// 如果由TimerManager创建的Timer，调用此Stop无效，请调用TimerManager.Stop
    /// </summary>
    /// <param name="invokeFunction"></param>
    public void Stop(bool invokeFunction = false)
    {
        Reset();

        IsRunning = false;

        if (invokeFunction)
        {
            try
            {
                function?.Invoke(curDelay);
            }
            catch (System.NullReferenceException exception)
            {
                LogUtils.LogError("空间名：" + exception.Source + "；" + '\n' +
                  "方法名：" + exception.TargetSite + '\n' +
                  "故障点：" + exception.StackTrace + '\n' +
                  "错误提示：" + exception.Message);
            }
        }

        function = null;
    }

    public void Reset()
    {
        curDelay = 0;
        curCallTime = 0;
    }
}