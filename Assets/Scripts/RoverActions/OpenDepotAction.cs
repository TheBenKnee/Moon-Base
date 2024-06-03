using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDepotAction : Action
{
    private string actionName = "Unlock Depot";

    public OpenDepotAction(Rover rover, MoonTile tile) : base(rover, tile)
    {
        
    }

    public override void Execute()
    {
        if(myRover.GetRoverStatus() != RoverStatus.Ready)
        {
            NotificationManager.instance.ShowInfoNotification("Rover cannot perform action as it's currently in " + myRover.GetRoverStatus().ToString() + " state.");
        }

        if(!Constants.DEPOT_OPENABLE_ROVERS.Contains(myRover.GetRoverType()))
        {
            NotificationManager.instance.ShowInfoNotification("Rover type " + myRover.GetRoverType().ToString() + " cannot open supply depot.");
        }
    }
}