using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GridPropertiesDetails
{
    public int gridX;
    public int gridY;
    public bool hoeable = false;
    public bool dropItem = false;
    public int timeDug = -1;
    public int timeWatered = -1;
    public int seedItemCode = -1;
    public int growthTime = -1;
    public int HarvestedTime = -1;

    public GridPropertiesDetails()
    {

    }
}
