using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAction : RoverAction
{
    public override void TriggerAction()
    {
        gameLogic.TriggerTileActionOnActiveRover();
    }
}