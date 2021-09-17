using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CustomAnimBehaviour : PlayableBehaviour
{

    public float firstClipWeight;
    AnimationMixerPlayable _mixerPlayable;
    //控制所有AnimationBlendPlayableBehaviour的AnimationMixerPlayable
    Playable _fatherMixerPlayable;
    PlayableGraph _playableGraph;
    float _firstClipLength;
    float _secondClipLength;

    public void Init(AnimationClip clip1, AnimationClip clip2, float weight)
    {
        var clip1Playable = AnimationClipPlayable.Create(_playableGraph, clip1);
        var clip2Playable = AnimationClipPlayable.Create(_playableGraph, clip2);

        _mixerPlayable.ConnectInput(0, clip1Playable, 0);
        _mixerPlayable.ConnectInput(1, clip2Playable, 0);
        firstClipWeight = Mathf.Clamp01(weight);

        _firstClipLength = clip1.length;
        _secondClipLength = clip2.length;

        clip1Playable.SetSpeed(_firstClipLength);
        clip2Playable.SetSpeed(_secondClipLength);
    }

    public override void OnPlayableCreate (Playable playable)
    {
        base.OnPlayableCreate(playable);

        _playableGraph = playable.GetGraph();
        _mixerPlayable = AnimationMixerPlayable.Create(_playableGraph, 2);
        playable.ConnectInput(0, _mixerPlayable, 0);
    }


    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);

        _fatherMixerPlayable = playable.GetOutput(0);
        if (!_fatherMixerPlayable.IsPlayableOfType<AnimationMixerPlayable>())
            Debug.LogError("Get AnimationMixerPlayable Error");

        //如果是第一个Clip，直接设置权重
        if (playable.Equals(_fatherMixerPlayable.GetInput(0)))
            SetWeight();
    }


    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        base.OnBehaviourPause(playable, info);

        //播放结束
        if (playable.GetTime() >= playable.GetDuration())
        {
            _mixerPlayable.SetInputWeight(0, 0);
            _mixerPlayable.SetInputWeight(1, 0);

            //设置下一个Clip权重
            for (int i = 0, count = _fatherMixerPlayable.GetInputCount(); i < count - 1; i++)
            {
                if (playable.Equals(_fatherMixerPlayable.GetInput(i)))
                {
                    ScriptPlayable<CustomAnimBehaviour> sp = (ScriptPlayable<CustomAnimBehaviour>)_fatherMixerPlayable.GetInput(i + 1);
                    sp.GetBehaviour().SetWeight();
                    break;
                }
            }
        }
    }

    //public override void PrepareFrame(Playable playable, FrameData info)
    //{
    //    base.PrepareFrame(playable, info);
    //    float secondClipWeight = 1.0f - firstClipWeight;
    //    _mixerPlayable.SetInputWeight(0, firstClipWeight);
    //    _mixerPlayable.SetInputWeight(1, secondClipWeight);
    //    float mixerPlayableSpeed = 1.0f / (firstClipWeight * _firstClipLength + secondClipWeight * _secondClipLength);
    //    _mixerPlayable.SetSpeed(mixerPlayableSpeed);
    //}

    public void SetWeight()
    {
        float secondClipWeight = 1.0f - firstClipWeight;
        _mixerPlayable.SetInputWeight(0, firstClipWeight);
        _mixerPlayable.SetInputWeight(1, secondClipWeight);
        float mixerPlayableSpeed = 1.0f / (firstClipWeight * _firstClipLength + secondClipWeight * _secondClipLength);
        _mixerPlayable.SetSpeed(mixerPlayableSpeed);
    }
}
