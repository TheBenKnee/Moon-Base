using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Direction
{
    North,
    East,
    South,
    West
}

public class Moon : MonoBehaviour
{
    [SerializeField] private List<Rover> rovers = new List<Rover>();
    [SerializeField] private List<MoonTile> moonTiles = new List<MoonTile>();
    // Number of depots on map: first number is full, second half, third weak
    [SerializeField] private List<InteractableTile> specialTiles = new List<InteractableTile>();
    [SerializeField] private List<int> totalDepots = new List<int>{ 20, 15, 10 };
    [SerializeField] private List<LaunchPadTile> launchPads = new List<LaunchPadTile>();

    [SerializeField] private List<GameObject> roverPrefabs = new List<GameObject>();
    [SerializeField] private GameObject roverParent;

    [SerializeField] private GameObject moonTilePrefab;
    [SerializeField] private GameObject moonBaseTilePrefab;
    [SerializeField] private GameObject depotTilePrefab;
    [SerializeField] private GameObject experimentTilePrefab;
    [SerializeField] private GameObject borderTilePrefab;
    [SerializeField] private GameObject launchPadTilePrefab;

    [SerializeField] private GameObject tileParent;

    [SerializeField] private MoonUI moonFrontend;
    [SerializeField] private GameLogic gameLogic;

    [SerializeField] private MoonBase moonBase;

    private void Awake() 
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        BuildMoon();
        SpawnRovers();
        gameLogic.RoverHookup(rovers);

