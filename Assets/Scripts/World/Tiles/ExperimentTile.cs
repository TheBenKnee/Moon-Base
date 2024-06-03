using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExperimentStatus
{
    Unexplored,
    Uncollected,
    Collected,
    Malfunctioned
}

public class ExperimentTile : InteractableTile
{
    [SerializeField] private List<Sprite> experimentSprites;  
    private ExperimentStatus experimentStatus = ExperimentStatus.Unexplored;

    private int experimentNumber;
    private bool keyExperiment;

    private Rover currentRover; 
    private float currentMalfunctionDuration;

    public override void TileAction(Rover interactingRover)
    {
        currentRover = interactingRover;

        switch(experimentStatus)
        {
            case ExperimentStatus.Unexplored:
            {
                AttemptExperimentCollection();
                if(experimentStatus == ExperimentStatus.Collected)
                {
                    NotificationManager.instance.ShowInfoNotification("Science Experiment #" + experimentNumber + " successfully collected!");
                }
                if(experimentStatus == ExperimentStatus.Malfunctioned)
                {
                    NotificationManager.instance.ShowInfoNotification("Science Experiment at (" + x + ", " + y + ") has a malfunction. Repair or Lab rover required for repair.");
                }
                break;
            }
            case ExperimentStatus.Collected:
            {
                NotificationManager.instance.ShowInfoNotification("Science Experiment at (" + x + ", " + y + ") has already been taken.");
                break;
            }
            case ExperimentStatus.Malfunctioned:
            {
                currentMalfunctionDuration = DetermineFixDuration();
                NotificationManager.instance.ShowQueryNotification("Science Experiment will take " + currentMalfunctionDuration + " seconds to repair. Do you wish to start now?", StartExperimentRepair);
                break;
            }
        }
    }

    public void StartExperimentRepair()
    {
        // currentRover.AddTask(new RoverTask(currentMalfunctionDuration, this, 1));
    }

    // public override void FinishedTask(RoverTask finishedTask)
    // {
    //     base.FinishedTask(finishedTask);
    //     experimentStatus = ExperimentStatus.Uncollected;
    //     acceptableRoverTypes.Clear();
    //     acceptableRoverTypes.Add(RoverType.Supply);
    // }

    // Method which returns the duration of the fix IN SECONDS
    public float DetermineFixDuration()
    {
        return 3f;
    }

    public void AttemptExperimentCollection()
    {
        int seed = Random.Range(0, 100);
        if(seed < 30)
        {
            experimentStatus = ExperimentStatus.Malfunctioned;
            acceptableRoverTypes.Clear();
            acceptableRoverTypes.Add(RoverType.Repair);
        }
        else
        {
            experimentStatus = ExperimentStatus.Collected;
        }
    }

    public bool GetImportance()
    {
        return this.keyExperiment;
    }

    public int GetExperimentNumber()
    {
        return this.experimentNumber;
    }

    // 'Overriding' MoonTile method for actions at tile creation
    public virtual void InitializeTile(int experimentNumber, bool isKeyExperiment, int xLocation, int yLocation, Moon parentMoon)
    {
        base.InitializeTile(xLocation, yLocation, parentMoon);
        
        this.experimentNumber = experimentNumber;
        this.keyExperiment = isKeyExperiment;

        GetComponent<SpriteRenderer>().sprite = experimentSprites[experimentNumber - 1];

        acceptableRoverTypes.Add(RoverType.Science);
        acceptableRoverTypes.Add(RoverType.Repair);
    }
}
