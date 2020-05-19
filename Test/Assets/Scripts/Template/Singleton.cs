using System;

/// <summary>
/// 不继承Mono的单例，加了个线程锁，所有C#程序都能访问
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : new()
{
    /// <summary>
    /// 线程锁
    /// </summary>
    private static readonly object _lock = new object();

    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}

//public class Singleton<T>
//{
//    private static T _instance = Activator.CreateInstance<T>();

//    public static T Instance
//    {
//        get
//        {
//            return _instance;
//        }
//    }

//    private Singleton(){ }
//}
