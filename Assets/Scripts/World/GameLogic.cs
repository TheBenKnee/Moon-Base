using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private MoonUI moonUI;
    [SerializeField] private Dice dice;

    protected Rover activeRover;

    public void SetActiveRover(Rover newRover)
    {
        activeRover = newRover;
        moonUI.RoverSelectedUIUpdates(newRover);
    }

    public Rover GetActiveRover()
    {
        return activeRover;
    }

    public void TriggerTileActionOnActiveRover()
    {
        MoonTile currentTile = activeRover.GetRoverPosition();
        if(currentTile is InteractableTile)
        {
            ((InteractableTile)currentTile).TileAction(activeRover);
        }
    }

    public void DiceResult(int result)
    {
        if(!activeRover.CheckSupplies(Constants.SUPPLY_REQUIREMENT(result)))
        {
            //Trigger recovery operations
        }
        activeRover.SetRemainingMovement(result);
    }

    public void MoveSelectedRover(Direction direction)
    {
        if(dice.CheckMovementAvailable() && activeRover.CheckDirectionAvailable(direction))
        {
            // Reduce the dice number
            dice.ReduceDieNumber();

            // Move the rover
            activeRover.MoveRover(direction, 3);
            activeRover.SetRemainingMovement(activeRover.GetRemainingMovement() - 1);

            // Update UI
            moonUI.UpdateSupplyUI(activeRover);
            moonUI.FocusCameraOnRover(activeRover);
        }
        else
        {
            // Indicate move impossible
        }
    }
}
