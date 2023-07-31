using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonUI : MonoBehaviour
{
    public Moon moonBackend;
    public InteractableQuery interactableQuery;

    public GameObject interactableQueryUIObject;

    public DepotUIHandler depotHandler;

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
}
