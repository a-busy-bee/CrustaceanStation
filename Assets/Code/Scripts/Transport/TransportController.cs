using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class TransportController : MonoBehaviour
{
    protected TransportPath rail;
    // MOVEMENT
    [SerializeField] protected RectTransform transportTransform;
    protected Vector3 startingPosArrive = new Vector3(0, 882, 0); // where the train is before it moves into the station
    protected Vector3 startingPosDepart = new Vector3(0, 175, 0); // also endPosArrive
    protected Vector3 endPosDepart = new Vector3(0, -1084, 0); // where the train goes to be completely offscreen
    protected Vector3 currentVelocity;

    // IDS and INFO
    protected int coins = 0;

    // CARTS
    [SerializeField] protected List<TransportSelection> transportSelections = new List<TransportSelection>();

    // STATE MACHINE
    public enum TransportState
    {
        Setup,          // initialize the train  
        NotArrived,     // train hasn't moved into station yet
        Arriving,       // train is moving into the station
        NotBoarding,    // train is in station but not boarding (crab hasn't been approved)
        Boarding,       // train is in station and is boarding (crab has been approved)
        Departing,      // train is moving out of the station
        Departed        // train is offscreen
    }
    protected TransportState transportState;



    // State machine go brrrrr
    virtual public void SetState(TransportState newState)
    {
        TransportState prevState = transportState;
        transportState = newState;

        switch (transportState)
        {
            case TransportState.Setup:
                {
                    // do nothing, logic already in InitTrain
                }
                break;

            case TransportState.NotArrived:
                {
                    // set starting position
                    transportTransform.anchoredPosition = startingPosArrive;
                }
                break;

            case TransportState.Arriving:
                {
                    // nothing to do here, all logic in Update()
                }
                break;

            case TransportState.NotBoarding:
                {
                    SetThisClickable(false);
                }
                break;

            case TransportState.Boarding:
                {
                    SetThisClickable(true);
                }
                break;

            case TransportState.Departing:
                {
                    // give player coins
                    coins = 0;

                    foreach (TransportSelection ts in transportSelections)
                    {
                        ts.SetThisClickable(false);
                        coins += ts.Depart();
                    }
                    Kiosk.instance.GivePlayerCoins(coins);
                }
                break;

            case TransportState.Departed:
                {
                    //LevelManager.instance.RemoveCurrentTrain(this);
                    rail.Summon();

                    Destroy(gameObject);
                }
                break;
        }
    }

    virtual public void InitTransport(TransportPath newRail)
    {
        rail = newRail;
        GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);

        transportTransform.anchoredPosition = startingPosArrive;
        SetState(TransportState.Arriving);
    }

    protected void SetThisClickable(bool isClickable)
    {
        foreach (TransportSelection ts in transportSelections)
        {
            ts.SetThisClickable(isClickable);
        }
    }

    protected void Update()
    {
        switch (transportState)
        {
            case TransportState.Arriving:
                {
                    // move train down from offscreen
                    transportTransform.anchoredPosition = Vector3.SmoothDamp(transportTransform.anchoredPosition, startingPosDepart, ref currentVelocity, 0.5f);

                    if (Vector2.Distance(transportTransform.anchoredPosition, startingPosDepart) < 0.1f)
                    {
                        if (Kiosk.instance.kioskState == Kiosk.KioskState.CrabApproved)
                        {
                            SetState(TransportState.Boarding);
                        }
                        else
                        {
                            SetState(TransportState.NotBoarding);
                        }
                    }
                }
                break;

            case TransportState.Departing:
                {
                    // move train down from onscreen
                    transportTransform.anchoredPosition = Vector3.SmoothDamp(transportTransform.anchoredPosition, endPosDepart, ref currentVelocity, 0.5f);

                    if (Vector2.Distance(transportTransform.anchoredPosition, endPosDepart) < 100f)
                    {
                        SetState(TransportState.Departed);
                    }
                }
                break;
        }
    }

    public void SetBoarding(bool isBoarding)
    {
        if (transportState == TransportState.NotBoarding || transportState == TransportState.Boarding)
        {
            if (isBoarding)
            {
                SetState(TransportState.Boarding);
            }
            else
            {
                SetState(TransportState.NotBoarding);
            }
        }
    }
}
