using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(GenerateGUID))]

public class GridPropertiesManager : SingletonMonoBehaviour<GridPropertiesManager>, ISaveable
{
    private Transform cropParentTransform;
    private Tilemap groundDecor1;
    private Tilemap groundDecor2;
    private Grid grid;
    private Dictionary<string, GridPropertiesDetails> gridPropertiesDict;
    [SerializeField] private SO_CropDetailsList cropDetailsList = null;
    [SerializeField] private ScriptObjectGrid[] scriptObjectGridsArr = null;
    [SerializeField] private Tile[] dugGround = null;
    [SerializeField] private Tile[] wateredGround = null;

    private string iSaveableUniqueID;
    public string ISaveableUniqueID
    {
        get
        {
            return iSaveableUniqueID;
        }
        set
        {
            iSaveableUniqueID = value;
        }
    }

    public GameObjectSave gameObjectSave;
    public GameObjectSave GameObjectSave
    {
        get
        {
            return gameObjectSave;
        }
        set
        {
            gameObjectSave = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    private void AfterSceneLoaded()
    {
        if (GameObject.FindGameObjectWithTag(Tags.CropsParentTransform) != null)
        {
            cropParentTransform = GameObject.FindGameObjectWithTag(Tags.CropsParentTransform).transform;
        }
        else
        {
            cropParentTransform = null;
        }

        grid = GameObject.FindObjectOfType<Grid>();

        groundDecor1 = GameObject.FindGameObjectWithTag(Tags.GroundDecoration1).GetComponent<Tilemap>();
        groundDecor2 = GameObject.FindGameObjectWithTag(Tags.GroundDecoration2).GetComponent<Tilemap>();
    }

    private void OnEnable()
    {
        ISaveableRegister();

        EventHandle.AfterSceneLoadEvent += AfterSceneLoaded;
        EventHandle.AdvanceGameDayEvent += AdvanceDay;
    }

    private void OnDisable()
    {
        ISaveableDeregister();

        EventHandle.AfterSceneLoadEvent -= AfterSceneLoaded;
        EventHandle.AdvanceGameDayEvent -= AdvanceDay;
    }

    private void Start()
    {
        StartGridProperties();
    }

    private void ClearDisplayDecorGround()
    {
        groundDecor1.ClearAllTiles();
        groundDecor2.ClearAllTiles();
    }

    private void ClearDisplayAllPlantedCrop()
    {
        Crops[] cropsArray;
        cropsArray = FindObjectsOfType<Crops>();

        foreach(Crops crops in cropsArray)
        {
            Destroy(crops.gameObject);
        }
    }

    private void ClearDisplayGridPropertyDetails()
    {
        ClearDisplayDecorGround();

        ClearDisplayAllPlantedCrop();
    }

    public void DisplayDugGround(GridPropertiesDetails gridPropertiesDetails)
    {
        if(gridPropertiesDetails.timeDug > -1)
        {
            ConnectDugGround(gridPropertiesDetails);
        }
    }

    public void DisplayWaterGround(GridPropertiesDetails gridPropertiesDetails)
    {
        if (gridPropertiesDetails.timeWatered > -1)
        {
            ConnectWateredGround(gridPropertiesDetails);
        }
    }

    private void ConnectDugGround(GridPropertiesDetails gridPropertyDetails)
    {
        // Select tile based on surrounding dug tiles

        Tile dugTile0 = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY);
        groundDecor1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY, 0), dugTile0);

        // Set 4 tiles if dug surrounding current tile - up, down, left, right now that this central tile has been dug

