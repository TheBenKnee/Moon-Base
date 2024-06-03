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

    //  Void method which actually performs the action of the interactable tile. 
    public virtual void TileAction(Rover interactingRover)
    {

    }

    public override void LeaveTile(Rover leavingRover)
    {
        base.LeaveTile(leavingRover);
        moonParent.RemoveInteractableQueryAndBackup();
    }
}
