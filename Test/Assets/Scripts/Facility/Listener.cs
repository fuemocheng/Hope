using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listener<TK, TV> where TV : class
{
    private Dictionary<TK, List<TV>> listeners = new Dictionary<TK, List<TV>>();
    private Dictionary<object, List<KeyValuePair<TK, TV>>> objectMapper = new Dictionary<object, List<KeyValuePair<TK, TV>>>();
    private HashSet<TV> autoRemoveHashSet = new HashSet<TV>();
    private List<KeyValuePair<TK, TV>> autoRemoveList = new List<KeyValuePair<TK, TV>>();

    public Listener()
    {

    }

    /// <summary>
    /// 添加事件侦听
    /// </summary>
    /// <param name="tk">事件ID</param>
    /// <param name="tv">事件触发回调</param>
    /// <param name="order">同一事件ID，调用的顺序，order越小，调用越早</param>
    public void Add(TK tk, TV tv, int order)
    {
        if (listeners.ContainsKey(tk))
        {
            listeners[tk].Insert(order, tv);
        }
        else
        {
            var list = new List<TV> { tv };
            listeners.Add(tk, list);
        }
    }

    /// <summary>
    /// 添加事件侦听
    /// </summary>
    /// <param name="tk">事件ID</param>
    /// <param name="tv">事件触发回调</param>
    /// <param name="autoRemove">为true时，事件触发一次后自动删除侦听</param>
    public void Add(TK tk, TV tv, bool autoRemove = false)
    {
        if (listeners.ContainsKey(tk))
        {
            listeners[tk].Add(tv);
        }
        else
        {
            var list = new List<TV> { tv };
            listeners.Add(tk, list);
        }

        if (autoRemove)
        {
            autoRemoveHashSet.Add(tv);
        }
    }

    /// <summary>
    /// 添加事件侦听
    /// </summary>
    /// <param name="obj">侦听绑定的对象，Remove(object)可以直接删除所有绑定的对象的事件</param>
    /// <param name="tk">事件ID</param>
    /// <param name="tv">事件触发回调</param>
    public void Add(object obj, TK tk, TV tv)
    {
        Add(tk, tv);
        if (objectMapper.ContainsKey(obj))
        {
            objectMapper[obj].Add(new KeyValuePair<TK, TV>(tk, tv));
        }
        else
        {
            var list = new List<KeyValuePair<TK, TV>> { new KeyValuePair<TK, TV>(tk, tv) };
            objectMapper.Add(obj, list);
        }
    }

    public void Remove(TK tk, TV tv)
    {
        if (listeners.ContainsKey(tk))
        {
            listeners[tk].Remove(tv);
        }
    }

    public void Remove(TK tk)
    {
        if (listeners.ContainsKey(tk))
        {
            listeners[tk].Clear();
        }
    }

    /// <summary>
    /// 删除obj绑定的所有事件，参见Add(object obj, TK tk, TV tv)
    /// </summary>
    /// <param name="obj">绑定事件的对象</param>
    public void Remove(object obj)
    {
        if (!objectMapper.ContainsKey(obj)) return;

        var list = objectMapper[obj];
        foreach (var e in list)
        {
            var listenerList = listeners[e.Key];
            for (int i = listenerList.Count - 1; i >= 0; i--)
            {
                if (e.Value == listenerList[i])
                {
                    listenerList.RemoveAt(i);
                }
            }
        }
        objectMapper.Remove(obj);
    }

    /// <summary>
    /// 字典不适合用枚举做key
    /// </summary>
    /// <param name="tk"></param>
    /// <param name="dispatchAction"></param>
    /// <returns></returns>
    public bool Dispatch(TK tk, Action<TV> dispatchAction)
    {
        bool hasDiapatch = false;
        if (listeners.TryGetValue(tk, out List<TV> list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                TV tv = list[i];
                dispatchAction?.Invoke(tv);

                hasDiapatch = true;

                if (autoRemoveHashSet.Contains(tv))
                {
                    autoRemoveList.Add(new KeyValuePair<TK, TV>(tk, list[i]));
                }
            }
        }

        foreach (var kvp in autoRemoveList)
        {
            Remove(kvp.Key, kvp.Value);
        }
        autoRemoveList.Clear();

        return hasDiapatch;
    }

    public void Clear()
    {
        listeners.Clear();
        objectMapper.Clear();
        autoRemoveHashSet.Clear();
        autoRemoveList.Clear();
    }
}
