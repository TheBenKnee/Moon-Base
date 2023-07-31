using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairRover : Rover
{
    private void Awake() 
    {
        myRoverType = RoverType.Repair;
    }
}
