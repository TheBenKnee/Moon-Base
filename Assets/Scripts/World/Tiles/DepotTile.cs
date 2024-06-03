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

    private Rover currentRover; 
    private float currentMalfunctionDuration;

    public override void TileAction(Rover interactingRover)
    {
        currentRover = interactingRover;

        switch(depotStatus)
        {
            case DepotStatus.Unexplored:
            {
                OpenDepot();
                if(depotStatus == DepotStatus.Opened)
                {
                    NotificationManager.instance.ShowInfoNotification("Depot at (" + x + ", " + y + ") successfully opened with " + depotSupplies + " supplies available.");
                }
                if(depotStatus == DepotStatus.Malfunctioned)
                {
                    NotificationManager.instance.ShowInfoNotification("Depot at (" + x + ", " + y + ") has a malfunction. Repair or Lab rover required for repair.");
                }
                break;
            }
            case DepotStatus.Opened:
            {
                // Query how much to take
                NotificationManager.instance.DepotAccessUI(this, AttemptDepotInteract);
                break;
            }
            case DepotStatus.Malfunctioned:
            {
                currentMalfunctionDuration = DetermineFixDuration();
                NotificationManager.instance.ShowQueryNotification("Depot will take " + currentMalfunctionDuration + " seconds to repair. Do you wish to start now?", StartDepotRepair);
                break;
            }
        }
    }

    public void StartDepotRepair()
    {
        // currentRover.AddTask(new RoverTask(currentMalfunctionDuration, this, 1));
    }

    // public override void FinishedTask(RoverTask finishedTask)
    // {
    //     base.FinishedTask(finishedTask);
    //     depotStatus = DepotStatus.Opened;
    //     acceptableRoverTypes.Clear();
    //     acceptableRoverTypes.Add(RoverType.Supply);
    // }

    // Method which returns the duration of the fix IN SECONDS
    public float DetermineFixDuration()
    {
        return 3f;
    }

    public void AttemptDepotInteract(int interactAmount, bool taking)
    {
        if(taking)
        {
            if(interactAmount > depotSupplies)
            {
                NotificationManager.instance.ShowInfoNotification("Error: Supply Rover attempted to take " + interactAmount + " supplies, yet Depot only has " + depotSupplies + " supplies.");
            }
            else
            {
                currentRover.AddSupplies(TakeDepotSupplies(interactAmount));
                NotificationManager.instance.ShowInfoNotification("Successfully took " + interactAmount + " supplies. Depot now has " + depotSupplies + " remaining supplies.");
            }
        }
        else
        {
            if(interactAmount > depotCapacity - depotSupplies)
            {
                NotificationManager.instance.ShowInfoNotification("Error: Supply Rover attempted to store " + interactAmount + " supplies, yet Depot only has room for " + (depotCapacity - depotSupplies) + " supplies.");
            }
            else
            {
                if(currentRover.GetSupplies() < interactAmount)
                {
                    NotificationManager.instance.ShowInfoNotification("Error: Supply Rover attempted to store " + interactAmount + " supplies, yet Rover only has " + currentRover.GetSupplies() + " supplies.");
                }
                else
                {
                    currentRover.TakeSupplies(StoreDepotSupplies(interactAmount));
                    NotificationManager.instance.ShowInfoNotification("Successfully stored " + interactAmount + " supplies. Depot now has " + depotSupplies + " total supplies.");
                }
            }
        }
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


