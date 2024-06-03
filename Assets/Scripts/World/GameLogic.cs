using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private MoonUI moonUI;
    [SerializeField] private Dice dice;
    [SerializeField] private GameObject movementButtons;

    protected Rover activeRover;

    private List<Rover> roverList = new List<Rover>();

    public void RoverHookup(List<Rover> createdRovers) 
    {
        roverList.Clear();
        foreach(Rover rover in createdRovers)
        {
            roverList.Add(rover);
            rover.OnTaskCompleted += HandleTaskCompleted;
        }
    }

    private void HandleTaskCompleted(Rover rover, TaskType taskType)
    {
        switch(taskType)
        {
            case TaskType.ExperimentCollection:
            {
                break;
            }
            case TaskType.ExperimentRepair:
            {
                break;
            }
            case TaskType.SupplyDepotOpening:
            {
                break;
            }
            case TaskType.SupplyDepotRepairing:
            {
                break;
            }
            default:
            {
                break;
            }
        }
    }

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

    public void UpdateSupplyCountUIOnActiveRover()
    {
        moonUI.UpdateSupplyUI(activeRover);
    }

    public void DiceResult(int result)
    {
        if(!activeRover.CheckSupplies(Constants.SUPPLY_REQUIREMENT(result)))
        {
            NotificationManager.instance.ShowInfoNotification("Entering recovery mode! Rover has reached danger low supply level. Resupply with Supply or Repair rovers!");
            activeRover.EnterRecoveryMode();
            dice.ClearDice();
        }
        else
        {
            if(result == 6)
            {
                NotificationManager.instance.ShowQueryNotification("Rolled 6! Move 6 spaces for free (yes) or gain " + Constants.FREE_SUPPLIES_FROM_6 + " free supplies (no).", FreeMovementSelected, FreeSuppliesSelected);
            }
            else
            {
                activeRover.SetRemainingMovement(result);
                activeRover.TakeSupplies(Constants.SUPPLY_REQUIREMENT(result));
                UpdateSupplyCountUIOnActiveRover();
                movementButtons.SetActive(true);
            }
            
        }
    }

    public void FreeMovementSelected()
    {
        activeRover.SetRemainingMovement(6);
        movementButtons.SetActive(true);
    }

    public void FreeSuppliesSelected()
    {
        dice.ClearDice();
        activeRover.AddSupplies(Constants.FREE_SUPPLIES_FROM_6);
        UpdateSupplyCountUIOnActiveRover();
    }

    public void MoveSelectedRover(Direction direction)
    {
        if(dice.CheckMovementAvailable() && activeRover.CheckDirectionAvailable(direction) && activeRover.GetRemainingMovement() > 0)
        {
            // Reduce the dice number
            dice.ReduceDieNumber();

            // Move the rover
            activeRover.MoveRover(direction);
            activeRover.SetRemainingMovement(activeRover.GetRemainingMovement() - 1);

            // Update UI
            moonUI.FocusCameraOnRover(activeRover);
        }
        else
        {
            // Indicate move impossible
            Debug.Log("Failed to move rover.");
        }

        if(activeRover.GetRemainingMovement() <= 0)
        {
            movementButtons.SetActive(false);
        }
    }
}
