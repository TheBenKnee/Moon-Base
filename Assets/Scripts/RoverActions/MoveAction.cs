using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : RoverAction
{
    public override void TriggerAction()
    {
        moonUI.RevealDice();
    }
}