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
    /// ����¼�����
    /// </summary>
    /// <param name="tk">�¼�ID</param>
    /// <param name="tv">�¼������ص�</param>
    /// <param name="order">ͬһ�¼�ID�����õ�˳��orderԽС������Խ��</param>
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
    /// ����¼�����
    /// </summary>
    /// <param name="tk">�¼�ID</param>
    /// <param name="tv">�¼������ص�</param>
    /// <param name="autoRemove">Ϊtrueʱ���¼�����һ�κ��Զ�ɾ������</param>
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
    /// ����¼�����
    /// </summary>
    /// <param name="obj">�����󶨵Ķ���Remove(object)����ֱ��ɾ�����а󶨵Ķ�����¼�</param>
    /// <param name="tk">�¼�ID</param>
    /// <param name="tv">�¼������ص�</param>
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
    /// ɾ��obj�󶨵������¼����μ�Add(object obj, TK tk, TV tv)
    /// </summary>
    /// <param name="obj">���¼��Ķ���</param>
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
    /// �ֵ䲻�ʺ���ö����key
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
