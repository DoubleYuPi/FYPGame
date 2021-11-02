using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SceneItem
{
    public int itemCode;
    public Vector3Serialize position;
    public string itemName;

    public SceneItem()
    {
        position = new Vector3Serialize();
    }
}
