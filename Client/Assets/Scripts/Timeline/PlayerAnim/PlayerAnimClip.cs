using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class PlayerAnimClip : PlayableAsset, ITimelineClipAsset
{
    public PlayerAnimBehaviour template = new PlayerAnimBehaviour ();

    public ExposedReference<AnimationClip> clip;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<PlayerAnimBehaviour>.Create (graph, template);
        PlayerAnimBehaviour clone = playable.GetBehaviour();
        clone.clip = clip.Resolve(graph.GetResolver());
        return playable;
    }
}
