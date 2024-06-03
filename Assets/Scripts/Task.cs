using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType 
{
    ExperimentCollection,
    ExperimentRepair,
    SupplyDepotOpening,
    SupplyDepotRepairing
}

public class Task
{
    public float duration { get; private set; }
    private float elapsedTime;
    public bool isComplete { get; private set; }
    public TaskType taskType { get; private set; }

    public Task(float duration, TaskType taskType) 
    {
        this.duration = duration;
        elapsedTime = 0;
        isComplete = false;
        this.taskType = taskType;
    }

    public void Start() 
    {
        elapsedTime = 0;
        isComplete = false;
    }

    public void UpdateTask() 
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= duration) 
        {
            isComplete = true;
        }
    }
}
