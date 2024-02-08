using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderTile : MonoBehaviour
{
    [SerializeField] private List<Sprite> borderTileSprites;  
    
    public void InitializeTile(int borderNumber)
    {
        GetComponent<SpriteRenderer>().sprite = borderTileSprites[borderNumber];
    }
}
