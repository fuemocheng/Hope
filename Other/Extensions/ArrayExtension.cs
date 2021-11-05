using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtension
{
    public static bool Contains<T>(this T[] array, T value)
    {
        foreach (var e in array)
        {
            if (value.Equals(e))
            {
                return true;
            }
        }

        return false;
    }

    public static int IndexOf<T>(this T[] array, T value)
    {
        for (int i=0; i<array.Length; i++)
        {
            if (value.Equals(array[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static T Find<T>(this T[] array, Func<T, bool> func)
    {
        for (int i = 0; i < array.Length; i++)
        {
            var val = array[i];
            if(val!=null)
            {
                if (func(val))
                    return val;
            }
        }

        return default;
    }

    public static bool Find<T>(this T[] array, Func<T, bool> func, out T ret)
    {
        for (int i = 0; i < array.Length; i++)
        {
            var val = array[i];
            if(val!=null)
            {
                if (func(val))
                {
                    ret = val;
                    return true;
                }
            }
        }
        ret = default;

        return false;
    }

    public static void Foreach<T>(this T[] array, Action<T> action)
    {
        for (int i = 0; i < array.Length; i++)
        {
            action(array[i]);
        }
    }
}
