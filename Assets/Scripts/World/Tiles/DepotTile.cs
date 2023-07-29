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

    public override void TileAction(Rover interactingRover)
    {
        switch(depotStatus)
        {
            case DepotStatus.Unexplored:
            {
                OpenDepot();
                break;
            }
            case DepotStatus.Opened:
            {
                // Querry how much to take
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

    public float DetermineFixDuration()
    {
        return 60f;
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
        switch(strength)
        {
            case DepotStrength.Full:
            {
                if(seed < 40)
                {
                    return 0;
                }
                else
                {
                    return 70;
                }
            }
            case DepotStrength.Half:
            {
                if(seed < 40)
                {
                    return 0;
                }
                else
                {
                    return 35;
                }
            }
            case DepotStrength.Weak:
            {
                if(seed < 40)
                {
                    return 0;
                }
                else
                {
                    return 20;
                }
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

    public DepotStatus GetDepotStatus()
    {
        return depotStatus;
    }

    public DepotStrength GetDepotStrength()
    {
        return depotStrength;
    }

    // 'Overriding' MoonTile method for actions at tile creation
    public virtual void InitializeTile(DepotStrength initializedStrength, int xLocation, int yLocation)
    {
        base.InitializeTile(xLocation, yLocation);
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


