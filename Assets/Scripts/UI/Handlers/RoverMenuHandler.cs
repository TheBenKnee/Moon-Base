using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject roverOptionPrefab;

    [SerializeField] private GameObject menuContent;

    [SerializeField] private GameLogic gameLogic;

    public void ClearList()
    {
        foreach(Transform child in menuContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddOption(Rover newRover)
    {
        GameObject newOption = Instantiate(roverOptionPrefab);
        newOption.transform.SetParent(menuContent.transform);

        RoverMenuOption newOptionComponent = newOption.GetComponent<RoverMenuOption>();
        newOptionComponent.InitializeOption(gameLogic, newRover);
    }
}