        moonFrontend.BuildRoverMenu(rovers);
    }

    public void SpawnRovers()
    {
        foreach(GameObject roverPrefab in roverPrefabs)
        {
            GameObject newRover = Instantiate(roverPrefab);
            newRover.transform.SetParent(roverParent.transform);

            Rover newRoverComponent = newRover.GetComponent<Rover>();
            LaunchPadTile roversStartingTile = GetRoversLaunchPad(newRoverComponent);
            newRoverComponent.InitializeRover(this, roversStartingTile);

            rovers.Add(newRoverComponent);
        }
    }

    public LaunchPadTile GetRoversLaunchPad(Rover rover)
    {
        foreach(LaunchPadTile pad in launchPads)
        {
            if(pad.InteractWithTile(rover))
            {
                return pad;
            }
        }

        Debug.LogWarning("Warning: Could not find launch pad for rover.");
        return null;
    }

    // Will build out the moon's tiles
    public void BuildMoon()
    {
        BuildBorderTiles();
        BuildLaunchPads();
        BuildMoonBase();
        BuildExperimentLocations();
        BuildDepots();
        BuildBasicMoonTiles();
    }

    public void BuildBorderTiles()
    {
        for(int i = 1; i < 41; i++)
        {
            GameObject bottomBorderTile = Instantiate(borderTilePrefab);
            bottomBorderTile.transform.SetParent(tileParent.transform);
            bottomBorderTile.transform.position = new Vector3(i + 0.5f, -1.0f + 0.5f, 0);

            bottomBorderTile.GetComponent<BorderTile>().InitializeTile(i - 1);

            GameObject topBorderTile = Instantiate(borderTilePrefab);
            topBorderTile.transform.SetParent(tileParent.transform);
            topBorderTile.transform.position = new Vector3(i + 0.5f, 41.0f + 0.5f, 0);

            topBorderTile.GetComponent<BorderTile>().InitializeTile(i - 1);
        }
        for(int i = 1; i < 41; i++)
        {
            GameObject leftBorderTile = Instantiate(borderTilePrefab);
            leftBorderTile.transform.SetParent(tileParent.transform);
            leftBorderTile.transform.position = new Vector3(0.5f, i + 0.5f, 0);

            leftBorderTile.GetComponent<BorderTile>().InitializeTile(40 + i - 1);

            GameObject rightBorderTile = Instantiate(borderTilePrefab);
            rightBorderTile.transform.SetParent(tileParent.transform);
            rightBorderTile.transform.position = new Vector3(41.0f + 0.5f, i + 0.5f, 0);

            rightBorderTile.GetComponent<BorderTile>().InitializeTile(40 + i - 1);
        }
    }

    public void BuildLaunchPads()
    {
        GameObject firstLaunchPad = Instantiate(launchPadTilePrefab);
        firstLaunchPad.transform.SetParent(tileParent.transform);
        firstLaunchPad.transform.position = new Vector3(1 + 0.5f, 0.5f, 0);

        LaunchPadTile firstLaunchPadComponent = firstLaunchPad.GetComponent<LaunchPadTile>();
        firstLaunchPadComponent.InitializeTile(1, 0, this);
        firstLaunchPadComponent.SetStartingRover(RoverType.Science);

        GameObject secondLaunchPad = Instantiate(launchPadTilePrefab);
        secondLaunchPad.transform.SetParent(tileParent.transform);
        secondLaunchPad.transform.position = new Vector3(10 + 0.5f, 0.5f, 0);

        LaunchPadTile secondLaunchPadComponent = secondLaunchPad.GetComponent<LaunchPadTile>();
        secondLaunchPadComponent.InitializeTile(10, 0, this);
        secondLaunchPadComponent.SetStartingRover(RoverType.Supply);

        GameObject thirdLaunchPad = Instantiate(launchPadTilePrefab);
        thirdLaunchPad.transform.SetParent(tileParent.transform);
        thirdLaunchPad.transform.position = new Vector3(30 + 0.5f, 0.5f, 0);

        LaunchPadTile thirdLaunchPadComponent = thirdLaunchPad.GetComponent<LaunchPadTile>();
        thirdLaunchPadComponent.InitializeTile(30, 0, this);
        thirdLaunchPadComponent.SetStartingRover(RoverType.Repair);

        GameObject fourthLaunchPad = Instantiate(launchPadTilePrefab);
        fourthLaunchPad.transform.SetParent(tileParent.transform);
        fourthLaunchPad.transform.position = new Vector3(40 + 0.5f, 0.5f, 0);

        LaunchPadTile fourthLaunchPadComponent = fourthLaunchPad.GetComponent<LaunchPadTile>();
        fourthLaunchPadComponent.InitializeTile(40, 0, this);
        fourthLaunchPadComponent.SetStartingRover(RoverType.Lab);

        specialTiles.Add(firstLaunchPadComponent);
        moonTiles.Add(firstLaunchPadComponent);
        launchPads.Add(firstLaunchPadComponent);

        specialTiles.Add(secondLaunchPadComponent);
        moonTiles.Add(secondLaunchPadComponent);
        launchPads.Add(secondLaunchPadComponent);

        specialTiles.Add(thirdLaunchPadComponent);
        moonTiles.Add(thirdLaunchPadComponent);
        launchPads.Add(thirdLaunchPadComponent);

        specialTiles.Add(fourthLaunchPadComponent);
        moonTiles.Add(fourthLaunchPadComponent);
        launchPads.Add(fourthLaunchPadComponent);
    }

    // Will place moon base in set location
    public void BuildMoonBase()
    {
        GameObject firstBaseTile = Instantiate(moonBaseTilePrefab);
        firstBaseTile.transform.SetParent(tileParent.transform);
        firstBaseTile.transform.position = new Vector3(33 + 0.5f, 40 + 0.5f, 0);

        MoonBaseTile firstMoonBaseComponent = firstBaseTile.GetComponent<MoonBaseTile>();
        firstMoonBaseComponent.InitializeTile(moonBase, 1, 33, 40, this);

        GameObject secondBaseTile = Instantiate(moonBaseTilePrefab);
        secondBaseTile.transform.SetParent(tileParent.transform);
        secondBaseTile.transform.position = new Vector3(34 + 0.5f, 40 + 0.5f, 0);

        MoonBaseTile secondMoonBaseComponent = secondBaseTile.GetComponent<MoonBaseTile>();
        secondMoonBaseComponent.InitializeTile(moonBase, 2, 34, 40, this);

        GameObject thirdBaseTile = Instantiate(moonBaseTilePrefab);
        thirdBaseTile.transform.SetParent(tileParent.transform);
        thirdBaseTile.transform.position = new Vector3(35 + 0.5f, 40 + 0.5f, 0);

        MoonBaseTile thirdMoonBaseComponent = thirdBaseTile.GetComponent<MoonBaseTile>();
        thirdMoonBaseComponent.InitializeTile(moonBase, 3, 35, 40, this);

        specialTiles.Add(firstMoonBaseComponent);
        moonTiles.Add(firstMoonBaseComponent);

        specialTiles.Add(secondMoonBaseComponent);
        moonTiles.Add(secondMoonBaseComponent);

        specialTiles.Add(thirdMoonBaseComponent);
        moonTiles.Add(thirdMoonBaseComponent);
    }

    // Will scatter experiment locations around remaining tiles
    public void BuildExperimentLocations()
    {
        List<(int, int)> experimentLocations = DetermineExperimentLocations(10);

        int currentExperimentNumber = 1;

        foreach((int, int) experimentLocation in experimentLocations)
        {
            GameObject newExperimentTile = Instantiate(experimentTilePrefab);
            newExperimentTile.transform.SetParent(tileParent.transform);
            newExperimentTile.transform.position = new Vector3(experimentLocation.Item1 + 0.5f, experimentLocation.Item2 + 0.5f, 0);

            bool important = false;
            // Determine if experiment is key or not
            if(currentExperimentNumber % 2 == 1)
            {
                important = true;
            }

            ExperimentTile experimentTileComponent = newExperimentTile.GetComponent<ExperimentTile>();
            experimentTileComponent.InitializeTile(currentExperimentNumber, important, experimentLocation.Item1, experimentLocation.Item2, this);

            specialTiles.Add(experimentTileComponent);
            moonTiles.Add(experimentTileComponent);

            currentExperimentNumber++;
        } 
    }

    // Will scatter depots around remaining open tiles
    public void BuildDepots()
    {
        List<List<(int, int)>> depotLocations = DetermineDepotLocations();

        DepotStrength depotType = (DepotStrength)0; // 0 - Full, 1 - Half, 2 - Weak
        foreach(List<(int, int)> depotTypeGroup in depotLocations)
        {
            foreach((int, int) depotLocation in depotTypeGroup)
            {
                GameObject newDepotTile = Instantiate(depotTilePrefab);
                newDepotTile.transform.SetParent(tileParent.transform);
                newDepotTile.transform.position = new Vector3(depotLocation.Item1 + 0.5f, depotLocation.Item2 + 0.5f, 0);
                DepotTile depotTileComponent = newDepotTile.GetComponent<DepotTile>();
                depotTileComponent.InitializeTile(depotType, depotLocation.Item1, depotLocation.Item2, this);
                specialTiles.Add(depotTileComponent);
                moonTiles.Add(depotTileComponent);
            }
            depotType++;
        } 
    }

    // Will fill out all the remaining tiles with generic moon tiles.
    public void BuildBasicMoonTiles()
    {
        for(int i = 1; i <= Constants.MOON_X_SIZE; i++)
        {
            for(int j = 1; j <= Constants.MOON_Y_SIZE; j++)
            {
                if(specialTiles.FirstOrDefault(tile => tile.x == i && tile.y == j) == null)
                {
                    GameObject newMoonTile = Instantiate(moonTilePrefab);
                    newMoonTile.transform.SetParent(tileParent.transform);
                    newMoonTile.transform.position = new Vector3(i + 0.5f, j + 0.5f, 0);
                    MoonTile moonTileComponent = newMoonTile.GetComponent<MoonTile>();
                    moonTileComponent.InitializeTile(i, j, this);
                    moonTiles.Add(moonTileComponent);
                }
            }
        }
    }

    // Will generate the specific x,y locations for the number of experiments passed in by the user.
    // Will check if a previous interactable tile occupies that tile, and will regenerate a location until a free one is found.
    //
    // TODO: Change this generation from completely random to either noise or with further stipulations,
    // i.e., checking that there are no other interactables within a range, etc. 
    public List<(int, int)> DetermineExperimentLocations(int amountOfExperimentTiles)
    {
        List<(int, int)> returnList = new List<(int, int)>();

        for(int i = 0; i < amountOfExperimentTiles; i++)
        {
            (int, int) location;
            do
            {
                location = (Random.Range(1, Constants.MOON_X_SIZE), Random.Range(1, Constants.MOON_Y_SIZE));
            }
            while(returnList.Contains(location) && specialTiles.FirstOrDefault(tile => tile.x == location.Item1 && tile.y == location.Item2) != null);

            returnList.Add(location);
        }

        return returnList;
    }

    // Will generate the specific x,y locations for the number/type of depots described in the totalDepots list.
    // Will check if a previous interactable tile occupies that tile or if a depot is queued to be placed there,
    // and will regenerate a location until a free one is found.
    //
    // TODO: Change this generation from completely random to either noise or with further stipulations,
    // i.e., checking that there are no other depots within a range, no other interactables within a range, etc. 
    public List<List<(int, int)>> DetermineDepotLocations()
    {
        List<(int, int)> infoList = new List<(int, int)>();
        List<List<(int, int)>> returnList = new List<List<(int, int)>>();

        foreach(int typeDepot in totalDepots)
        {
            List<(int, int)> depotTypeList = new List<(int, int)>();
            for(int i = 0; i < typeDepot; i++)
            {
                (int, int) location;
                do
                {
                    location = (Random.Range(1, Constants.MOON_X_SIZE), Random.Range(1, Constants.MOON_Y_SIZE));
                }
                while(infoList.Contains(location) && specialTiles.FirstOrDefault(tile => tile.x == location.Item1 && tile.y == location.Item2) != null);

                infoList.Add(location);
                depotTypeList.Add(location);
            }

            returnList.Add(depotTypeList);
        }

        return returnList;
    }

    // Method will get the depot tile from a specific x, y position from the list of interactable tiles.
    // If there is no interactable tile at that location or it is not specifically a DepotTile, it will return null.
    public DepotTile GetDepotTile(int x, int y)
    {
        InteractableTile depotTile = specialTiles.FirstOrDefault(tile => tile.x == x && tile.y == y);
        if(depotTile == null || depotTile.GetType() != typeof(DepotTile))
        {
            return null;
        }

        return (DepotTile)depotTile;
    }

    public MoonTile GetMoonTileXY(int x, int y)
    {
        return moonTiles.FirstOrDefault(tile => tile.x == x && tile.y == y);
    }

    public MoonTile GetTileAdjacent(MoonTile currentTile, Direction direction)
    {
        switch(direction)
        {
            case Direction.North:
            {
                if(currentTile.y + 1 > Constants.MOON_Y_SIZE)
                {
                    return currentTile;
                }
                return GetMoonTileXY(currentTile.x, currentTile.y + 1);
            }
            case Direction.East:
            {
                if(currentTile.x + 1 > Constants.MOON_X_SIZE)
                {
                    return currentTile;
                }
                return GetMoonTileXY(currentTile.x + 1, currentTile.y);
            }
            case Direction.South:
            {
                if(currentTile.y - 1 < 1)
                {
                    return currentTile;
                }
                return GetMoonTileXY(currentTile.x, currentTile.y - 1);
            }
            case Direction.West:
            {
                if(currentTile.y - 1 < 1)
                {
                    return currentTile;
                }
                return GetMoonTileXY(currentTile.x - 1, currentTile.y);
            }
            default:
            {
                return currentTile;
            }
        }
    }

    ////////
    // UI METHODS
    ////////

    public void RemoveInteractableQueryAndBackup()
    {
        moonFrontend.SuppressInteractableAndBackupUI();
    }

    public void TriggerDepotOpenedUI(DepotTile tile)
    {
        moonFrontend.TriggerDepotOpenedInfo(tile);
    }

    public void TriggerDepotMalfunctionedUI(DepotTile tile)
    {
        moonFrontend.TriggerDepotMalfunctionedUI(tile);
    }

    public void TriggerDepotSupplyAccessUI(DepotTile tile)
    {
        moonFrontend.TriggerDepotSuppliesInfo(tile);
    }
}
