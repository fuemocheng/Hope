using UnityEngine;

/// <summary>
/// 继承Mono的单例模板
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// 线程锁
    /// </summary>
    private static readonly object _lock = new object();

    private static T _instance;

    protected static bool ApplicationIsQuitting { get; private set; }

    protected static bool IsGlobal = true;

    static MonoSingleton()
    {
        ApplicationIsQuitting = false;
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                if (ApplicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton]" + typeof(T) +
                        " already destory in application quit." +
                        " Don`t create again.");

                    return null;
                }

                lock (_lock)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogWarning("[Singleton]" + typeof(T) + " should never be more than 1 in scene!");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singletonObj = new GameObject();
                        _instance = singletonObj.AddComponent<T>();
                        singletonObj.name = "(singleton)" + typeof(T);

                        if (IsGlobal && Application.isPlaying)
                        {
                            DontDestroyOnLoad(singletonObj);
                        }
                        return _instance;
                    }
                }
            }

            return _instance;
        }
    }

    /// <summary>
    /// 程序退出
    /// </summary>
    public void OnApplicationQuit()
    {
        ApplicationIsQuitting = true;
    }

}
