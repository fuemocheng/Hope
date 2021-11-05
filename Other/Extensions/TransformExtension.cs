using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
    public static void DestroyChildren(this Transform transform)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            child.SetParent(null);
            Giant.Resource.ResourceHelper.Destroy(child.gameObject);
        }
    }

    public static void DestroyChildrenIm(this Transform transform)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            child.SetParent(null);
            GameObject.DestroyImmediate(child.gameObject);
        }
    }

    public delegate bool ChildDelegate<T>(T t);
    public static T Find<T>(this Transform transform, ChildDelegate<T> action) where T : class
    {
        var isTrans = typeof(T) == typeof(Transform);
        var isGo = typeof(T) == typeof(GameObject);
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (isTrans)
            {
                if (action(child as T))
                {
                    return child as T;
                }
            }
            else if (isGo)
            {
                if (action(child.gameObject as T))
                {
                    return child.gameObject as T;
                }
            }
            else
            {
                var comp = child.GetComponent<T>();
                if (comp != null && action(comp))
                {
                    return comp;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 由深及浅遍历
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform FindRecursively(this Transform transform, string name)
    {
        if (transform.name == name)
            return transform;

        foreach (Transform item in transform)
        {
            var ret = item.FindRecursively(name);
            if (ret)
                return ret;
        }

        return null;
    }

    /// <summary>
    /// 获取所有的子节点
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="result"></param>
    /// <param name="maxDepth"></param>
    public static List<GameObject> getGameObjectRecursive(this Transform transform)
    {
        var result = new List<GameObject>();
        getGameObjectRecursive(transform, ref result);
        return result;
    }

    static void getGameObjectRecursive(Transform transform, ref List<GameObject> result)
    {
        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                getGameObjectRecursive(transform.GetChild(i), ref result);
        }
        else
        {
            result.Add(transform.gameObject);
        }
    }

    public static string GetTransformPath(this Transform transform)
    {
        var ret = string.Empty;
        if (transform.parent)
        {
            var canvas = transform.parent.GetComponent<Canvas>();
            if (transform.parent && !(canvas && canvas.isRootCanvas))
            {
                ret = transform.name;
                var name = transform.parent.GetTransformPath();
                if (string.IsNullOrEmpty(name))
                    return ret;
                else
                    return name + "/" + ret;
            }
        }

        return ret;
    }

    public static string GetTransformPath(this Transform transform, bool includeSelf)
    {
        if (includeSelf)
        {
            var ret = transform.name;
            if (transform.parent)
            {
                var canvas = transform.parent.GetComponent<Canvas>();
                if (transform.parent && !(canvas && canvas.isRootCanvas))
                    return transform.parent.GetTransformPath() + "/" + ret;
            }

            return ret;
        }
        else
            return transform.GetTransformPath();
    }

    //逆序防止删除子节点的问题
    public static void Foreach<T>(this Transform transform, Action<T> action) where T : class
    {
        var isTrans = typeof(T) == typeof(Transform);
        var isGo = typeof(T) == typeof(GameObject);
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            if (isTrans)
            {
                action(child as T);
                continue;
            }
            else if (isGo)
            {
                action(child.gameObject as T);
                continue;
            }

            var comp = child.GetComponent<T>();
            if (comp != null)
            {
                action(comp);
            }
        }
    }

    public static void Foreach(this Transform transform, Action<Transform> action)
    {
        Foreach<Transform>(transform, action);
    }

    public static void ForeachByRecur<T>(this Transform rootTransform, Action<T> action) where T : class
    {
        void recur(Transform recurTrans)
        {
            var isTrans = typeof(T) == typeof(Transform);
            var isGo = typeof(T) == typeof(GameObject);
            for (int i = recurTrans.childCount - 1; i >= 0; i--)
            {
                var child = recurTrans.GetChild(i);
                recur(child);

                if (isTrans)
                {
                    action(child as T);
                    continue;
                }
                else if (isGo)
                {
                    action(child.gameObject as T);
                    continue;
                }

                var comp = child.GetComponent<T>();
                if (comp != null)
                {
                    action(comp);
                }
            }
        }

        recur(rootTransform);
    }

    public static void ForeachByRecur(this Transform transform, Action<Transform> action)
    {
        ForeachByRecur<Transform>(transform, action);
    }

    public static Vector2 GetScreenPosition(this Transform rt)
    {
        Camera camera = CameraCache.main;
        return camera.WorldToScreenPoint(rt.position);
    }

    public static void SetXPos(this Transform transform, float x)
    {
        transform.position = transform.position.SetX(x);
    }

    public static void SetYPos(this Transform transform, float y)
    {
        transform.position = transform.position.SetY(y);
    }

    public static void SetZPos(this Transform transform, float z)
    {
        transform.position = transform.position.SetZ(z);
    }

    public static void SetXScale(this RectTransform transform, float x)
    {
        transform.localScale = transform.localScale.SetX(x);
    }

    public static void SetYScale(this RectTransform transform, float y)
    {
        transform.localScale = transform.localScale.SetY(y);
    }

    public static void SetZScale(this RectTransform transform, float z)
    {
        transform.localScale = transform.localScale.SetZ(z);
    }

    public static float SqrDistance(this Transform transform, Transform other)
    {
        return (transform.position - other.position).sqrMagnitude;
    }

    public static float Distance(this Transform transform, Transform other)
    {
        return (transform.position - other.position).magnitude;
    }

    //matrix with scale
    public static Matrix4x4 GetLocalToWorldMatrix(this Transform transform)
    {
        return Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
    }

    public static Matrix4x4 GetWorldToLocalMatrix(this Transform transform)
    {
        return transform.GetLocalToWorldMatrix().inverse;
    }
}
