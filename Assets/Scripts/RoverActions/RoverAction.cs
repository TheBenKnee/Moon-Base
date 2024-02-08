using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverAction : MonoBehaviour
{
    [SerializeField] protected GameLogic gameLogic;
    [SerializeField] protected MoonUI moonUI;
    
    public void InitializeVariables(GameLogic incomingGameLogic, MoonUI incomingMoonUI)
    {
        gameLogic = incomingGameLogic;
        moonUI = incomingMoonUI;
    }

    public virtual void TriggerAction()
    {

    }
}
