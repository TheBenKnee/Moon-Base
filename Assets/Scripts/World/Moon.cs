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
    public List<MoonTile> moonTiles = new List<MoonTile>();
    // Number of depots on map: first number is full, second half, third weak
    public List<InteractableTile> specialTiles = new List<InteractableTile>();
    public List<int> totalDepots = new List<int>{ 20, 15, 10 };

    public GameObject moonTilePrefab;
    public GameObject depotTilePrefab;

    public MoonUI moonFrontend;

    private void Awake() 
    {
        BuildMoon();
    }

    // Will build out the moon's tiles
    public void BuildMoon()
    {
        BuildMoonBase();
        BuildScienceLocations();
        BuildDepots();
        BuildBasicMoonTiles();
    }

    // Will place moon base in set location
    public void BuildMoonBase()
    {
        
    }

    // Will scatter science locations around remaining tiles
    public void BuildScienceLocations()
    {
        
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
                newDepotTile.transform.SetParent(transform);
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
                    newMoonTile.transform.SetParent(transform);
                    newMoonTile.transform.position = new Vector3(i + 0.5f, j + 0.5f, 0);
                    MoonTile moonTileComponent = newMoonTile.GetComponent<MoonTile>();
                    moonTileComponent.InitializeTile(i, j, this);
                    moonTiles.Add(moonTileComponent);
                }
            }
        }
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

    public void TriggerInteractableQuery(InteractableTile tile, Rover interactingRover)
    {
        moonFrontend.InformAndTriggerInteractableUI(tile, interactingRover);
    }

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
