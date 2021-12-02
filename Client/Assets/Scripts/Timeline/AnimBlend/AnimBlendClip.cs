using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AnimBlendClip : PlayableAsset, ITimelineClipAsset
{
    //public AnimBlendBehaviour template = new AnimBlendBehaviour ();
    public AnimationClip clip1;
    public AnimationClip clip2;
    public ClipCaps clipCaps
    {
        get { return ClipCaps.All; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AnimBlendBehaviour>.Create (graph, 2);
        AnimBlendBehaviour clone = playable.GetBehaviour ();
        clone.clip1 = clip1;
        clone.clip2 = clip2;
        clone.Init();
        return playable;
    }
}
