using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyRover : Rover
{
    private void Awake() 
    {
        myRoverType = RoverType.Supply;
        supplies = 25;
    }
}
