using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimationBlendBehaviour : PlayableBehaviour
{

    private PlayableGraph playableGraph;

    private int clipIndex;
    private int inputIndex;
    private int inputIndexEnd;
    private AnimationMixerPlayable animMixer;

    //private AnimationClip clip;
    //private AnimationClip endClip;

    private AnimationClipPlayable clipPlayable;
    private AnimationClipPlayable endClipPlayable;

    private bool _playing = false;
    private bool _isEndCrossed = false;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        playableGraph = TimelineManager.Instance.GetGraph();

    }

    public void SetOutPutPlayable(int index, AnimationMixerPlayable animMixer)
    {
        this.clipIndex = index;
        this.animMixer = animMixer;
    }

    public void SetAnimationClip(AnimationClip clip, AnimationClip endClip)
    {
        clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
        endClipPlayable = AnimationClipPlayable.Create(playableGraph, endClip);

        inputIndex = animMixer.AddInput(clipPlayable, 0, 0);
        inputIndexEnd = animMixer.AddInput(endClipPlayable, 0, 0);
        animMixer.SetInputWeight(inputIndex, 0);
        animMixer.SetInputWeight(inputIndexEnd, 0);
    }

    public void SetClipEndCrossed(bool isEndCrossed)
    {
        this._isEndCrossed = isEndCrossed;
    }

    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);

    }

    public override void OnGraphStop(Playable playable)
    {
        
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        _playing = true;
        _isEndCrossed = false;

        Debug.LogError("OnBehaviourPlay : " + clipIndex);
        int preIndex = inputIndex - 1;
        if(preIndex > 0)
        {
            animMixer.SetInputWeight(preIndex, 0);
        }
        animMixer.SetInputWeight(inputIndex, 1);
        animMixer.SetInputWeight(inputIndexEnd, 0);

        TimelineManager.Instance.Play();
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (!_playing)
            return;
        base.OnBehaviourPause(playable, info);
        animMixer.SetInputWeight(inputIndex, 0);
        animMixer.SetInputWeight(inputIndexEnd, 1);

        if(!_isEndCrossed)
        {
            
        }

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

        //if (endClip != null)
        //{
        //    //Playable fatherMixerPlayable = playable.GetOutput(0);

        //    //if (!fatherMixerPlayable.IsPlayableOfType<AnimationMixerPlayable>())
        //    //    Debug.LogError("fatherMixerPlayable.Is Not <AnimationMixerPlayable>");
        //    //else
        //    //    Debug.LogError("fatherMixerPlayable.Is < AnimationMixerPlayable >");


        //    //endClipPlayable = AnimationClipPlayable.Create(playableGraph, endClip);
        //    //playable.ConnectInput(0, endClipPlayable, 0);

        //    //m_fatherMixerPlayable.ConnectInput(0, endClipPlayable, 0);
        //}
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        base.PrepareFrame(playable, info);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        base.ProcessFrame(playable, info, playerData);
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
        TimelineManager.Instance.OnDestroy();
    }
}
