using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        DefaultCursor();
    }

    public void UpdateCursor(Texture2D newTexture, Vector2 newHotSpot)
    {
        Cursor.SetCursor(newTexture, newHotSpot, cursorMode);
    }

    public void DefaultCursor()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
}
