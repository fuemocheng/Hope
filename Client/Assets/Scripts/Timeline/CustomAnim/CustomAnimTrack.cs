using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.3066038f, 1f, 0.3150301f)]
[TrackClipType(typeof(CustomAnimClip))]
[TrackBindingType(typeof(Animator))]
public class CustomAnimTrack : AnimationTrack
{
    //public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    //{
    //    return ScriptPlayable<CustomAnimMixerBehaviour>.Create(graph, inputCount);
    //}
}
