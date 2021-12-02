using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class CustomAnimClip : PlayableAsset
{
    //public CustomAnimBehaviour template = new CustomAnimBehaviour ();
    public AnimationClip clip1;
    public AnimationClip clip2;

    [Range(0, 1)]
    public float firstClipWeight;

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        //var playable = ScriptPlayable<CustomAnimBehaviour>.Create(graph, template);
        var playable = ScriptPlayable<CustomAnimBehaviour>.Create(graph, 1);
        CustomAnimBehaviour clone = playable.GetBehaviour ();
        clone.Init(clip1, clip2, firstClipWeight);
        return playable;
    }
}
