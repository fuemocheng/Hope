using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AnimBlendBehaviour : PlayableBehaviour
{
    public AnimationClip clip1;
    public AnimationClip clip2;
    PlayableGraph m_playableGraph;
    AnimationClipPlayable clipPlayable1;
    AnimationClipPlayable clipPlayable2;
    Playable inPlayable;

    public override void OnPlayableCreate (Playable playable)
    {
        base.OnPlayableCreate(playable);
        m_playableGraph = playable.GetGraph();

        inPlayable = playable;
    }

    public void Init()
    {
        clipPlayable1 = AnimationClipPlayable.Create(m_playableGraph, clip1);
        inPlayable.ConnectInput(0, clipPlayable1, 0);

        clipPlayable2 = AnimationClipPlayable.Create(m_playableGraph, clip2);
        //inPlayable.ConnectInput(1, clipPlayable2, 0);

        //inPlayable.SetInputWeight(0, 0);
        //inPlayable.SetInputWeight(1, 1);
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);

        //playable.SetInputWeight(0, 0);
        //playable.SetInputWeight(1, 1);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        base.OnBehaviourPause(playable, info);

       
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        base.PrepareFrame(playable, info);

        //if (playable.GetTime() > playable.GetLeadTime() +  playable.GetDuration() - 2.0f)
        //{
        //    playable.DisconnectInput(0);
        //    playable.ConnectInput(0, clipPlayable2, 0);
        //}
    }
}
