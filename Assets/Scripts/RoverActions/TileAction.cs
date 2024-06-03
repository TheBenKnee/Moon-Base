using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAction : Action
{
    private string actionName = "Tile Action";
    private GameLogic gameLogic;
    
    public TileAction(Rover rover, MoonTile tile, GameLogic gameLogic) : base(rover, tile)
    {
        this.gameLogic = gameLogic;
    }

    public override void Execute()
    {   
        if(myRover.GetRoverStatus() == RoverStatus.Ready)
        {
            gameLogic.TriggerTileActionOnActiveRover();
        }
        else
        {
            NotificationManager.instance.ShowInfoNotification("Rover cannot perform action as it's currently in " + myRover.GetRoverStatus().ToString() + " state.");
        }
    }
}