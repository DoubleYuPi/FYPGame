using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOverride : MonoBehaviour
{
    [SerializeField] private GameObject character = null;
    [SerializeField] private AnimationType[] animationTypes = null;

    private Dictionary<AnimationClip, AnimationType> dictionaryByAnimation;
    private Dictionary<string, AnimationType> dictionaryByComposite;

    private void Start()
    {
        dictionaryByAnimation = new Dictionary<AnimationClip, AnimationType>();

        foreach(AnimationType item in animationTypes)
        {
            dictionaryByAnimation.Add(item.animationClip, item);
        }

        dictionaryByComposite = new Dictionary<string, AnimationType>();

        foreach(AnimationType item in animationTypes)
        {
            string key = item.playerState.ToString() + item.partVariantType.ToString() + item.animationName.ToString();
            dictionaryByComposite.Add(key, item);
        }
    }

    public void ApplyCharaCustomParam(List<CharacterAttribute> characterAttributesList)
    {
        foreach(CharacterAttribute characterAttribute in characterAttributesList)
        {
            Animator currentAnimator = null;
            List<KeyValuePair<AnimationClip, AnimationClip>> animKeyValuePairList = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            string animatorAssetName = characterAttribute.playerState.ToString();

            Animator[] animatorsArray = character.GetComponentsInChildren<Animator>();

            foreach(Animator animator in animatorsArray)
            {
                if(animator.name == animatorAssetName)
                {
                    currentAnimator = animator;
                    break;
                }
            }

            AnimatorOverrideController aoc = new AnimatorOverrideController(currentAnimator.runtimeAnimatorController);
            List<AnimationClip> animationsList = new List<AnimationClip>(aoc.animationClips);

            foreach(AnimationClip animationClip in animationsList)
            {
                AnimationType animationType;
                bool foundAnimation = dictionaryByAnimation.TryGetValue(animationClip, out animationType);

                if (foundAnimation)
                {
                    string key = characterAttribute.playerState.ToString() + characterAttribute.partVariantType.ToString() + animationType.animationName.ToString();

                    AnimationType swapAnimationType;
                    bool foundSwapAnimation = dictionaryByComposite.TryGetValue(key, out swapAnimationType);

                    if (foundSwapAnimation)
                    {
                        AnimationClip swapAnimationClip = swapAnimationType.animationClip;
                        animKeyValuePairList.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, swapAnimationClip));
                    }
                }
            }
            aoc.ApplyOverrides(animKeyValuePairList);
            currentAnimator.runtimeAnimatorController = aoc;
        }
    }
}
