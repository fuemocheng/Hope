using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[System.Serializable]
public class AnimationBlendClip : PlayableAsset
{
    private int clipIndex;
    private AnimationMixerPlayable animMixer;

    public AnimationClip clip;
    public AnimationClip endClip;

    private bool isEndCrossed = false;

    public void SetOutPutPlayable(int index, AnimationMixerPlayable animMixer)
    {
        clipIndex = index;
        this.animMixer = animMixer;
    }

    public void SetClipEndCrossed(bool isEndCrossed)
    {
        this.isEndCrossed = isEndCrossed;
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<AnimationBlendBehaviour>.Create(graph, 1);
        AnimationBlendBehaviour clone = playable.GetBehaviour();
        clone.SetOutPutPlayable(clipIndex, animMixer);
        clone.SetAnimationClip(clip, endClip);
        clone.SetClipEndCrossed(isEndCrossed);
        return playable;
    }

}
