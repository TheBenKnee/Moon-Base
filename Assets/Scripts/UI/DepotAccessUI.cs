using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DepotAccessUI : MonoBehaviour
{
    public DepotUIHandler depotHandler;
    public TextMeshProUGUI depotDescription;
    public Slider depotSliderAmount;
    public DepotToggleSlider myToggleSlider;

    private DepotTile selectedTile;

    public void UpdateSliderValues()
    {
        if(!myToggleSlider.isOn)
        {
            SetDebotSliderAmounts(0, selectedTile.GetDepotSupplies());
        }
        else
        {
            SetDebotSliderAmounts(0, selectedTile.GetDepotRemainingStorage());
        }
    }

    public void OpenDepotUI(DepotTile tile)
    {
        selectedTile = tile;

        UpdateSliderValues();
    }

    public void SetDebotSliderAmounts(int min, int max)
    {
        depotSliderAmount.maxValue = max;
        depotSliderAmount.minValue = min;

        depotSliderAmount.value = depotSliderAmount.minValue;

        depotDescription.text = "Depot has '" + selectedTile.GetDepotSupplies() + "' available supplies to take. Depot can hold '" + selectedTile.GetDepotRemainingStorage() + "'\nmore supplies. How many do you wish to take or leave?";
    }

    public void ConfirmSelection()
    {
        if(!myToggleSlider.isOn)
        {
            int result = selectedTile.TakeDepotSupplies((int)depotSliderAmount.value);  //Todo: If the result != the slider value, accidentally requested too many. Maybe do something in this case. (Note: Safety was in place to prevent. Nothing deeper broke if this happened.)
            depotHandler.AddSuppliesToRover((int)depotSliderAmount.value);
        }
        else
        {
            if(depotHandler.QueryRoverSupplies() >= (int)depotSliderAmount.value)
            {
                int result = selectedTile.StoreDepotSupplies((int)depotSliderAmount.value); //Todo: If the result != the slider value, accidentally tried to store too many. Maybe do something in this case. (Note: Safety was in place to prevent. Nothing deeper broke if this happened.)
                depotHandler.TakeSuppliesFromRover((int)depotSliderAmount.value);
            }
            else
            {
                depotHandler.OverDepositError((int)depotSliderAmount.value);
            }
        }
        UpdateSliderValues();
    }
}
