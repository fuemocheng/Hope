using UnityEngine;

public static class AnimatorExt
{
    public static float GetClipLength(this Animator animator, string clip)
    {
        if (null == animator || string.IsNullOrEmpty(clip) || null == animator.runtimeAnimatorController)
            return 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        AnimationClip[] tAnimationClips = ac.animationClips;
        if (null == tAnimationClips || tAnimationClips.Length <= 0) return 0;
        AnimationClip tAnimationClip;
        for (int tCounter = 0, tLen = tAnimationClips.Length; tCounter < tLen; tCounter++)
        {
            tAnimationClip = ac.animationClips[tCounter];
            if (null != tAnimationClip && tAnimationClip.name == clip)
                return tAnimationClip.length;
        }
        return 0F;
    }

    public static float GetClipLengthByOverrideController(this Animator animator, string key)
    {
        if (null == animator || string.IsNullOrEmpty(key) || null == animator.runtimeAnimatorController)
            return 0;
        AnimatorOverrideController ac = animator.runtimeAnimatorController as AnimatorOverrideController;
        AnimationClip tAnimationClip = ac[key];
        if (null != tAnimationClip)
            return tAnimationClip.length;
        return 0F;
    }
}