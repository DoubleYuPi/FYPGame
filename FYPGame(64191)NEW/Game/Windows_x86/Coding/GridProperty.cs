using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GridProperty
{
    public GridCoordinate gridCoordinate;
    public GridBool gridBool;
    public bool gridBoolValue = false;

    public GridProperty(GridCoordinate gridCoordinate,GridBool gridBool, bool gridBoolValue)
    {
        this.gridCoordinate = gridCoordinate;
        this.gridBool = gridBool;
        this.gridBoolValue = gridBoolValue;
    }
}
