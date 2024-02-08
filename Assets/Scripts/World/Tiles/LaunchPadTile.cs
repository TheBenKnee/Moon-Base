using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPadTile : InteractableTile
{
    public void SetStartingRover(RoverType newRoverType)
    {
        acceptableRoverTypes.Clear();
        acceptableRoverTypes.Add(newRoverType);
    }
}
