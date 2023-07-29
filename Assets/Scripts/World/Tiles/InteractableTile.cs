using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTile : MoonTile
{
    public List<RoverType> acceptableRoverTypes = new List<RoverType>();

    // Method which takes a rover instance and returns false if it was not of a compatible type with this
    // interactable tile. Otherwise, executes the tile's function and returns true.
    public bool InteractWithTile(Rover interactingRover)
    {
        if(interactingRover.GetRoverStatus() == RoverStatus.Dead || !acceptableRoverTypes.Contains(interactingRover.GetRoverType()))
        {
            return false;
        }

        TileAction(interactingRover);
        return true;
    }

    public virtual void FinishedTask(RoverTask finishedTask)
    {
        
    }

    //  Void method which actually performs the action of the interactable tile. 
    public virtual void TileAction(Rover interactingRover)
    {

    }
}
