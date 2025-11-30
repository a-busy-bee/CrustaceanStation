using System;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.Collections.Generic;
using System.Data.Common;
using JetBrains.Annotations;

public class TrainController : MonoBehaviour
{
    private bool isMoving = false;
    private bool isDeparting = false;
    private bool isArriving = false;
    [SerializeField] private RectTransform trainTransform;

    private Vector3 startingPosArrive; // where the train is before it moves into the station
    private Vector3 startingPosDepart; // also endPosArrive
    private Vector3 endPosDepart; // where the train goes to be completely offscreen
    private Vector3 currentVelocity;

    // IDS and INFO
    private Train trainInfo;

    [SerializeField] private TextMeshProUGUI text;
    private string[] trainIDLetters = { "A", "B", "C", "D", "E", "F" };
    private string trainID = "";

    private int coins = 0;

    // SELECTION
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
    private Kiosk kiosk;

    // ALERT
    [SerializeField] private GameObject alertObject;
    private bool isAlerting;

    // GETTERS
    public float GetArrivalTime()
    {
        return trainInfo.arrivalTimeHour;
    }

    public float GetDepartureTime()
    {
        return trainInfo.departureTimeHour;
    }

    public void SetTrainInfo(int newArrivalTime, int newDepartureTime, int newTrainID)
    {
        trainInfo = ScriptableObject.CreateInstance<Train>();
        trainInfo.arrivalTimeHour = newArrivalTime;
        trainInfo.departureTimeHour = newDepartureTime;
        trainInfo.trainID = newTrainID;
        Init();
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

    public string GetID()
    {
        return trainID;
    }

    public void SetThisClickable(bool isClickable)
    {
        foreach (TrainSelection ts in trainSelections)
        {
            ts.SetThisClickable(isClickable);
        }
    }

    public void AboutToDepartAlert()
    {
        isAlerting = true;
        StartCoroutine(Alert());
    }
    public void Init()
    {
        float x = 0;
        if (trainInfo.trainID == 1)
        {
            x = 148;
            //text.rectTransform.anchoredPosition = new Vector2(223, -23);
            //alertObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(163, 28);

        }
        else if (trainInfo.trainID == 2)
        {
            x = 348;
            //text.rectTransform.anchoredPosition = new Vector2(440, -23);
            //alertObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(379, 28);

        }
        else if (trainInfo.trainID == 3)
        {
            x = 548;
            //text.rectTransform.anchoredPosition = new Vector2(657, -23);
            //alertObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(580, 28);
        }
        else if (trainInfo.trainID == 4)
        {
            x = 748;
            //text.rectTransform.anchoredPosition = new Vector2(874, -23);
            //alertObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(781, 28); 
        }

        startingPosArrive = new Vector3(x, 830, 0);
        startingPosDepart = new Vector3(x, -285f, 0);
        endPosDepart = new Vector3(x, -2426, 0);
        trainTransform.anchoredPosition = startingPosArrive;

        trainID = trainIDLetters[UnityEngine.Random.Range(0, trainIDLetters.Length)] + trainInfo.trainID.ToString();
    }

    private void Awake()
    {
        text.text = "";
    }

    private void Start()
    {
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
                float startingPos = selection.getStartingPos();
                //cart.transform.localPosition = new Vector3(0, startingPos, 0);
                cart.transform.localPosition = new Vector3(0, 44, 0);

                // add to cartPosStartingPoint
                //cartPosStartingPoint += startingPos + selection.getHeight();
                cartPosStartingPoint += 44 + 137;
            }
            else
            {
                // set position based on cartPosStartingPoint
                cart.transform.localPosition = new Vector3(0, cartPosStartingPoint, 0);

                // add to cartPosStartingPoint
                //cartPosStartingPoint += selection.getHeight();
                cartPosStartingPoint += 137;
            }

            // add trainSelection to list
            trainSelections.Add(selection);

            // set kiosk
            selection.SetKiosk(kiosk);
            selection.SetController(this);
        }
    }

    public void arriveTrain()
    {
        isMoving = true;
        isArriving = true;

        text.text = trainID;
    }

    public void departTrain()
    {
        text.text = "";

        // give player coins
        kiosk.GivePlayerCoins(coins);

        // move train offscreen (down)
        isMoving = true;
        isDeparting = true;

        foreach (TrainSelection ts in trainSelections)
        {
            ts.SetThisClickable(false);
        }
    }

    public void AddToCrabsOnTrain(int ticketCost)
    {
        coins += ticketCost;
    }

    private void Update()
    {
        if (isMoving)
        {

            if (isArriving)
            {
                // move train down from offscreen
                trainTransform.anchoredPosition = Vector3.SmoothDamp(trainTransform.anchoredPosition, startingPosDepart, ref currentVelocity, 0.5f);

                if (Vector2.Distance(trainTransform.anchoredPosition, startingPosDepart) < 0.1f)
                {
                    isArriving = false;
                    isMoving = false;
                }
            }
            else if (isDeparting)
            {
                isAlerting = false;

                // move train down from onscreen
                trainTransform.anchoredPosition = Vector3.SmoothDamp(trainTransform.anchoredPosition, endPosDepart, ref currentVelocity, 0.5f);

                if (Vector2.Distance(trainTransform.anchoredPosition, endPosDepart) < 0.1f)
                {
                    isDeparting = false;
                    isMoving = false;

                    Destroy(this);
                }

            }
        }
    }

    private IEnumerator Alert()
    {
        while (isAlerting)
        {
            alertObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            alertObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
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
