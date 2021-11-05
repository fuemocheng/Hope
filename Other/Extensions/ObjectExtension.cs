using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectExtension
{
    //TODO 需要释放
    private static Dictionary<object, Dictionary<int, ElapseTimeSt>> _objectTimeDic = new Dictionary<object, Dictionary<int, ElapseTimeSt>>();
    //经过timeout时间返回true，否则返回false。
    //index：当前object计时器索引；请不要在不同地方调用同一个索引
    //loop：循环次数，-1为无限循环
    public static bool ElapseTime(this object obj, float timeout, int index = 0, int loop = 1)
    {
        var ret = ElapseTimeFloat(obj, timeout, index, loop);
        return ret <= 0;
    }

    //loop = -1代表无限循环
    public static float ElapseTimeFloat(this object obj, float timeout, int index = 0, int loop = 1)
    {
        if (!_objectTimeDic.ContainsKey(obj))
            _objectTimeDic.Add(obj, new Dictionary<int, ElapseTimeSt>());

        if (!_objectTimeDic[obj].ContainsKey(index))
            _objectTimeDic[obj].Add(index, new ElapseTimeSt { timer = timeout - Time.deltaTime, loop = loop });
        else
            _objectTimeDic[obj][index].timer -= Time.deltaTime;

        var timeObj = _objectTimeDic[obj][index];
        if (timeObj.timer <= 0)
        {
            timeObj.timer = 0;
            timeObj.loop -= 1;

            if (loop < 0 || timeObj.loop > 0)
                timeObj.timer = timeout;

            return 0;
        }

        return _objectTimeDic[obj][index].timer;
    }

    public static bool IsNot<T>(this object obj)
    {
        return !(obj is T);
    }

    public static bool IsClass<T>(this object obj)
    {
        return obj.GetType() == typeof(T);
    }

    public static WeakReference<T> ToWeakRef<T>(this T obj) where T : class
    {
        return new WeakReference<T>(obj);
    }
}

public class ElapseTimeSt
{
    public float timer;
    public float loop;
}