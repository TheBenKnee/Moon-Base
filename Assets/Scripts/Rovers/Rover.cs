using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    PerformingTask,
    Recovery,
    Dead
}

public class Rover : MonoBehaviour
{
    protected RoverType myRoverType;
    protected RoverStatus myRoverStatus = RoverStatus.Ready;
    protected int supplies;
    protected Color waitingColor = Color.yellow;
    protected Color defaultColor;
    protected Task currentTask;

    public event Action<Rover, TaskType> OnTaskCompleted;

    protected Moon parentMoon;
    protected MoonTile roverPosition;

    private int remainingMovement;

    public void InitializeRover(Moon zeMoon, MoonTile newTile)
    {
        parentMoon = zeMoon;
        SetSupplies(UnityEngine.Random.Range(20, 30));
        TeleportRoverToTile(newTile);
    }

    public bool CheckSupplies(int suppliesNeeded)
    {
        // Check to see if sufficient supplies
        if(supplies < suppliesNeeded)
        {
            // Trigger refill protocol
            return false;
        }

        return true;
    }

    public bool CheckDirectionAvailable(Direction direction)
    {
        // Get new position based on diretion of movement
        MoonTile newPosition = parentMoon.GetTileAdjacent(roverPosition, direction);
        if(newPosition == null)
        {
            Debug.Log("No tile in direction to move to.");
            return false;
        }

        return true;
    }

    // Will move the rover and trigger any tile interactions. Will return true if movement possible, false if not.
    public void MoveRover(Direction direction, int suppliesNeeded = 0)
    {
        // Update the backend supply number
        TakeSupplies(suppliesNeeded);

        // Get the new position
        MoonTile newPosition = parentMoon.GetTileAdjacent(roverPosition, direction);

        // Trigger any leave conditions
        roverPosition.LeaveTile(this);

        // Switch internal tracking
        roverPosition = newPosition;

        // Trigger any enter conditions
        roverPosition.EnterTile(this);

        // Move rover visually (TODO: Maybe move this somewhere else)
        gameObject.transform.position = new Vector3(roverPosition.x + 0.5f, roverPosition.y + 0.5f, 0);
    }

    public void TeleportRoverToTile(MoonTile newTile)
    {
        // Switch internal tracking
        roverPosition = newTile;

        // Move rover visually (TODO: Maybe move this somewhere else)
        gameObject.transform.position = new Vector3(roverPosition.x + 0.5f, roverPosition.y + 0.5f, 0);
    }

    [ContextMenu("RestRoverMovement")]
    public void TestRoverMovement()
    {
        MoveRover(Direction.North, 0);
    }

    public void PerformTask(Task task)
    {
        if(myRoverStatus != RoverStatus.Ready)
        {
            return;
        }

        myRoverStatus = RoverStatus.PerformingTask;
        currentTask = task;
        currentTask.Start();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Frontend Methods
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Update() 
    {
        if (myRoverStatus == RoverStatus.PerformingTask && currentTask != null)
        {
            currentTask.UpdateTask();
            if(currentTask.isComplete)
            {
                myRoverStatus = RoverStatus.Ready;
                OnTaskCompleted?.Invoke(this, currentTask.taskType);
                currentTask = null;
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

    public void EnterRecoveryMode()
    {
        myRoverStatus = RoverStatus.Recovery;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    // GET/SET Methods
    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetRemainingMovement()
    {
        return remainingMovement;
    }

    public void SetRemainingMovement(int newMovement)
    {
        remainingMovement = newMovement;
    }

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

    public MoonTile GetRoverPosition()
    {
        return roverPosition;
    }
}
