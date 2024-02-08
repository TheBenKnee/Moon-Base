using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabRover : Rover
{
    private void Awake() 
    {
        myRoverType = RoverType.Lab;
        supplies = 25;
    }
}
