/// <summary>
/// 用于计算两次更新间隔
/// </summary>
public struct TimeHelper
{
    private float m_LastTime;



    private float m_Interval;



    private bool m_Repeat;



    private bool m_Pause;



    public TimeHelper(float interval ,bool repeat = false)
    {
        m_LastTime = UnityEngine.Time.realtimeSinceStartup;//System.DateTime.Now.Ticks;
        m_Interval = interval;
        m_Repeat = repeat;
        m_Pause = false;
    }



    /// <summary>
    /// 自上一次调用后，间隔时间是否大于预设值
    /// </summary>
    public bool IsValid()
    {
        if (m_Pause) return false;

        float tCurrent = UnityEngine.Time.realtimeSinceStartup;//.DateTime.Now.Ticks;
        var tDeltaTime = tCurrent - this.m_LastTime;
        bool tIsAchieve = tDeltaTime >= this.m_Interval;
        if (tIsAchieve && m_Repeat)
            this.m_LastTime = tCurrent;

        return tIsAchieve;
    }



    public void Refresh(float _Interval)
    {
        m_LastTime = UnityEngine.Time.realtimeSinceStartup;//System.DateTime.Now.Ticks;
        m_Interval = _Interval ;
    }



    public void Pause(bool _Pause = true)
    {
        m_Pause = _Pause;
        if(!m_Pause)
            m_LastTime = UnityEngine.Time.realtimeSinceStartup;//System.DateTime.Now.Ticks;
    }
}
