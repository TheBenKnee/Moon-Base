using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoverMenuOption : MonoBehaviour
{
    private Rover myRover;
    private GameLogic gameLogic;

    [SerializeField] private TextMeshProUGUI displayName;

    public void InitializeOption(GameLogic newLogic, Rover newRover)
    {
        gameLogic = newLogic;
        myRover = newRover;
        displayName.text = newRover.GetRoverType().ToString() + "\nRover";
    }

    public void RoverSelected()
    {
        gameLogic.SetActiveRover(myRover);
    }
}
