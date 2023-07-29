using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoverType
{
    Lab,
    Repair,
    Science,
    Supply,
    Generic
}

public enum RoverStatus
{
    Ready,
    Waiting,
    Dead
}

public class RoverTask
{
    public float totalTime;
    public float elapsedTime;
    public InteractableTile backReference;
    public int infoValue;

    public RoverTask(float duration, InteractableTile initialBackRef, int initialInfoValue)
    {
        backReference = initialBackRef;
        totalTime = duration;
        elapsedTime = duration;
        infoValue = initialInfoValue;
    }

    public void TickElapsed(float tick, Rover refRover)
    {
        elapsedTime -= tick;

        if(elapsedTime <= 0)
        {
            elapsedTime = 0;
            refRover.FinishedTask(this);
        }
    }
}

public class Rover : MonoBehaviour
{
    protected RoverType myRoverType;
    protected RoverStatus myRoverStatus = RoverStatus.Ready;
    private int supplies;
    public Color waitingColor = Color.yellow;
    public Color defaultColor;
    private float waitTime = 0;

    private RoverTask currentTask;
    private Queue<RoverTask> roverTasks = new Queue<RoverTask>();


    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Frontend Methods
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public void FinishedTask(RoverTask finishedTask)
    {
        finishedTask.backReference.FinishedTask(finishedTask);
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    public void AddTask(RoverTask taskToAdd)
    {
        roverTasks.Enqueue(taskToAdd);
    }

    private void FixedUpdate() 
    {
        if(currentTask != null)
        {
            currentTask.TickElapsed(Time.deltaTime, this);
        }
        else
        {
            if(roverTasks.Count > 0)
            {
                GetComponent<SpriteRenderer>().color = waitingColor;
                myRoverStatus = RoverStatus.Waiting;
                currentTask = roverTasks.Dequeue();
            }
        }
    }

    private void Awake() 
    {
        defaultColor = GetComponent<SpriteRenderer>().color;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Health Methods
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    public virtual void Die()
    {
        myRoverStatus = RoverStatus.Dead;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Supply Methods
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void AddSupplies(int incomingSupplies)
    {
        supplies += incomingSupplies;
    }

    public int TakeSupplies(int takeSupplies)
    {
        if(supplies <= takeSupplies)
        {
            int tempSupplies = supplies;
            supplies = 0;
            return tempSupplies;
        }

        supplies -= takeSupplies;
        return takeSupplies;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    // GET/SET Methods
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    public RoverStatus GetRoverStatus()
    {
        return myRoverStatus;
    }

    public RoverType GetRoverType()
    {
        return myRoverType;
    }

    public int GetSupplies()
    {
        return supplies;
    }

    public void SetSupplies(int newSupplies)
    {
        supplies = newSupplies;
    }
}
