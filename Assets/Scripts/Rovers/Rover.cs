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
    protected int supplies;
    protected Color waitingColor = Color.yellow;
    protected Color defaultColor;
    private float waitTime = 0;

    protected Moon parentMoon;
    protected MoonTile roverPosition;

    private RoverTask currentTask;
    private Queue<RoverTask> roverTasks = new Queue<RoverTask>();

    private int remainingMovement;

    public void InitializeRover(Moon zeMoon, MoonTile newTile)
    {
        parentMoon = zeMoon;
        SetSupplies(Random.Range(20, 30));
        TeleportRoverToTile(newTile);
    }

    public bool CheckSupplies(int suppliesNeeded)
    {
        // Check to see if sufficient supplies
        if(supplies < suppliesNeeded)
        {
            // Trigger refill protocol
            Debug.Log("Not enough supplies for rover to move.");
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
    public void MoveRover(Direction direction, int suppliesNeeded)
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

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Frontend Methods
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public void FinishedTask(RoverTask finishedTask)
    {
        finishedTask.backReference.FinishedTask(finishedTask);
        GetComponent<SpriteRenderer>().color = defaultColor;
        currentTask = null;
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
