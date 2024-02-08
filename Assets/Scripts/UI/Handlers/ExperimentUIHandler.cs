using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExperimentUIHandler : MonoBehaviour
{
    // public InteractableQuery interactQuery;
    public GameObject experimentInfoUIObject;
    public SlidingUIObject slidingObject;

    public TextMeshProUGUI openedText;

    public void InformCollection(ExperimentTile experiment)
    {
        openedText.text = "Science Experiment #" + experiment.GetExperimentNumber() + " successfully collected!";
        slidingObject.ShowObject();
    }

    public void InformMalfunction(ExperimentTile experiment)
    {
        openedText.text = "Science Experiment at (" + experiment.x + ", " + experiment.y + ") has a malfunction. Repair or Lab rover required for repair.";
        slidingObject.ShowObject();
    }

    public void InformExperimentAlreadyTaken(ExperimentTile experiment)
    {
        openedText.text = "Science Experiment at (" + experiment.x + ", " + experiment.y + ") has already been taken.";
        slidingObject.ShowObject();
    }

    public void CloseInfo()
    {
        slidingObject.HideObject();
    }
}
