using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceRover : Rover
{
    private void Awake() 
    {
        myRoverType = RoverType.Science;
        supplies = 25;
    }
}
