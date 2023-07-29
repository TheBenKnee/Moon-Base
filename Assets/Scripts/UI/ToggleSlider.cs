using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleSlider : MonoBehaviour
{
    public TextMeshProUGUI initialText;
    public TextMeshProUGUI finalText;

    public bool isOn; // Should initially be false
    public bool isChanging;

    public float speed = -1000; // Negative if it should move right first, positive if it should move left first. 

    public float initialXPosition = 0f;
    public float finalXPostition = 0f;

    public virtual void TogglePosition()
    {
        isOn = !isOn;
        if(isOn)
        {
            initialText.gameObject.SetActive(false);
        }
        else
        {
            finalText.gameObject.SetActive(false);
        }
        isChanging = true;
        speed = -speed;
    }

    private void FixedUpdate() 
    {
        if(isChanging)
        {
            transform.position = new Vector3(transform.position.x + (Time.deltaTime * speed), transform.position.y, transform.position.z);
            if(isOn)
            {
                if(transform.position.x > finalXPostition)
                {
                    finalText.gameObject.SetActive(true);
                    transform.position = new Vector3(finalXPostition, transform.position.y, transform.position.z);
                    isChanging = false;
                }
            }
            else
            {
                if(transform.position.x < initialXPosition)
                {
                    initialText.gameObject.SetActive(true);
                    transform.position = new Vector3(initialXPosition, transform.position.y, transform.position.z);
                    isChanging = false;
                }
            }
        }
    }

    // Helper function to determine the "real" position of the image at different times.
    // Use this to determine what value to input initial and final XPositions as.
    [ContextMenu("DetermineLocation")]
    public void DetermineLocation()
    {
        Debug.Log(transform.position.x);
    }
}
