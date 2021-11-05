using Giant.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//毋须Monobehavour的协程
//另外不用unity的coroutine为了便于管理和使用pool，以及性能易于监控
//不过主要原因还是能让其跟gameobject脱离
//TODO协程池/死循环规避（超时时间）
//TODO协程boxing优化（yield指令不使用struct）
public class CoroutineManager : InstanceBase<CoroutineManager>
{
    public List<CoroutineObj> coroutineList = new List<CoroutineObj>();
    public delegate IEnumerator CoroutineDelegate(CoroutineObj cor);

    public override void Init()
    {
    }

    public override void DoUpdate()
    {
        PluginUtilities.ProfilerBegin("ConroutineManager.DoUpdate");
        for (int i = 0; i < coroutineList.Count; i++)
        {
            var c = coroutineList[i];
            c.DoUpdate();
            if (c.IsFinish && coroutineList.IndexOf(c) != -1)
            {
                coroutineList.RemoveAt(i);
                i--;
            }
        }
        PluginUtilities.ProfilerEnd();
    }

    //开始一个非Monobehavour的协程，CoroutineDelegate内的CoroutineObj成员代表各种可用的协程yield指令
    //有boxing和alloc，慎用
    public static CoroutineObj Start(CoroutineDelegate func)
    {
        var obj = new CoroutineObj();
        obj.Enumerator = func(obj);
        Instance.coroutineList.Add(obj);

        return obj;
    }

    public static void Stop(CoroutineObj obj)
    {
        if (obj == null)
            return;

        Instance.coroutineList.Remove(obj);
    }
}

public class CoroutineObj
{
    public IEnumerator Enumerator { get; set; }
    public bool IsFinish { get; private set; }

    public CoroutineObj()
    {
        IsFinish = false;
    }

    public void DoUpdate()
    {
        var canMove = false;
        var current = Enumerator.Current;

        if (current == null)
            canMove = true;
        else if (current != null)
        {
            if (current is int)
                canMove = true;
            else if (current is Func<bool>)
            {
                IsFinish = ((Func<bool>)current)();
            }
            else if (current is CoroutineYieldObj yieldObj)
            {
                canMove = yieldObj.IsFinish();
            }
            else if (current is AsyncOperation asynOp)
            {
                canMove = asynOp.isDone;
            }
            //else if (current is SceneLoadAsync resAsynOp)
            //{
            //    canMove = resAsynOp.isDone;
            //}
            //else if (current is Giant.Res.ResourceHandle assetop) {
            //    canMove = assetop.IsDone;
            //}
        }

        if (canMove)
        {
            if (!Enumerator.MoveNext())
                IsFinish = true;
        }
    }

    //类似WaitForSeconds
    public CoroutineYieldObj Timer(float timer, bool scaleTime = true)
    {
        //todo pool
        return new CoroutineTimer(timer, scaleTime);
    }

    //类似WaitWhile
    public CoroutineYieldObj Condition(Func<bool> func)
    {
        return new CoroutineCondition(func);
    }

    //类似WaitForNextFrame
    public CoroutineYieldObj NextFrame()
    {
        return new CoroutineNextFrame();
    }
}

public abstract class CoroutineYieldObj
{
    public abstract bool IsFinish();
}

public class CoroutineTimer : CoroutineYieldObj
{
    private float duration;
    private float currentTime;
    private bool scaleTime;

    public CoroutineTimer(float pDuration, bool pScaleTime)
    {
        scaleTime = pScaleTime;
        duration = pDuration;
        currentTime = pScaleTime ? Time.time : Time.realtimeSinceStartup;
    }

    public override bool IsFinish()
    {
        var time = scaleTime ? Time.time : Time.realtimeSinceStartup;
        return time - currentTime >= duration;
    }
}

public class CoroutineCondition : CoroutineYieldObj
{
    private Func<bool> conditionFunc;

    public CoroutineCondition(Func<bool> pConditionFunc)
    {
        conditionFunc = pConditionFunc;
    }

    public override bool IsFinish()
    {
        return conditionFunc();
    }
}

public class CoroutineNextFrame : CoroutineYieldObj
{
    public override bool IsFinish()
    {
        return true;
    }
}
