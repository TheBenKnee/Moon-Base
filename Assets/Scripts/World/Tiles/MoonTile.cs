using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonTile : MonoBehaviour
{
    protected Moon moonParent;
    public int x, y;

    public virtual void EnterTile(Rover enteringRover)
    {
        Debug.Log(enteringRover.GetRoverType().ToString() + " rover entering tile (" + x + ", " + y + ").");
    }

    public virtual void LeaveTile(Rover leavingRover)
    {

    }

    public virtual void InitializeTile(int xLocation, int yLocation, Moon parentMoon)
    {
        x = xLocation;
        y = yLocation;
        moonParent = parentMoon;
    }
}
