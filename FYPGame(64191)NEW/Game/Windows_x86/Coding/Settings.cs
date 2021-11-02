using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static int InventoryCapacity = 24;
    public static int MaxInventoryCapacity = 48;

    public const float gridCellSize = 1f;
    public static Vector2 cursorSize = Vector2.one;

    //Tools Desc
    public const string HoeingTool = "Hoe";
    public const string WateringTool = "Watering Can";
    public const string PickupTool = "Bag";
    public const string SlayingTool = "Sword";
    public const string ChopingTool = "Scythe";

    public static float useToolAnimationPause = 0.35f;
    public static float afterUseToolAnimationPause = 0.3f;
}
