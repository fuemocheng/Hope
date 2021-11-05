using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayCommander
{
    public Queue<Action> actionsQueue = new Queue<Action>();

    public void DelayCall(Action action)
    {
        lock (actionsQueue)
        {
            actionsQueue.Enqueue(action);
        }
    }

    public void Execute()
    {
        lock (actionsQueue)
        {
            if (actionsQueue.Count > 0)
                actionsQueue.Dequeue()();
        }
    }
}
