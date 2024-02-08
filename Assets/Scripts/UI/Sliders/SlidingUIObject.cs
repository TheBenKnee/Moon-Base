using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingUIObject : MonoBehaviour
{
    [SerializeField] private Vector3 shownPosition;
    [SerializeField] private Vector3 hiddenPosition;

    [SerializeField] private float speed;
    [SerializeField] private bool vertical;
    [SerializeField] private bool hidingNegative;

    private bool hiding = false;
    private bool showing = false;

    private bool hidden = true;

    [SerializeField] private GameObject revealHideButton;

    [ContextMenu("TestLoction")]
    public void TestLoction()
    {
        Debug.Log(transform.position);
    }

    public void TriggerMovement()
    {
        if(hidden)
        {
            ShowObject();
        }
        else
        {
            HideObject();
        }
    }

    [ContextMenu("ShowObject")]
    public void ShowObject()
    {
        showing = true;
    }

    [ContextMenu("HideObject")]
    public void HideObject()
    {
        hiding = true;
    }

    [ContextMenu("GetStatus")]
    public void GetStatus()
    {
        Debug.Log("Location: " + transform.position);
        Debug.Log("Hiding: " + hiding);
        Debug.Log("Showing: " + showing);
        Debug.Log("Vertical: " + vertical);
        Debug.Log("HidingNegative: " + hidingNegative);
    }

    private void Update() 
    {
        if ((hiding && !showing) || (!hiding && showing))
        {
            float offset = hidingNegative ? -speed : speed;
            offset = showing ? -offset : offset;

            if((hiding && ((vertical && ((hidingNegative && transform.position.y <= hiddenPosition.y) || (!hidingNegative && transform.position.y >= hiddenPosition.y))) || (!vertical && ((hidingNegative && transform.position.x <= hiddenPosition.x) || (!hidingNegative && transform.position.x >= hiddenPosition.x))))) || (showing && ((vertical && ((hidingNegative && transform.position.y >= shownPosition.y) || (!hidingNegative && transform.position.y <= shownPosition.y))) || (!vertical && ((hidingNegative && transform.position.x >= shownPosition.x) || (!hidingNegative && transform.position.x <= shownPosition.x))))))
            {
                if(hiding)
                {
                    hidden = true;
                }
                else
                {
                    hidden = false;
                }
                
                hiding = false;
                showing = false;

                if(revealHideButton != null)
                {
                    revealHideButton.transform.Rotate(0f, 0f, 180.0f);
                }

                return;
            }

            if (vertical)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
            }
        }
    }
}
