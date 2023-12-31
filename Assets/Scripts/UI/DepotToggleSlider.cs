using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepotToggleSlider : ToggleSlider
{
    public DepotUIHandler myDepotUIHandler;

    public override void TogglePosition()
    {
        base.TogglePosition();
        myDepotUIHandler.depotAccess.UpdateSliderValues();
    }
}
