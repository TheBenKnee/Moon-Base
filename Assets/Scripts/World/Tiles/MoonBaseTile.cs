using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonBaseTile : InteractableTile
{
    [SerializeField] private List<Sprite> moonBaseSprites;

    private MoonBase moonBase;

    public override void TileAction(Rover interactingRover)
    {
        switch(moonBase.GetMoonBaseStatus())
        {
            case MoonBaseStatus.Unopened:
            {
                break;
            }
            case MoonBaseStatus.Opened:
            {
                break;
            }
        }
    }


    // 'Overriding' MoonTile method for actions at tile creation
    public virtual void InitializeTile(MoonBase theMoonBase, int tileNumber, int xLocation, int yLocation, Moon parentMoon)
    {
        base.InitializeTile(xLocation, yLocation, parentMoon);
        moonBase = theMoonBase;

        GetComponent<SpriteRenderer>().sprite = moonBaseSprites[tileNumber - 1];

        acceptableRoverTypes.Add(RoverType.Repair);
        acceptableRoverTypes.Add(RoverType.Lab);
    }
}
