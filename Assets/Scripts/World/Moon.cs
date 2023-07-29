using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Moon : MonoBehaviour
{
    public List<MoonTile> moonTiles = new List<MoonTile>();
    // Number of depots on map: first number is full, second half, third weak
    public List<InteractableTile> specialTiles = new List<InteractableTile>();
    public List<int> totalDepots = new List<int>{ 20, 15, 10 };

    public GameObject moonTilePrefab;
    public GameObject depotTilePrefab;

    private void Awake() 
    {
        BuildMoon();
    }

    public void BuildMoonBase()
    {
        
    }

    public void BuildScienceLocations()
    {
        
    }

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
                depotTileComponent.InitializeTile(depotType, depotLocation.Item1, depotLocation.Item2);
                specialTiles.Add(depotTileComponent);
                moonTiles.Add(depotTileComponent);
            }
            depotType++;
        } 
    }

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
                    moonTileComponent.InitializeTile(i, j);
                    moonTiles.Add(moonTileComponent);
                }
            }
        }
    }

    public void BuildMoon()
    {
        BuildMoonBase();
        BuildScienceLocations();
        BuildDepots();
        BuildBasicMoonTiles();
    }

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
}
