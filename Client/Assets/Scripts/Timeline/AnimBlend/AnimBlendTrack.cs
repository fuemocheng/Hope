using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.4481132f, 0.7240566f, 1f)]
[TrackClipType(typeof(AnimBlendClip))]
[TrackBindingType(typeof(Animator))]
public class AnimBlendTrack : AnimationTrack
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<AnimBlendMixerBehaviour>.Create (graph, inputCount);
    }
}
