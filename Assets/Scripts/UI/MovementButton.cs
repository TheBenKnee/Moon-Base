using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementButton : MonoBehaviour
{
    [SerializeField] private GameLogic gameLogic;
    [SerializeField] private Direction direction;

    public void MovementSelected()
    {
        gameLogic.MoveSelectedRover(direction);
    }
}
