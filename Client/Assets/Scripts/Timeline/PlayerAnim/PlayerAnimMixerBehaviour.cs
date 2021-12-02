using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerAnimMixerBehaviour : PlayableBehaviour
{


    Animator m_TrackBinding;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_TrackBinding = playerData as Animator;

        if (m_TrackBinding == null)
            return;


        int inputCount = playable.GetInputCount ();

        float totalWeight = 0f;


        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<PlayerAnimBehaviour> inputPlayable = (ScriptPlayable<PlayerAnimBehaviour>)playable.GetInput(i);
            PlayerAnimBehaviour input = inputPlayable.GetBehaviour ();
            
            totalWeight += inputWeight;



        }
    }
}
