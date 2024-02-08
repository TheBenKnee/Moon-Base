using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DepotUIHandler : MonoBehaviour
{
    public InteractableQuery interactQuery;
    public GameObject depotOpenedInfoUIObject;
    public GameObject depotSupplyUIObject;
    public GameObject depotOverDepositErrorUIObject;
    public DepotAccessUI depotAccess;

    public TextMeshProUGUI openedText;
    public TextMeshProUGUI overDepositText;

    public void InformOpening(DepotTile depot)
    {
        openedText.text = "Depot at (" + depot.x + ", " + depot.y + ") successfully opened with " + depot.GetDepotSupplies() + " supplies available.";
        depotOpenedInfoUIObject.SetActive(true);
    }

    public void InformMalfunction(DepotTile depot)
    {
        openedText.text = "Depot at (" + depot.x + ", " + depot.y + ") has a malfunction. Repair or Lab rover required for repair.";
        depotOpenedInfoUIObject.SetActive(true);
    }

    public void DepotStatusConfirmed()
    {
        depotOpenedInfoUIObject.SetActive(false);
        interactQuery.backupGameObject.SetActive(true);
    }

    public void OpenDepotUI(DepotTile depot)
    {
        depotSupplyUIObject.SetActive(true);
        depotAccess.OpenDepotUI(depot);
    }

    public void CloseDepotUI()
    {
        depotSupplyUIObject.SetActive(false);
        interactQuery.backupGameObject.SetActive(true);
    }

    public int QueryRoverSupplies()
    {
        return interactQuery.GetActiveRoverSupplies();
    }

    public void OverDepositError(int value)
    {
        depotOverDepositErrorUIObject.SetActive(true);
        depotSupplyUIObject.SetActive(false);
        overDepositText.text = "Error: Supply Rover attempted to store " + value + " supplies, yet only has " + QueryRoverSupplies() + " supplies.";
    }

    public void AddSuppliesToRover(int supplyNumber)
    {
        interactQuery.AddSuppliesToRover(supplyNumber);
    }

    public void TakeSuppliesFromRover(int supplyNumber)
    {
        interactQuery.TakeSuppliesFromRover(supplyNumber);
    }

    public void OverDepositAccepted()
    {
        depotOverDepositErrorUIObject.SetActive(false);
    }

    [ContextMenu("ToggleDepotUI")]
    public void ToggleDepotAccessUIObject()
    {
        depotSupplyUIObject.SetActive(!depotSupplyUIObject.activeSelf);

        if(depotSupplyUIObject.activeSelf)
        {
            depotAccess.UpdateSliderValues();
        }
    }
}
