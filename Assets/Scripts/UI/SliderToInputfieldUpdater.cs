using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderToInputfieldUpdater : MonoBehaviour
{
    public TMP_InputField inputField;
    public Slider slider;

    public void OnSliderChange()
    {
        inputField.text = slider.value.ToString();
    }
}
