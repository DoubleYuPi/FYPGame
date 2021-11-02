using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animation Type", menuName = "Scriptable Objects/Animation/Animation Type")]
public class AnimationType : ScriptableObject
{
    public AnimationClip animationClip;
    public AnimationName animationName;
    public PartVariantType partVariantType;
    public PlayerState playerState;
}
