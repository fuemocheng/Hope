using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class AnimationBlendClip : PlayableAsset
{
    public AnimationClip clip;
    public AnimationClip endClip;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<AnimationBlendBehaviour>.Create(graph, 1);
        AnimationBlendBehaviour clone = playable.GetBehaviour();
        clone.clip = clip;
        clone.endClip = endClip;
        clone.Init();
        return playable;
    }
}
