using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.3066038f, 1f, 0.3150301f)]
[TrackClipType(typeof(AnimationBlendClip))]
[TrackBindingType(typeof(Animator))]
public class AnimationBlendTrack : TrackAsset
{
    private AnimationPlayableOutput animOutput;
    private AnimationMixerPlayable animMixer;

    protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
    {
        PlayableGraph externalGraph = TimelineManager.Instance.GetGraph();

        if (!animOutput.IsOutputValid())
        {
            PlayableDirector playableDirector = graph.GetResolver() as PlayableDirector;
            Animator bindAnimator = playableDirector.GetGenericBinding(this) as Animator;
            animOutput = AnimationPlayableOutput.Create(externalGraph, "AnimOutPut", bindAnimator);
            animMixer = AnimationMixerPlayable.Create(externalGraph);
            animOutput.SetSourcePlayable(animMixer);
        }

        int clipIndex = 0;
        IEnumerable<TimelineClip> clips = GetClips();
        foreach (var animClip in clips)
        {
            if (animClip == clip)
            {
                var asset = clip.asset as AnimationBlendClip;
                asset.SetOutPutPlayable(clipIndex, animMixer);
                break;
            }
            clipIndex++;
        }
        TimelineClip frontClip = null;
        foreach (var animClip in clips)
        {
            if(frontClip != null)
            {
                if (IsCrossed(frontClip, animClip))
                {
                    (frontClip.asset as AnimationBlendClip)?.SetClipEndCrossed(true);
                }
            }
            frontClip = animClip;
        }

        return base.CreatePlayable(graph, gameObject, clip);
    }

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return base.CreateTrackMixer(graph, go, inputCount);
    }

    protected override void OnCreateClip(TimelineClip clip)
    {
        base.OnCreateClip(clip);
    }

//    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
//    {
//#if UNITY_EDITOR
//        Animator trackBinding = director.GetGenericBinding(this) as Animator;
//        if (trackBinding == null)
//            return;

//#endif
//        base.GatherProperties(director, driver);
//    }

    private void OnDestroy()
    {
        Debug.LogError("OnDestroy");
    }

    /// <summary>
    /// 只判断 frontClip 的尾和 backClip 的头是否相交
    /// </summary>
    private bool IsCrossed(TimelineClip frontClip, TimelineClip backClip)
    {
        if (frontClip.end > backClip.start)
            return true;
        return false;
    }

}
