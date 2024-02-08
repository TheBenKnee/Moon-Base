using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepotToggleSlider : ToggleSlider
{
    public DepotAccessUI myDepotAccessUI;

    public override void TogglePosition()
    {
        base.TogglePosition();
        myDepotAccessUI.UpdateSliderValues();
    }
}
