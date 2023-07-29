using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DepotUIHandler : MonoBehaviour
{
    public GameObject DepotSupplyUIObject;
    public TextMeshProUGUI depotDescription;
    public Slider depotSliderAmount;
    public DepotToggleSlider myToggleSlider;

    public int storageCapacity;
    public int currentStorage;

    [ContextMenu("ToggleDepotUI")]
    public void ToggleDepotUIObject()
    {
        DepotSupplyUIObject.SetActive(!DepotSupplyUIObject.activeSelf);

        if(DepotSupplyUIObject.activeSelf)
        {
            if(!myToggleSlider.isOn)
            {
                SetDebotSliderAmounts(0, currentStorage);
            }
            else
            {
                SetDebotSliderAmounts(0, storageCapacity - currentStorage);
            }
        }
    }

    public void SetDebotSliderAmounts(int min, int max)
    {
        depotSliderAmount.maxValue = max;
        depotSliderAmount.minValue = min;

        depotSliderAmount.value = depotSliderAmount.minValue;

        depotDescription.text = "Depot has '" + currentStorage + "' available supplies to take. Depot can hold '" + (storageCapacity - currentStorage) + "'\nmore supplies. How many do you wish to take or leave?";
    }

    public void ConfirmSelection()
    {
        if(!myToggleSlider.isOn)
        {
            currentStorage -= (int)depotSliderAmount.value;
            SetDebotSliderAmounts(0, currentStorage);
        }
        else
        {
            currentStorage += (int)depotSliderAmount.value;
            SetDebotSliderAmounts(0, storageCapacity - currentStorage);
        }
    }
}
