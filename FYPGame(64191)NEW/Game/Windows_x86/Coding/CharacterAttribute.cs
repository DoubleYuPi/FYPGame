using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterAttribute
{
    public PlayerState playerState;
    public PartVariantType partVariantType;

    public CharacterAttribute(PlayerState playerState, PartVariantType partVariantType)
    {
        this.playerState = playerState;
        this.partVariantType = partVariantType;
    }
}


