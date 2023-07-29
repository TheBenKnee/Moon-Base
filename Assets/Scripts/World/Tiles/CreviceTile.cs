using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreviceTile : HiddenTile
{
    public override void EnterTile(Rover enteringRover)
    {
        enteringRover.Die();
    }

    protected override void TriggerReveal()
    {
        base.TriggerReveal();
    }

    protected override void TriggerReHiding()
    {
        base.TriggerReHiding();
    }
}
