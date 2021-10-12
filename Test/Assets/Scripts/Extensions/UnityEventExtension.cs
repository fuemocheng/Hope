using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class UnityEventExtension
{
    public static void AddListenerAutoRemove(this UnityEvent ev, UnityAction action)
    {
        UnityAction unityAction = null;
        unityAction = () =>
        {
            ev.RemoveListener(unityAction);
            action();
        };
        ev.AddListener(unityAction);
    }
}
