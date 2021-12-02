using System;

/// <summary>
/// Mono生命周期事件
/// 一些不继承Mono的类如果想在Mono生命周期做一些事，可以往这里添加
/// </summary>
public class MonoEvent : MonoSingleton<MonoEvent>
{
    public event Action UPDATE;
    public event Action LATEUPDATE;
    public event Action FIXUPDATE;
    public event Action ONGUI;

    void Update()
    {
        UPDATE?.Invoke();
    }

    void LateUpdate()
    {
        LATEUPDATE?.Invoke();
    }
    
    void FixUpdate()
    {
        FIXUPDATE?.Invoke();
    }

    void OnGUI()
    {
        ONGUI?.Invoke();
    }

}
