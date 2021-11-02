using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ScriptObjectGrid", menuName ="Scriptable Objects/Grid Properties")]

public class ScriptObjectGrid : ScriptableObject
{
    public Places sceneName;
    public int gridWidth;
    public int gridHeight;
    public int originX;
    public int originY;

    [SerializeField]
    public List<GridProperty> gridPropertyList;
}
