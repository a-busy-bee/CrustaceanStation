using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class TrainController : MonoBehaviour
{
    private Rail rail;

    // MOVEMENT
    [SerializeField] private RectTransform trainTransform;
    private Vector3 startingPosArrive; // where the train is before it moves into the station
    private Vector3 startingPosDepart; // also endPosArrive
    private Vector3 endPosDepart; // where the train goes to be completely offscreen
    private Vector3 currentVelocity;

    // IDS and INFO
    private Rail.RailDirection railDirection;
    private int railNumber;
    private int coins = 0;

    // CARTS
    [SerializeField] private List<TrainSelection> trainSelections = new List<TrainSelection>();

    // ALERT
    [SerializeField] private GameObject alertObject;


    // STATE MACHINE
    public enum TrainState
    {
        Setup,          // initialize the train  
        NotArrived,     // train hasn't moved into station yet
        Arriving,       // train is moving into the station
        NotBoarding,    // train is in station but not boarding (crab hasn't been approved)
        Boarding,       // train is in station and is boarding (crab has been approved)
        Departing,      // train is moving out of the station
        Departed        // train is offscreen
    }
    public TrainState trainState { get; private set; }

    [SerializeField] private List<TrainPosition> trainPositionsBasedOnDirection;


    // State machine go brrrrr
    public void SetState(TrainState newState)
    {
        TrainState prevState = trainState;
        trainState = newState;

        switch (trainState)
        {
            case TrainState.Setup:
                {
                    // do nothing, logic already in InitTrain
                }
                break;

            case TrainState.NotArrived:
                {
                    // set starting position
                    trainTransform.anchoredPosition = startingPosArrive;
                }
                break;

            case TrainState.Arriving:
                {
                    // nothing to do here, all logic in Update()
                }
                break;

            case TrainState.NotBoarding:
                {
                    SetThisClickable(false);
                }
                break;

            case TrainState.Boarding:
                {
                    SetThisClickable(true);
                }
                break;

            case TrainState.Departing:
                {
                    // give player coins
                    coins = 0;

                    foreach (TrainSelection ts in trainSelections)
                    {
                        ts.SetThisClickable(false);
                        coins += ts.DepartTrain();
                    }
                    Kiosk.instance.GivePlayerCoins(coins);
                }
                break;
                
            case TrainState.Departed:
                {
                    //LevelManager.instance.RemoveCurrentTrain(this);
                    rail.SummonTrain();

                    Destroy(gameObject);
                }
                break;
        }
    }

    public void InitTrain(Rail.RailDirection newTrainDirection, Rail newRail, GameObject newStandardCart, GameObject newEconomyCart, int newRailNumber)
    {
        railDirection = newTrainDirection;
        rail = newRail;
        railNumber = newRailNumber;

        TrainPosition trainPosition = trainPositionsBasedOnDirection[(int)railDirection];

        startingPosArrive = new Vector3(0, trainPosition.startingPosArrive, 0);
        startingPosDepart = new Vector3(0, trainPosition.startingPosDepart, 0);
        endPosDepart = new Vector3(0, trainPosition.endPosDepart, 0);

        GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, trainPosition.rotation);

        // set up carts
        foreach (TrainSelection selection in trainSelections)
        {
            selection.SetController(this);
            selection.SetRailNumber(railNumber);
        }

        trainTransform.anchoredPosition = startingPosArrive;
        SetState(TrainState.Arriving);
    }
    public bool IsTrainFull()
    {
        foreach (TrainSelection cart in trainSelections)
        {
            if (!cart.isItFull())
            {
                return false;
            }
        }

        return true;
    }
    public string GetID()
    {
        return "temp";
    }
    private void SetThisClickable(bool isClickable)
    {
        foreach (TrainSelection ts in trainSelections)
        {
            ts.SetThisClickable(isClickable);
        }
    }

    public void CheckIfFull()
    {
        if (IsTrainFull())
        {
            rail.FlashFullAlert();
        }
    }
    public void AddToCrabsOnTrain(int ticketCost)
    {
        coins += ticketCost;
    }

    private void Update()
    {
        switch (trainState)
        {
            case TrainState.Arriving:
                {
                    // move train down from offscreen
                    trainTransform.anchoredPosition = Vector3.SmoothDamp(trainTransform.anchoredPosition, startingPosDepart, ref currentVelocity, 0.5f);

                    if (Vector2.Distance(trainTransform.anchoredPosition, startingPosDepart) < 0.1f)
                    {
                        if (Kiosk.instance.kioskState == Kiosk.KioskState.CrabApproved)
                        {
                            SetState(TrainState.Boarding);
                        }
                        else
                        {
                            SetState(TrainState.NotBoarding);
                        }
                    }
                }
                break;

            case TrainState.Departing:
                {
                    // move train down from onscreen
                    trainTransform.anchoredPosition = Vector3.SmoothDamp(trainTransform.anchoredPosition, endPosDepart, ref currentVelocity, 0.5f);

                    if (Vector2.Distance(trainTransform.anchoredPosition, endPosDepart) < 100f)
                    {
                        SetState(TrainState.Departed);
                    }
                }
                break;
        }
    }

    public void SetBoarding(bool isBoarding)
    {
        if (trainState == TrainState.NotBoarding || trainState == TrainState.Boarding)
        {
            if (isBoarding)
            {
                SetState(TrainState.Boarding);
            }
            else
            {
                SetState(TrainState.NotBoarding);
            }
        }
    }

    public Cart.Type GetRandomCartType() // get a random cart type when instantiating the ticket
    {
        List<TrainSelection> notFilledCarts = new List<TrainSelection>();
        foreach (TrainSelection train in trainSelections)
        {
            if (!train.isItFull())
            {
                notFilledCarts.Add(train);
            }
        }

        if (notFilledCarts.Count == 0)
        {
            return trainSelections[UnityEngine.Random.Range(0, trainSelections.Count)].GetCartType();
        }
        else
        {
            return notFilledCarts[UnityEngine.Random.Range(0, notFilledCarts.Count)].GetCartType();
        }
    }
}
