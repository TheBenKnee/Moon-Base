using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DepotStrength
{
    Full = 0,
    Half = 1,
    Weak = 2
}

public enum DepotStatus
{
    Unexplored,
    Opened,
    Malfunctioned
}

public class DepotTile : InteractableTile
{
    public Sprite fullDepotSprite;
    public Sprite halfDepotSprite;
    public Sprite weakDepotSprite;

    private DepotStatus depotStatus = DepotStatus.Unexplored;
    private DepotStrength depotStrength;
    private int depotSupplies;
    private int depotCapacity;

    public override void TileAction(Rover interactingRover)
    {
        switch(depotStatus)
        {
            case DepotStatus.Unexplored:
            {
                OpenDepot();
                if(depotStatus == DepotStatus.Opened)
                {
                    moonParent.TriggerDepotOpenedUI(this);
                }
                if(depotStatus == DepotStatus.Malfunctioned)
                {
                    moonParent.TriggerDepotMalfunctionedUI(this);
                }
                break;
            }
            case DepotStatus.Opened:
            {
                // Query how much to take
                moonParent.TriggerDepotSupplyAccessUI(this);
                break;
            }
            case DepotStatus.Malfunctioned:
            {
                float malfunctionedDuration = DetermineFixDuration();
                interactingRover.AddTask(new RoverTask(malfunctionedDuration, this, 1));
                break;
            }
        }
    }

    public override void FinishedTask(RoverTask finishedTask)
    {
        base.FinishedTask(finishedTask);
        depotStatus = DepotStatus.Opened;
        acceptableRoverTypes.Clear();
        acceptableRoverTypes.Add(RoverType.Supply);
    }

    // Method which returns the duration of the fix IN SECONDS
    public float DetermineFixDuration()
    {
        return 3f;
    }

    public int StoreDepotSupplies(int requestedStorage)
    {
        if(depotCapacity - depotSupplies < requestedStorage)
        {
            int tempSupplies = depotCapacity - depotSupplies;
            depotSupplies = depotCapacity;
            return tempSupplies;
        }

        depotSupplies += requestedStorage;
        return requestedStorage;
    }

    public int TakeDepotSupplies(int requestedSupplies)
    {
        if(depotSupplies <= requestedSupplies)
        {
            int tempSupplies = depotSupplies;
            depotSupplies = 0;
            return tempSupplies;
        }

        depotSupplies -= requestedSupplies;
        return requestedSupplies;
    }

    public void OpenDepot()
    {
        depotSupplies = DetermineDepotSupplies(depotStrength);
        switch(depotStrength)
        {
            case DepotStrength.Full:
            {
                depotCapacity = Constants.FULL_DEPOT_CAPACITY;
                break;
            }
            case DepotStrength.Half:
            {
                depotCapacity = Constants.HALF_DEPOT_CAPACITY;
                break;
            }
            case DepotStrength.Weak:
            {
                depotCapacity = Constants.WEAK_DEPOT_CAPACITY;
                break;
            }
        }

        int seed = Random.Range(0, 100);
        if(seed < 30)
        {
            depotStatus = DepotStatus.Malfunctioned;
            acceptableRoverTypes.Clear();
            acceptableRoverTypes.Add(RoverType.Repair);
        }
        else
        {
            depotStatus = DepotStatus.Opened;
        }
    }

    public int DetermineDepotSupplies(DepotStrength strength)
    {
        int seed = Random.Range(0, 100);
        if(seed < 40)
        {
            return 0;
        }

        switch(strength)
        {
            case DepotStrength.Full:
            {
                return Constants.FULL_DEPOT_CAPACITY;
            }
            case DepotStrength.Half:
            {
                return Constants.HALF_DEPOT_CAPACITY;
            }
            case DepotStrength.Weak:
            {
                return Constants.WEAK_DEPOT_CAPACITY;
            }
            default:
            {
                return 0;
            }
        }
    }

    // GET Methods
    public int GetDepotSupplies()
    {
        return depotSupplies;
    }

    public int GetDepotCapacity()
    {
        return depotCapacity;
    }

    public int GetDepotRemainingStorage()
    {
        return depotCapacity - depotSupplies;
    }

    public DepotStatus GetDepotStatus()
    {
        return depotStatus;
    }

    public DepotStrength GetDepotStrength()
    {
        return depotStrength;
    }

    // 'Overriding' MoonTile method for actions at tile creation
    public virtual void InitializeTile(DepotStrength initializedStrength, int xLocation, int yLocation, Moon parentMoon)
    {
        base.InitializeTile(xLocation, yLocation, parentMoon);
        depotStrength = initializedStrength;
        switch(initializedStrength)
        {
            case DepotStrength.Full:
            {
                GetComponent<SpriteRenderer>().sprite = fullDepotSprite;
                break;
            }
            case DepotStrength.Half:
            {
                GetComponent<SpriteRenderer>().sprite = halfDepotSprite;
                break;
            }
            case DepotStrength.Weak:
            {
                GetComponent<SpriteRenderer>().sprite = weakDepotSprite;
                break;
            }
        }
        acceptableRoverTypes.Add(RoverType.Supply);
    }
}


