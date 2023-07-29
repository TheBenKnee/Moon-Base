using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonTile : MonoBehaviour
{
    public int x, y;

    public virtual void EnterTile(Rover enteringRover)
    {

    }

    public virtual void InitializeTile(int xLocation, int yLocation)
    {
        x = xLocation;
        y = yLocation;
    }
}
