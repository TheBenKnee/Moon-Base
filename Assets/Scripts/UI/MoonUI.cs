using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoonUI : MonoBehaviour
{
    [SerializeField] private Moon moonBackend;
    [SerializeField] private InteractableQuery interactableQuery;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject interactableQueryUIObject;

    [SerializeField] private DepotUIHandler depotHandler;
    [SerializeField] private ExperimentUIHandler experimentHandler;
    [SerializeField] private ActionMenuUIHandler actionHandler;
    [SerializeField] private RoverMenuHandler roverMenuHandler;

    [SerializeField] private TextMeshProUGUI roverName;
    [SerializeField] private GameObject roverActionContent;

    // Dice related objects
    [SerializeField] private GameObject diceHintObject;
    [SerializeField] private GameObject diceObject;
    [SerializeField] private GameObject movementButtons;

    [SerializeField] private TextMeshProUGUI roverSupplyAmount;

    public void UpdateSupplyUI(Rover rover)
    {
        roverSupplyAmount.text = rover.GetSupplies().ToString();
    }

    public void BuildRoverMenu(List<Rover> rovers)
    {
        foreach(Rover rover in rovers)
        {
            roverMenuHandler.AddOption(rover);
        }
    }

    public void RevealDice()
    {
        diceHintObject.SetActive(true);
        diceObject.SetActive(true);
    }

    public void HideDice()
    {
        diceHintObject.SetActive(false);
        diceObject.SetActive(false);
        movementButtons.SetActive(false);
    }

    public void FocusCameraOnRover(Rover rover)
    {
        // Move the camera to the rover
        mainCamera.transform.position = new Vector3(rover.transform.position.x, rover.transform.position.y, mainCamera.transform.position.z);
    }

    //Action Cancellations
    public void CancelActions()
    {
        // Go through each action and cancel any UI for them
        CancelMoveAction();
    }

    public void CancelMoveAction()
    {
        HideDice();
    }

    //End Action Cancellations

    public void RoverSelectedUIUpdates(Rover rover)
    {
        // Remove any remaining UI from previous rover
        CancelActions();
        
        // Move the camera to the rover
        FocusCameraOnRover(rover);

        // Update the UI to indicate selected rover
        roverName.text = rover.GetRoverType().ToString();

        // Remove any previous actions        
        foreach(Transform child in roverActionContent.transform)
        {
            Destroy(child.gameObject);
        }
        
        // Build new actions
        actionHandler.GenerateActions(rover);

        // Update UI's supply amount
        UpdateSupplyUI(rover);
    }

    public (int, int) GetDepotStrengthAndCapacity(int x, int y)
    {
        return (0, 0);
    }

    public void InformAndTriggerInteractableUI(InteractableTile tile, Rover interactingRover)
    {
        Debug.Log("Get to highest level front-end inform");
        if(tile.GetType() == typeof(DepotTile))
        {
            Debug.Log("Determined its a depot");
            interactableQueryUIObject.SetActive(true);
            interactableQuery.QueryInteraction((DepotTile)tile, interactingRover);
        }
    }

    public void SuppressInteractableAndBackupUI()
    {
        interactableQuery.DenyAndSuppressBackupInteraction();
    }

    public void TriggerDepotOpenedInfo(DepotTile depot)
    {
        depotHandler.InformOpening(depot);
    }

    public void TriggerDepotMalfunctionedUI(DepotTile depot)
    {
        depotHandler.InformMalfunction(depot);
    }

    public void TriggerDepotSuppliesInfo(DepotTile depot)
    {
        depotHandler.OpenDepotUI(depot);
    }

    public void TriggerExperimentCollectedUI(ExperimentTile tile)
    {
        experimentHandler.InformCollection(tile);
    }

    public void TriggerExperimentMalfunctionedUI(ExperimentTile tile)
    {
        experimentHandler.InformMalfunction(tile);
    }

    public void TriggerExperimentAlreadyTakenUI(ExperimentTile tile)
    {
        experimentHandler.InformExperimentAlreadyTaken(tile);
    }
}
