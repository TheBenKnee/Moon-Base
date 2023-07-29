using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepotToggleSlider : ToggleSlider
{
    public DepotUIHandler myDepotUIHandler;

    public override void TogglePosition()
    {
        base.TogglePosition();
        if(!isOn)
        {
            myDepotUIHandler.SetDebotSliderAmounts(0, myDepotUIHandler.currentStorage);
        }
        else
        {
            myDepotUIHandler.SetDebotSliderAmounts(0, myDepotUIHandler.storageCapacity - myDepotUIHandler.currentStorage);
        }
    }
}
