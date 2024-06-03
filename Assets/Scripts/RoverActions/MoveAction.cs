using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : Action
{
    private string actionName = "Move";
    private MoonUI moonUI;

    public MoveAction(Rover rover, MoonTile tile, MoonUI moonUI) : base(rover, tile)
    {
        this.moonUI = moonUI;
    }

    public override void Execute()
    {
        if(myRover.GetRoverStatus() == RoverStatus.Ready)
        {
            moonUI.RevealDice();
        }
        else
        {
            NotificationManager.instance.ShowInfoNotification("Rover cannot move as it's currently in " + myRover.GetRoverStatus().ToString() + " state.");
        }
    }
}