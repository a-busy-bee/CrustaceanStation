using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class TrainController : MonoBehaviour
{
    private Kiosk kiosk;

    // MOVEMENT
    [SerializeField] private RectTransform trainTransform;
    private Vector3 startingPosArrive; // where the train is before it moves into the station
    private Vector3 startingPosDepart; // also endPosArrive
    private Vector3 endPosDepart; // where the train goes to be completely offscreen
    private Vector3 currentVelocity;

    // IDS and INFO
    [SerializeField] private TextMeshProUGUI text; // text that displays train ID
    private Train trainInfo;
    private string[] trainIDLetters = { "A", "B", "C", "D", "E", "F" };
    private string trainID = "";
    private int coins = 0;

    // CARTS
    [Serializable]
    private struct CartType
    {
        public GameObject cartPrefab;
        public int weight;
    }
    [SerializeField] private GameObject cartParent;
    [SerializeField] private CartType[] cartTypes;
    private List<TrainSelection> trainSelections = new List<TrainSelection>();
    private float cartPosStartingPoint = 0f;


    // ALERT
    [SerializeField] private GameObject alertObject;
    private bool isAlerting;


    // STATE MACHINE
    public enum TrainState
    {
        Setup,          // initialize the train (carts, id, etc) 
        NotArrived,     // train hasn't moved into station yet
        Arriving,       // train is moving into the station
        NotBoarding,    // train is in station but not boarding (crab hasn't been approved)
        Boarding,       // train is in station and is boarding (crab has been approved)
        Departing,      // train is moving out of the station
        Departed        // train is offscreen
    }
    public TrainState trainState { get; private set; }
    bool boardAfterArrival = false;



    private void Start()
    {
        text.text = "";
        SetState(TrainState.Setup);
    }

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
                    text.text = trainID;
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
                    text.text = "";
                    isAlerting = false;

                    // give player coins
                    kiosk.GivePlayerCoins(coins);

                    foreach (TrainSelection ts in trainSelections)
                    {
                        ts.SetThisClickable(false);
                    }
                }
                break;
                
            case TrainState.Departed:
                {
                    LevelManager.instance.RemoveCurrentTrain(this);

                    Destroy(gameObject);
                }
                break;
        }
    }

    // GETTERS
    public float GetArrivalTime()
    {
        return trainInfo.arrivalTimeHour;
    }
    public float GetDepartureTime()
    {
        return trainInfo.departureTimeHour;
    }
    public void InitTrain(int newArrivalTime, int newDepartureTime, int newTrainID)
    {
        trainInfo = ScriptableObject.CreateInstance<Train>();
        trainInfo.arrivalTimeHour = newArrivalTime;
        trainInfo.departureTimeHour = newDepartureTime;
        trainInfo.trainID = newTrainID;

        trainID = trainIDLetters[UnityEngine.Random.Range(0, trainIDLetters.Length)] + trainInfo.trainID.ToString();

        float x = trainInfo.trainID * 200 - 52; // (1, 148), (2, 348), (3, 548), (4, 748)
        startingPosArrive = new Vector3(x, 830, 0);
        startingPosDepart = new Vector3(x, -285f, 0);
        endPosDepart = new Vector3(x, -2426, 0);

        // set up carts
        int numOfCarts = UnityEngine.Random.Range(1, 6);
        for (int i = 0; i < numOfCarts; i++)
        {
            // instantiate cart as child
            GameObject cart = Instantiate(cartTypes[GetRandomCart()].cartPrefab, cartParent.transform);
            TrainSelection selection = cart.GetComponent<TrainSelection>();

            // figure out position
            if (i == 0)
            {
                // set position based on cart type
                cart.transform.localPosition = new Vector3(0, 44, 0);

                // add to cartPosStartingPoint
                cartPosStartingPoint += 44 + 137;
            }
            else
            {
                // set position based on cartPosStartingPoint
                cart.transform.localPosition = new Vector3(0, cartPosStartingPoint, 0);

                // add to cartPosStartingPoint
                cartPosStartingPoint += 137;
            }

            // add trainSelection to list
            trainSelections.Add(selection);

            // set kiosk
            selection.SetKiosk(kiosk);
            selection.SetController(this);
        }

        SetState(TrainState.NotArrived);
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
    public bool IsStartTime0()
    {
        return trainInfo.arrivalTimeHour == 0;
    }
    public void SetArrivalTime()
    {
        trainInfo.arrivalTimeHour = 0;
    }
    public void SetKiosk(Kiosk newKiosk)
    {
        kiosk = newKiosk;
    }
    public int GetTrainLine()
    {
        return trainInfo.trainID;
    }
    public string GetID()
    {
        return trainID;
    }
    private void SetThisClickable(bool isClickable)
    {
        foreach (TrainSelection ts in trainSelections)
        {
            ts.SetThisClickable(isClickable);
        }
    }
    public void AboutToDepartAlert()
    {
        if (!isAlerting)
        {
            isAlerting = true;
            StartCoroutine(Alert());
        }
    }

    public void CheckIfFull()
    {
        if (IsTrainFull())
        {
            AboutToDepartAlert();
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
                        if (kiosk.kioskState == Kiosk.KioskState.CrabApproved)
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

    private IEnumerator Alert()
    {
        if (isAlerting)
        {
            for (int i = 0; i < 5; i++)
            {
                alertObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                alertObject.SetActive(false);
                yield return new WaitForSeconds(0.5f);
            }
        }

        SetState(TrainState.Departing);
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

    private int GetRandomCart() // to get a random cart type when instantiating the train
    {
        // TODO : Fix cart quality upgrade
        int maxCartQuality = LevelManager.instance.GetCartQuality() + 1;
        int totalWeight = 0;
        //for (CartType type in cartTypes)
        for (int i = 0; i < 3; i++)
        {
            totalWeight += cartTypes[i].weight;
        }

        int rand = UnityEngine.Random.Range(0, totalWeight);

        for (int i = 0; i < cartTypes.Length; i++)
        {
            if (rand < cartTypes[i].weight)
            {
                return i;
            }

            rand -= cartTypes[i].weight;
        }

        // failsafe
        return 0;
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
