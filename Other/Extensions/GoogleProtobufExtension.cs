using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GoogleProtobufExtension
{
    public delegate bool RepeatedFieldCompareDelegate<in T>(T t);

    public static T Find<T>(this RepeatedField<T> repeated, RepeatedFieldCompareDelegate<T> deleg)
    {
        foreach (var e in repeated)
        {
            if (deleg(e))
                return e;
        }

        return default;
    }

    public static T[] ToArray<T>(this RepeatedField<T> repeated)
    {
        var arr = new T[repeated.Count];
        repeated.CopyTo(arr, 0);
        return arr;
    }
}
