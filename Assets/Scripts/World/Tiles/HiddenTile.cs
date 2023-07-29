using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenTile : MoonTile
{
    private bool revealed;
    private Sprite revealedSprite;
    private Sprite hiddenSprite;

    public void RevealHidden()
    {
        revealed = true;
        TriggerReveal();
    }

    public void ReHideSprite()
    {
        revealed = false;
        TriggerReHiding();
    }

    protected virtual void TriggerReveal()
    {
        GetComponent<SpriteRenderer>().sprite = revealedSprite;
    }

    protected virtual void TriggerReHiding()
    {
        GetComponent<SpriteRenderer>().sprite = hiddenSprite;
    }
}