        GridPropertiesDetails adjacentGridPropertyDetails;

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.timeDug > -1)
        {
            Tile dugTile1 = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
            groundDecor1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1, 0), dugTile1);
        }

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.timeDug > -1)
        {
            Tile dugTile2 = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
            groundDecor1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1, 0), dugTile2);
        }
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.timeDug > -1)
        {
            Tile dugTile3 = SetDugTile(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
            groundDecor1.SetTile(new Vector3Int(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY, 0), dugTile3);
        }

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.timeDug > -1)
        {
            Tile dugTile4 = SetDugTile(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
            groundDecor1.SetTile(new Vector3Int(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY, 0), dugTile4);
        }
    }

    private void ConnectWateredGround(GridPropertiesDetails gridPropertyDetails)
    {
        // Select tile based on surrounding watered tiles

        Tile wateredTile0 = SetWateredTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY);
        groundDecor2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY, 0), wateredTile0);

        // Set 4 tiles if watered surrounding current tile - up, down, left, right now that this central tile has been watered

        GridPropertiesDetails adjacentGridPropertyDetails;

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.timeWatered > -1)
        {
            Tile wateredTile1 = SetWateredTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
            groundDecor2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1, 0), wateredTile1);
        }

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.timeWatered > -1)
        {
            Tile wateredTile2 = SetWateredTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
            groundDecor2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1, 0), wateredTile2);
        }

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.timeWatered > -1)
        {
            Tile wateredTile3 = SetWateredTile(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
            groundDecor2.SetTile(new Vector3Int(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY, 0), wateredTile3);
        }

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.timeWatered > -1)
        {
            Tile wateredTile4 = SetWateredTile(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
            groundDecor2.SetTile(new Vector3Int(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY, 0), wateredTile4);
        }
    }

    private Tile SetDugTile(int xGrid, int yGrid)
    {
        //Get whether surrounding tiles (up,down,left, and right) are dug or not

        bool upDug = IsGridSquareDug(xGrid, yGrid + 1);
        bool downDug = IsGridSquareDug(xGrid, yGrid - 1);
        bool leftDug = IsGridSquareDug(xGrid - 1, yGrid);
        bool rightDug = IsGridSquareDug(xGrid + 1, yGrid);

        #region Set appropriate tile based on whether surrounding tiles are dug or not

        if (!upDug && !downDug && !rightDug && !leftDug)
        {
            return dugGround[0];
        }
        else if (!upDug && downDug && rightDug && !leftDug)
        {
            return dugGround[1];
        }
        else if (!upDug && downDug && rightDug && leftDug)
        {
            return dugGround[2];
        }
        else if (!upDug && downDug && !rightDug && leftDug)
        {
            return dugGround[3];
        }
        else if (!upDug && downDug && !rightDug && !leftDug)
        {
            return dugGround[4];
        }
        else if (upDug && downDug && rightDug && !leftDug)
        {
            return dugGround[5];
        }
        else if (upDug && downDug && rightDug && leftDug)
        {
            return dugGround[6];
        }
        else if (upDug && downDug && !rightDug && leftDug)
        {
            return dugGround[7];
        }
        else if (upDug && downDug && !rightDug && !leftDug)
        {
            return dugGround[8];
        }
        else if (upDug && !downDug && rightDug && !leftDug)
        {
            return dugGround[9];
        }
        else if (upDug && !downDug && rightDug && leftDug)
        {
            return dugGround[10];
        }
        else if (upDug && !downDug && !rightDug && leftDug)
        {
            return dugGround[11];
        }
        else if (upDug && !downDug && !rightDug && !leftDug)
        {
            return dugGround[12];
        }
        else if (!upDug && !downDug && rightDug && !leftDug)
        {
            return dugGround[13];
        }
        else if (!upDug && !downDug && rightDug && leftDug)
        {
            return dugGround[14];
        }
        else if (!upDug && !downDug && !rightDug && leftDug)
        {
            return dugGround[15];
        }

        return null;

        #endregion Set appropriate tile based on whether surrounding tiles are dug or not
    }

    private bool IsGridSquareDug(int gridX, int gridY)
    {
        GridPropertiesDetails gridPropertiesDetails = GetGridPropertyDetails(gridX, gridY);

        if(gridPropertiesDetails == null)
        {
            return false;
        }
        else if (gridPropertiesDetails.timeDug > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Tile SetWateredTile(int xGrid, int yGrid)
    {
        // Get whether surrounding tiles (up,down,left, and right) are watered or not

        bool upWatered = IsGridSquareWatered(xGrid, yGrid + 1);
        bool downWatered = IsGridSquareWatered(xGrid, yGrid - 1);
        bool leftWatered = IsGridSquareWatered(xGrid - 1, yGrid);
        bool rightWatered = IsGridSquareWatered(xGrid + 1, yGrid);

        #region Set appropriate tile based on whether surrounding tiles are watered or not

        if (!upWatered && !downWatered && !rightWatered && !leftWatered)
        {
            return wateredGround[0];
        }
        else if (!upWatered && downWatered && rightWatered && !leftWatered)
        {
            return wateredGround[1];
        }
        else if (!upWatered && downWatered && rightWatered && leftWatered)
        {
            return wateredGround[2];
        }
        else if (!upWatered && downWatered && !rightWatered && leftWatered)
        {
            return wateredGround[3];
        }
        else if (!upWatered && downWatered && !rightWatered && !leftWatered)
        {
            return wateredGround[4];
        }
        else if (upWatered && downWatered && rightWatered && !leftWatered)
        {
            return wateredGround[5];
        }
        else if (upWatered && downWatered && rightWatered && leftWatered)
        {
            return wateredGround[6];
        }
        else if (upWatered && downWatered && !rightWatered && leftWatered)
        {
            return wateredGround[7];
        }
        else if (upWatered && downWatered && !rightWatered && !leftWatered)
        {
            return wateredGround[8];
        }
        else if (upWatered && !downWatered && rightWatered && !leftWatered)
        {
            return wateredGround[9];
        }
        else if (upWatered && !downWatered && rightWatered && leftWatered)
        {
            return wateredGround[10];
        }
        else if (upWatered && !downWatered && !rightWatered && leftWatered)
        {
            return wateredGround[11];
        }
        else if (upWatered && !downWatered && !rightWatered && !leftWatered)
        {
            return wateredGround[12];
        }
        else if (!upWatered && !downWatered && rightWatered && !leftWatered)
        {
            return wateredGround[13];
        }
        else if (!upWatered && !downWatered && rightWatered && leftWatered)
        {
            return wateredGround[14];
        }
        else if (!upWatered && !downWatered && !rightWatered && leftWatered)
        {
            return wateredGround[15];
        }

        return null;

        #endregion Set appropriate tile based on whether surrounding tiles are watered or not
    }

    private bool IsGridSquareWatered(int xGrid, int yGrid)
    {
        GridPropertiesDetails gridPropertyDetails = GetGridPropertyDetails(xGrid, yGrid);

        if (gridPropertyDetails == null)
        {
            return false;
        }
        else if (gridPropertyDetails.timeWatered > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DisplayGridPropetyDetails()
    {
        foreach (KeyValuePair<string, GridPropertiesDetails> item in gridPropertiesDict)
        {
            GridPropertiesDetails gridPropertiesDetails = item.Value;

            DisplayDugGround(gridPropertiesDetails);
            DisplayWaterGround(gridPropertiesDetails);
            DisplayPlantedCrop(gridPropertiesDetails);
        }
    }

    public void DisplayPlantedCrop(GridPropertiesDetails gridPropertiesDetails)
    {
        if (gridPropertiesDetails.seedItemCode > -1)
        {
            // get crop details
            CropsDetails cropDetails = cropDetailsList.GetCropDetails(gridPropertiesDetails.seedItemCode);

            if (cropDetails != null)
            {
                // prefab to use
                GameObject cropPrefab;

                // instantiate crop prefab at grid location
                int growthStages = cropDetails.growthTime.Length;

                int currentGrowthStage = 0;

                for (int i = growthStages - 1; i >= 0; i--)
                {
                    if (gridPropertiesDetails.growthTime >= cropDetails.growthTime[i])
                    {
                        currentGrowthStage = i;
                        break;
                    }

                }

                cropPrefab = cropDetails.growthPrefab[currentGrowthStage];

                Sprite growthSprite = cropDetails.growthSprite[currentGrowthStage];

                Vector3 worldPosition = groundDecor2.CellToWorld(new Vector3Int(gridPropertiesDetails.gridX, gridPropertiesDetails.gridY, 0));

                worldPosition = new Vector3(worldPosition.x + Settings.gridCellSize / 2, worldPosition.y, worldPosition.z);

                GameObject cropInstance = Instantiate(cropPrefab, worldPosition, Quaternion.identity);

                cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = growthSprite;
                cropInstance.transform.SetParent(cropParentTransform);
                cropInstance.GetComponent<Crops>().cropGridPosition = new Vector2Int(gridPropertiesDetails.gridX, gridPropertiesDetails.gridY);
            }
        }
    }

    public GridPropertiesDetails GetGridPropertyDetails(int gridX, int gridY, Dictionary<string, GridPropertiesDetails> gridPropertyDictionary)
    {
        string key = "x" + gridX + "y" + gridY;

        GridPropertiesDetails gridPropertyDetails;

        if (!gridPropertyDictionary.TryGetValue(key, out gridPropertyDetails))
        {
            return null;
        }
        else
        {
            return gridPropertyDetails;
        }
    }

    public GridPropertiesDetails GetGridPropertyDetails(int gridX, int gridY)
    {
        return GetGridPropertyDetails(gridX, gridY, gridPropertiesDict);
    }

    private void StartGridProperties()
    {
        foreach (ScriptObjectGrid scriptObjctGridProperties in scriptObjectGridsArr)
        {
            Dictionary<string, GridPropertiesDetails> gridPropertyDictionary = new Dictionary<string, GridPropertiesDetails>();

            foreach (GridProperty gridProperty in scriptObjctGridProperties.gridPropertyList)
            {
                GridPropertiesDetails gridPropertyDetails;

                gridPropertyDetails = GetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y, gridPropertyDictionary);

                if (gridPropertyDetails == null)
                {
                    gridPropertyDetails = new GridPropertiesDetails();
                }

                switch (gridProperty.gridBool)
                {
                    case GridBool.hoeable:
                        gridPropertyDetails.hoeable = gridProperty.gridBoolValue;
                        break;

                    case GridBool.dropItem:
                        gridPropertyDetails.dropItem = gridProperty.gridBoolValue;
                        break;

                    default:
                        break;
                }

                SetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y, gridPropertyDetails, gridPropertyDictionary);
            }

            // Create scene save for this gameobject
            SceneSave sceneSave = new SceneSave();

            // Add grid property dictionary to scene save data
            sceneSave.gridPropsDetails = gridPropertyDictionary;

            // If starting scene set the gridPropertyDictionary member variable to the current iteration
            if (scriptObjctGridProperties.sceneName.ToString() == SceneControlManager.Instance.startPlace.ToString())
            {
                this.gridPropertiesDict = gridPropertyDictionary;
            }

            GameObjectSave.sceneData.Add(scriptObjctGridProperties.sceneName.ToString(), sceneSave);
        }
    }

    public void SetGridPropertyDetails(int gridX, int gridY, GridPropertiesDetails gridPropertyDetails)
    {
        SetGridPropertyDetails(gridX, gridY, gridPropertyDetails, gridPropertiesDict);
    }

    public void SetGridPropertyDetails(int gridX, int gridY, GridPropertiesDetails gridPropertyDetails, Dictionary<string, GridPropertiesDetails> gridPropertyDictionary)
    {
        // Construct key from coordinate
        string key = "x" + gridX + "y" + gridY;

        gridPropertyDetails.gridX = gridX;
        gridPropertyDetails.gridY = gridY;

        // Set value
        gridPropertyDictionary[key] = gridPropertyDetails;
    }

    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    public void ISaveableStoreScene(string sceneName)
    {
        GameObjectSave.sceneData.Remove(sceneName);

        SceneSave sceneSave = new SceneSave();

        sceneSave.gridPropsDetails = gridPropertiesDict;

        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        if (GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave))
        {
            if(sceneSave.gridPropsDetails != null)
            {
                gridPropertiesDict = sceneSave.gridPropsDetails;
            }

            if(gridPropertiesDict.Count > 0)
            {
                ClearDisplayGridPropertyDetails();

                DisplayGridPropetyDetails();
            }
        }
    }
    private void AdvanceDay(int gameHour, int gameMinute, int gameSecond)
    {
        // Clear Display All Grid Property Details
        ClearDisplayGridPropertyDetails();

        // Loop through all scenes - by looping through all gridproperties in the array
        foreach (ScriptObjectGrid so_GridProperties in scriptObjectGridsArr)
        {
            // Get gridpropertydetails dictionary for scene
            if (GameObjectSave.sceneData.TryGetValue(so_GridProperties.sceneName.ToString(), out SceneSave sceneSave))
            {
                if (sceneSave.gridPropsDetails != null)
                {
                    for (int i = sceneSave.gridPropsDetails.Count - 1; i >= 0; i--)
                    {
                        KeyValuePair<string, GridPropertiesDetails> item = sceneSave.gridPropsDetails.ElementAt(i);

                        GridPropertiesDetails gridPropertyDetails = item.Value;

                        #region Update all grid properties to reflect the advance in the day

                        // If a crop is planted
                        if (gridPropertyDetails.growthTime > -1)
                        {
                            gridPropertyDetails.growthTime += 10;
                        }

                        // If ground is watered, then clear water
                        if (gridPropertyDetails.timeWatered > -1)
                        {
                            gridPropertyDetails.timeWatered = -1;
                        }

                        // Set gridpropertydetails
                        SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails, sceneSave.gridPropsDetails);

                        #endregion Update all grid properties to reflect the advance in the day
                    }
                }
            }
        }

        // Display grid property details to reflect changed values
        DisplayGridPropetyDetails();
    }
}
