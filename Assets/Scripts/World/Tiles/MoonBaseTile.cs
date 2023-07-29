using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonBaseTile : InteractableTile
{
    private MoonBase moonBase;

    public override void TileAction(Rover interactingRover)
    {
        switch(moonBase.GetMoonBaseStatus())
        {
            case MoonBaseStatus.Unopened:
            {
                break;
            }
            case MoonBaseStatus.Opened:
            {
                // Querry how much to take
                break;
            }
        }
    }


    // 'Overriding' MoonTile method for actions at tile creation
    public virtual void InitializeTile(MoonBase theMoonBase, int xLocation, int yLocation)
    {
        base.InitializeTile(xLocation, yLocation);
        moonBase = theMoonBase;

        acceptableRoverTypes.Add(RoverType.Repair);
        acceptableRoverTypes.Add(RoverType.Lab);
    }
}
