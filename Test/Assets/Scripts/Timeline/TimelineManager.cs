using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class TimelineManager : Singleton<TimelineManager>
{
    private PlayableGraph m_Graph;

    public PlayableGraph GetGraph()
    {
        if (!m_Graph.IsValid())
        {
            m_Graph = PlayableGraph.Create("TimelineExternal");
        }
        return m_Graph;
    }

    public void Play()
    {
        if (!m_Graph.IsValid())
            return;
        if (m_Graph.IsPlaying())
            return;
        m_Graph.Play();
    }

    public void GetInputCount()
    {

    }

    public void SetInputCount(int inputCount)
    {

    }

    public void OnDestroy()
    {
        if (m_Graph.IsValid())
        {
            m_Graph.Destroy();
        }
    }
}
