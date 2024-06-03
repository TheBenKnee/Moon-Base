using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionMenuUIHandler : MonoBehaviour
{ 
    [SerializeField] private MoonUI moonUI;
    [SerializeField] private GameLogic gameLogic;

    public List<GameObject> LabActions = new List<GameObject>();
    public List<GameObject> RepairActions = new List<GameObject>();
    public List<GameObject> ScienceActions = new List<GameObject>();
    public List<GameObject> SupplyActions = new List<GameObject>();

    [SerializeField] private GameObject actionMenuContent;

    public void GenerateActions(Rover newRover)
    {
        foreach(GameObject actionObject in GetListOfActions(newRover))
        {
            GameObject newAction = Instantiate(actionObject);
            newAction.transform.parent = actionMenuContent.transform;
            newAction.GetComponent<ActionUIObject>().InitializeAction(gameLogic, moonUI);
        }
    }

    public List<GameObject> GetListOfActions(Rover requestedRover)
    {
        if(requestedRover.GetType() == typeof(LabRover))
        {
            return LabActions;
        }
        if(requestedRover.GetType() == typeof(RepairRover))
        {
            return RepairActions;
        }
        if(requestedRover.GetType() == typeof(ScienceRover))
        {
            return ScienceActions;
        }
        if(requestedRover.GetType() == typeof(SupplyRover))
        {
            return SupplyActions;
        }
        return new List<GameObject>();
    }
}
