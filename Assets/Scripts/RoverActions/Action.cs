using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action 
{
    protected Rover myRover;
    protected MoonTile myTile;

    private string actionName = "Default Action";

    public Action(Rover rover, MoonTile tile)
    {
        this.myRover = rover;
        this.myTile = tile;
    }    

    public virtual void Execute()
    {

    }

    public string GetActionName()
    {
        return actionName;
    }
}