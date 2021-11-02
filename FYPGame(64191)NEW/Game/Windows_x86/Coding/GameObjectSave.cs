using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSave
{
    public Dictionary<string, SceneSave> sceneData;

    public GameObjectSave()
    {
        sceneData = new Dictionary<string, SceneSave>();
    }

    public GameObjectSave (Dictionary<string, SceneSave> sceneData)
    {
        this.sceneData = sceneData;
    }
}
