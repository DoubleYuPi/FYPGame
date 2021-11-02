using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropsDetails
{
    //[ItemsCodeDescAtr]
    public int seedItemCode;  // this is the item code for the corresponding seed
    public int[] growthTime; // time growth for each stage
    public GameObject[] growthPrefab;// prefab to use when instantiating growth stages
    public Sprite[] growthSprite; // growth sprite
    public Sprite harvestedSprite; // sprite used once harvested
    //[ItemsCodeDescAtr]
    public int harvestedTransformItemCode; // if the item transforms into another item when harvested this item code will be populated
    public bool hideCropBeforeHarvestedAnimation; // if the crop should be disabled before the harvested animation
    public bool disableCropCollidersBeforeHarvestedAnimation;// if colliders on crop should be disabled to avoid the harvested animation effecting any other game objects
    public bool isHarvestedAnimation; // true if harvested animation to be played on final growth stage prefab
    public bool isHarvestActionEffect = false; // flag to determine whether there is a harvest action effect
    public bool spawnCropProducedAtPlayerPosition;
    //[ItemsCodeDescAtr]
    public int[] harvestToolItemCode; // array of item codes for the tools that can harvest or 0 array elements if no tool required
    public int[] requiredHarvestActions; // number of harvest actions required for corressponding tool in harvest tool item code array
    //[ItemsCodeDescAtr]
    public int[] cropProducedItemCode; // array of item codes produced for the harvested crop
    public int[] cropProducedMinQuantity; // array of minimum quantities produced for the harvested crop
    public int[] cropProducedMaxQuantity; // if max quantity is > min quantity then a random number of crops between min and max are produced
    public int timeToRegrow; // days to regrow next crop or -1 if a single crop

    public bool CanUseToolToHarvestCrop(int toolItemCode)
    {
        if (RequiredHarvestActionsForTool(toolItemCode) == -1)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public int RequiredHarvestActionsForTool(int toolItemCode)
    {
        for (int i = 0; i < harvestToolItemCode.Length; i++)
        {
            if (harvestToolItemCode[i] == toolItemCode)
            {
                return requiredHarvestActions[i];
            }
        }
        return -1;
    }
}
