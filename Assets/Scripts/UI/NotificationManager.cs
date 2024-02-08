using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private GameObject upFacingIndication;
    [SerializeField] private GameObject downFacingIndication;

    [SerializeField] private SlidingUIObject infoNotification;
    [SerializeField] private TextMeshProUGUI infoNotificationText;
    [SerializeField] private SlidingUIObject queryNotification;
    [SerializeField] private TextMeshProUGUI queryNotificationText;
    
    [SerializeField] private SlidingUIObject depotAccessNotification;
    [SerializeField] private DepotAccessUI depotAccessUI;

    public delegate void NotificationClickAction();
    public delegate void QueryConfiredClickAction();
    public delegate void QueryDenyClickAction();
    public delegate void DepotInteractClickAction(Rover rover, int supplies, bool taking);

    public static event NotificationClickAction onNotificationClick;
    public static event QueryConfiredClickAction onQueryConfirmed;
    public static event QueryDenyClickAction onQueryDenied;
    public static event DepotInteractClickAction onDepotInteractRequest;

    private Rover savedRover;

    public void DepotAccessUI(DepotTile depotTile, Rover interactingRover, DepotInteractClickAction interactAction)
    {
        depotAccessUI.OpenDepotUI(depotTile);

        onDepotInteractRequest = null;

        onDepotInteractRequest += interactAction;

        savedRover = interactingRover;

        depotAccessNotification.ShowObject();
    }

    public void OnDepotConfirmedClicked()
    {
        onDepotInteractRequest?.Invoke(savedRover, depotAccessUI.GetCurrentAmount(), depotAccessUI.IsTaking());

        depotAccessNotification.HideObject();
    }

    public void OnDepotDeniedClicked()
    {
        depotAccessNotification.HideObject();
    }

    public void ShowInfoNotification(string newText, NotificationClickAction confirmAction = null)
    {
        infoNotificationText.text = newText;

        onNotificationClick = null;

        onNotificationClick += confirmAction;

        infoNotification.ShowObject();
    }

    public void OnInfoNotificationClicked()
    {
        onNotificationClick?.Invoke();

        infoNotification.HideObject();
    }

    public void ShowQueryNotification(string newText, QueryConfiredClickAction confirmedAction, QueryDenyClickAction deniedAction = null)
    {
        queryNotificationText.text = newText;

        onQueryConfirmed = null;
        onQueryDenied = null;

        onQueryConfirmed += confirmedAction;
        onQueryDenied += deniedAction;

        queryNotification.ShowObject();
    }

    public void OnQueryConfirmedClicked()
    {
        onQueryConfirmed?.Invoke();

        queryNotification.HideObject();
    }

    public void OnQueryDeniedClicked()
    {
        onQueryDenied?.Invoke();

        queryNotification.HideObject();
    }
}
