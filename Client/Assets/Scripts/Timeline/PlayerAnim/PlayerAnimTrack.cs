using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;

[TrackColor(0.854902f, 0.8705882f, 0.8608558f)]
[TrackClipType(typeof(PlayerAnimClip))]
[TrackBindingType(typeof(Animator))]
public class PlayerAnimTrack : AnimationTrack
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<PlayerAnimMixerBehaviour>.Create (graph, inputCount);
    }

    // Please note this assumes only one component of type Animator on the same gameobject.
    public override void GatherProperties (PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        Animator trackBinding = director.GetGenericBinding(this) as Animator;
        if (trackBinding == null)
            return;

#endif
        base.GatherProperties (director, driver);
    }
}
