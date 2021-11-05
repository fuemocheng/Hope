using System;
using System.Text;

public static class FunctionExtension
{
    private static StringBuilder _stacktrace = new StringBuilder();

    public static void InvokeSafely(this Action act)
    {
        try
        {
            act?.Invoke();
        }
        catch(Exception ex)
        {
            HandleException(ex, act);
        }
    }

    public static void InvokeSafely<T>(this Action<T> act, T obj1)
    {
        try
        {
            act?.Invoke(obj1);
        }
        catch (Exception ex)
        {
            HandleException(ex, act, obj1);
        }
    }

    public static void InvokeSafely<T1, T2>(this Action<T1, T2> act, T1 obj1, T2 obj2)
    {
        try
        {
            act?.Invoke(obj1, obj2);
        }
        catch (Exception ex)
        {
            HandleException(ex, act, obj1, obj2);
        }
    }

    public static void InvokeSafely<T1, T2, T3>(this Action<T1, T2, T3> act, T1 obj1, T2 obj2, T3 obj3)
    {
        try
        {
            act?.Invoke(obj1, obj2, obj3);
        }
        catch (Exception ex)
        {
            HandleException(ex, act, obj1, obj2, obj3);
        }
    }

    public static void InvokeSafely<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> act, T1 obj1, T2 obj2, T3 obj3, T4 obj4)
    {
        try
        {
            act?.Invoke(obj1, obj2, obj3, obj4);
        }
        catch (Exception ex)
        {
            HandleException(ex, act, obj1, obj2, obj3, obj4);
        }
    }

    public static void InvokeSafely<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> act, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5)
    {
        try
        {
            act?.Invoke(obj1, obj2, obj3, obj4, obj5);
        }
        catch (Exception ex)
        {
            HandleException(ex, act, obj1, obj2, obj3, obj4, obj5);
        }
    }

    public static TResult InvokeSafely<TResult>(this Func<TResult> func)
    {
        try
        {
            return func == null ? default : func.Invoke();
        }
        catch (Exception ex)
        {
            HandleException(ex, func);
            return default;
        }
    }

    public static TResult InvokeSafely<T1, TResult>(this Func<T1, TResult> func, T1 obj1)
    {
        try
        {
            return func == null ? default : func.Invoke(obj1);
        }
        catch (Exception ex)
        {
            HandleException(ex, func, obj1);
            return default;
        }
    }

    public static TResult InvokeSafely<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 obj1, T2 obj2)
    {
        try
        {
            return func == null ? default : func.Invoke(obj1, obj2);
        }
        catch (Exception ex)
        {
            HandleException(ex, func, obj1, obj2);
            return default;
        }
    }

    public static TResult InvokeSafely<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 obj1, T2 obj2, T3 obj3)
    {
        try
        {
            return func == null ? default : func.Invoke(obj1, obj2, obj3);
        }
        catch (Exception ex)
        {
            HandleException(ex, func, obj1, obj2, obj3);
            return default;
        }
    }

    public static TResult InvokeSafely<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T1 obj1, T2 obj2, T3 obj3, T4 obj4)
    {
        try
        {
            return func == null ? default : func.Invoke(obj1, obj2, obj3, obj4);
        }
        catch (Exception ex)
        {
            HandleException(ex, func, obj1, obj2, obj3, obj4);
            return default;
        }
    }

    public static TResult InvokeSafely<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5)
    {
        try
        {
            return func == null ? default : func.Invoke(obj1, obj2, obj3, obj4, obj5);
        }
        catch (Exception ex)
        {
            HandleException(ex, func, obj1, obj2, obj3, obj4, obj5);
            return default;
        }
    }

    private static void HandleException(Exception ex, Delegate del, params object[] args)
    {
        _stacktrace.Clear();
        string argInfo = args != null && args.Length != 0 ? string.Join(", ", args) : string.Empty;
        _stacktrace.AppendLine($"Invoke function failed. ({del?.Target}:{del?.Method} ({argInfo}))");
        _stacktrace.AppendLine("******* stack trace *******");
        _stacktrace.AppendLine(ex.ToString());
        LogUtils.LogError(_stacktrace.ToString());
    }
}
