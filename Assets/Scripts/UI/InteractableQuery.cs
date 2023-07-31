using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableQuery : MonoBehaviour
{
    // Front-end Variables
    public GameObject queryGameObject;
    public GameObject backupGameObject;
    public GameObject ineligibleRoverMessage;
    public TextMeshProUGUI queryText;
    public TextMeshProUGUI ineligibilityText;

    // Back-end Variables
    private InteractableTile interactableTile;
    private Rover interactingRover;

    public void QueryInteraction(InteractableTile newTile, Rover newRover)
    {
        interactableTile = newTile;
        interactingRover = newRover;
        
        if(interactableTile.GetType() == typeof(DepotTile))
        {
            queryText.text = "Interact with Supply Depot?";
        }
        if(interactableTile.GetType() == typeof(ScienceTile))
        {
            queryText.text = "Interact with Science Object?";
        }
        if(interactableTile.GetType() == typeof(MoonBaseTile))
        {
            queryText.text = "Interact with D.A.N. Moon Base?";
        }
    }
    
    public void ConfirmInteraction()
    {
        queryGameObject.SetActive(false);
        backupGameObject.SetActive(false);

        // Attempt interaction
        bool result = interactableTile.InteractWithTile(interactingRover);

        // Determine eligibility
        if(!result)
        {
            // Inform ineligibility
            ineligibleRoverMessage.SetActive(true);
            backupGameObject.SetActive(false);

            // Create string for list of acceptable rovers.
            string listOfEligibleRovers = "";
            foreach(RoverType eligibleRover in interactableTile.acceptableRoverTypes)
            {
                listOfEligibleRovers += eligibleRover.ToString() + " Rover, or";
            }

            // Remove the last comma and or from entry.
            if(listOfEligibleRovers.Length > 0)
            {
                listOfEligibleRovers = listOfEligibleRovers.Substring(0, listOfEligibleRovers.Length - 4);
                listOfEligibleRovers += " ";
            }

            ineligibilityText.text = "Error: Requires " + listOfEligibleRovers + "to interact with " + interactableTile.GetType().ToString() + ".";
        }
    }

    public void DenyInteraction()
    {
        queryGameObject.SetActive(false);
        backupGameObject.SetActive(true);
    }

    public void DenyAndSuppressBackupInteraction()
    {
        queryGameObject.SetActive(false);
        backupGameObject.SetActive(false);
    }

    public void BackupSelected()
    {
        queryGameObject.SetActive(true);
        backupGameObject.SetActive(false);
    }

    public void IneligibileErrorAccepted()
    {
        ineligibleRoverMessage.SetActive(false);
        backupGameObject.SetActive(true);
    }

    public int GetActiveRoverSupplies()
    {
        return interactingRover.GetSupplies();
    }

    public void AddSuppliesToRover(int supplyNum)
    {
        interactingRover.AddSupplies(supplyNum);
    }

    public void TakeSuppliesFromRover(int supplyNum)
    {
        interactingRover.TakeSupplies(supplyNum);
    }
}
