using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimationBlendBehaviour : PlayableBehaviour
{
    public AnimationClip clip;
    public AnimationClip endClip;

    Playable inPlayable;

    PlayableGraph playableGraph;

    AnimationClipPlayable clipPlayable;

    AnimationClipPlayable endClipPlayable;

    AnimationMixerPlayable m_mixerPlayable;

    //控制所有AnimationBlendPlayableBehaviour的AnimationMixerPlayable
    Playable m_fatherMixerPlayable;


    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        playableGraph = playable.GetGraph();

        m_mixerPlayable = AnimationMixerPlayable.Create(playableGraph, 2);
        playable.ConnectInput(0, m_mixerPlayable, 0);

        //inPlayable = playable;
    }

    public void Init()
    {
        clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
        endClipPlayable = AnimationClipPlayable.Create(playableGraph, endClip);
       
        m_mixerPlayable.ConnectInput(0, clipPlayable, 0);
        m_mixerPlayable.ConnectInput(1, endClipPlayable, 0);
        m_mixerPlayable.SetInputWeight(0, 1);
        m_mixerPlayable.SetInputWeight(1, 0);

        //inPlayable.ConnectInput(0, clipPlayable, 0);
    }

    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);

        m_fatherMixerPlayable = playable.GetOutput(0);
    }

    public override void OnGraphStop(Playable playable)
    {
        
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        //m_mixerPlayable.SetInputWeight(0, 1);
        //m_mixerPlayable.SetInputWeight(1, 0);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        base.OnBehaviourPause(playable, info);

        //m_mixerPlayable.SetInputWeight(0, 0);
        //m_mixerPlayable.SetInputWeight(1, 1);

        if (playable.GetTime() >= playable.GetDuration())
        {
            //m_mixerPlayable.SetInputWeight(0, 0);
            //m_mixerPlayable.SetInputWeight(1, 1);

            //if (!m_fatherMixerPlayable.IsNull())
            //{
            //    //设置下一个Clip权重
            //    for (int i = 0, count = m_fatherMixerPlayable.GetInputCount(); i < count - 1; i++)
            //    {
            //        if (playable.Equals(m_fatherMixerPlayable.GetInput(i)))
            //        {
            //            ScriptPlayable<AnimationBlendBehaviour> sp = (ScriptPlayable<AnimationBlendBehaviour>)m_fatherMixerPlayable.GetInput(i + 1);
            //            sp.GetBehaviour().SetWeight();
            //            break;
            //        }
            //    }
            //}
        }

        if (endClip != null)
        {
            //Playable fatherMixerPlayable = playable.GetOutput(0);

            //if (!fatherMixerPlayable.IsPlayableOfType<AnimationMixerPlayable>())
            //    Debug.LogError("fatherMixerPlayable.Is Not <AnimationMixerPlayable>");
            //else
            //    Debug.LogError("fatherMixerPlayable.Is < AnimationMixerPlayable >");


            //endClipPlayable = AnimationClipPlayable.Create(playableGraph, endClip);
            //playable.ConnectInput(0, endClipPlayable, 0);
        }
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
}
