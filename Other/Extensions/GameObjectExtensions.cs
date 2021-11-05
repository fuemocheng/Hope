using System;
using UnityEngine;

public static class GameObjectExtensions
{
    public static void SetLayer(this GameObject gameObject, LayerMask layer)
    {
        SetLayer(gameObject, layer, false);
    }

    public static void SetLayer(this GameObject gameObject, LayerMask layer, bool recursive)
    {
        gameObject.layer = layer;

        if (recursive)
            foreach (Transform item in gameObject.transform)
                item.gameObject.SetLayer(layer, true);
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var comp = gameObject.GetComponent<T>();
        if (comp == null)
            comp = gameObject.AddComponent<T>();

        return comp;
    }
    public static Component GetOrAddComponent(this GameObject gameObject,Type t)
    {
        var comp = gameObject.GetComponent(t);
        if (comp == null)
            comp = gameObject.AddComponent(t);

        return comp;
    }
    public static GameObject FindRecursively(this GameObject go, string name)
    {
        if (go != null)
        {
            var retgo = go.transform.FindRecursively(name);
            return retgo != null ? retgo.gameObject : null;
        }
        return null;
    }
}
