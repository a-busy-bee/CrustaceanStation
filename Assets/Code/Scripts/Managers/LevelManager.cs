using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    [Header("Main")]
    [SerializeField] private Clock clock;
    [SerializeField] private Kiosk kiosk;


    [Header("Trains")]
    [SerializeField] private GameObject trainPrefab;    // train head
    [SerializeField] private GameObject trainParent;
    private List<TrainController> currentTrains = new List<TrainController>();  // trains currently at the station
    private List<GameObject>[] allTrains;                                       // all scheduled trains
    private int numActiveTracks = 1;


    [Header("Goals & Summary")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject goalRating;
    [SerializeField] private RatingGoal ratingGoalScript;
    [SerializeField] private GameObject goalCrabCount;
    [SerializeField] private CrabCountGoal crabCountGoalScript;
    [SerializeField] private GameObject summaryMenu;
    private bool isRating = false;
    private bool dayStarted = false;


    [Header("Track Upgrade")]
    [SerializeField] private GameObject[] tracks;


    [Header("Decor")]
    [SerializeField] private DecorItems[] items;
    [SerializeField] private DecorItems[] topDecor;
    [SerializeField] private GameObject leftSlot;
    [SerializeField] private GameObject rightSlot;
    [SerializeField] private GameObject topSlot;


    // UI background 
    [Header("Other")]
    [SerializeField] private GameObject transparentOverlay;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Tutorial tutorial;


    // STATE MACHINE
    public enum LMState
    {
        Setup,              // init trains
        Goal,               // show goal
        Game,               // game loop 
        Paused,             // tell animations & weather & sound to keep playing
        Summary             // end of the day, show option to go to the shop or start a new day
    }
    public LMState lmState { get; private set; }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        goalRating.SetActive(false);
        goalCrabCount.SetActive(false);
        summaryMenu.SetActive(false);
    }

    private void Start()
    {
        SetState(LMState.Setup);   
    }

    // State machine go brrrrr
    public void SetState(LMState newState)
    {
        LMState prevState = lmState;
        lmState = newState;

        switch (lmState)
        {
            case LMState.Setup:
                {
                    if (PlayerPrefs.GetInt("newGame") == 1)
                    {
                        PlayerPrefs.SetInt("newGame", -1); // set so that it's not replayed unless progress is reset
                        tutorial.gameObject.SetActive(true);
                        tutorial.Play(true);
                    }

                    ActivateUpgrades();
                    ShowDecor();
                    InitTrains();

                    SetState(LMState.Goal);
                }
                break;

            case LMState.Goal:
                {
                    StartCoroutine(ShowGoalForTheDay());
                }
                break;

            case LMState.Game:
                {
                    if (prevState == LMState.Paused)
                    {
                        // hide overlay background
                        transparentOverlay.SetActive(false);

                        // start clock & crabs & trains
                        Time.timeScale = 1f;
                    }
                    else if (prevState == LMState.Goal)
                    {
                        // start the clock
                        clock.BeginDay();

                        dayStarted = true;
                    }
                }
                break;


            case LMState.Paused:
                {
                    // show overlay background
                    transparentOverlay.SetActive(true);

                    // stop clock & crabs & trains
                    Time.timeScale = 0f;
                    audioManager.Play();
                }
                break;


            case LMState.Summary:
                {
                    kiosk.SetState(Kiosk.KioskState.EndOfDay);

                    foreach (TrainController train in currentTrains)
                    {
                        train.SetState(TrainController.TrainState.Arriving);
                    }

                    // show prefab
                    transparentOverlay.SetActive(true);
                    summaryMenu.SetActive(true);
                    summaryMenu.GetComponent<Summary>().SetRating(ratingGoalScript.GetRating());
                    summaryMenu.GetComponent<Summary>().SetCrabsProcessed(kiosk.GetTotalCrabs());

                    dayStarted = false;
                }
                break;
        }
    }

    public void OnShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void OnMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void OnNewDay()
    {
        SceneManager.LoadScene("Temp");
    }


    private IEnumerator ShowGoalForTheDay()
    {
        transparentOverlay.SetActive(true);

        // show prefab
        if (Random.Range(0, 10) > 4)
        {
            isRating = true;
            goalRating.SetActive(true);
            ratingGoalScript.SetGoalActive();
        }
        else
        {
            goalCrabCount.SetActive(true);
            crabCountGoalScript.SetGoalActive();
        }

        yield return new WaitForSeconds(3.5f);

        // hide prefab
        if (isRating)
        {
            goalRating.SetActive(false);
        }
        else
        {
            goalCrabCount.SetActive(false);
        }

        transparentOverlay.SetActive(false);

        SetState(LMState.Game);
    }

    public bool HasStarted()    // only used to prevent pause screen from activating during goals, TODO: consider removing
    {
        return dayStarted;
    }

    private void ActivateUpgrades()
    {
        int trackCount = PlayerPrefs.GetInt("numTracks"); // starts at 0, so need to add one when updating track num in clock

        // activate tracks
        for (int i = 0; i < trackCount; i++)
        {
            tracks[i].SetActive(true);
        }

        // update train controllers
        UpdateNumTracks(trackCount + 1);
    }

    private void InitTrains()
    {
        bool startingTrain = false;    // is there a train that arrives before the first crab does?

        // goes through all of the trainIDs (lines) and fills it with disjoint trains
        for (int i = 0; i < numActiveTracks; i++)
        {
            allTrains[i] = AddTrainsToLine(i + 1);
            if (allTrains[i][0].GetComponent<TrainController>().IsStartTime0())
            {
                startingTrain = true;
            }
        }

        if (!startingTrain)
        {
            allTrains[Random.Range(0, numActiveTracks)][0].GetComponent<TrainController>().SetArrivalTime();
        }
    }

    private List<GameObject> AddTrainsToLine(int trainID)
    {
        List<GameObject> trainsInLine = new List<GameObject>();

        int arrival = Random.Range(0, 2);
        int timeSpent = Random.Range(1, 3);         // time spent at station

        if (PlayerPrefs.GetInt("numTracks") == 0)
        {
            timeSpent = 1;
        }

        int departure = arrival + timeSpent;

        GameObject train = Instantiate(trainPrefab, trainParent.transform);
        train.SetActive(true);
        train.GetComponent<TrainController>().SetKiosk(kiosk);
        train.GetComponent<TrainController>().InitTrain(arrival, departure, trainID);
        trainsInLine.Add(train);

        int prevDeparture = departure;
        bool doneWithThisID = false;

        while (!doneWithThisID)                     // while there is still time for more trains
        {
            arrival = prevDeparture + 1; // new start time
            timeSpent = Random.Range(1, 3);

            if (PlayerPrefs.GetInt("numTracks") == 0)
            {
                timeSpent = 1;
            }

            departure = arrival + timeSpent;

            if (departure > Constants.CLOCK_END_TIME)                // latest departure time, so we're done here
            {
                departure = Constants.CLOCK_END_TIME;
                doneWithThisID = true;
            }
            else if (departure >= (Constants.CLOCK_END_TIME - 3))    // not enough time for another train, so we're done here
            {
                doneWithThisID = true;
            }

            train = Instantiate(trainPrefab, trainParent.transform);
            train.SetActive(true);
            
            train.GetComponent<TrainController>().SetKiosk(kiosk);
            train.GetComponent<TrainController>().InitTrain(arrival, departure, trainID);
            
            trainsInLine.Add(train);

            prevDeparture = departure;
        }

        return trainsInLine;
    }

    public void CheckTrains(int currentTime)
    {
        List<int> removeIndex = new List<int>();

        // checks for new arrivals and departures
        foreach (List<GameObject> trainID in allTrains)
        {
            for (int i = 0; i < trainID.Count; i++)
            {
                GameObject train = trainID[i];
                if (train == null) { continue; } // all trains have departed on this line, so we can skip it

                TrainController controller = train.GetComponent<TrainController>();
                if (controller != null)
                {
                    if (currentTime == controller.GetArrivalTime())
                    {
                        currentTrains.Add(controller);
                        controller.SetState(TrainController.TrainState.Arriving);

                        if (kiosk.kioskState == Kiosk.KioskState.CrabApproved)
                        {
                            controller.SetState(TrainController.TrainState.Boarding);
                        }
                    }
                    else if (currentTime == controller.GetDepartureTime())
                    {
                        currentTrains.Remove(controller);
                        removeIndex.Add(i);
                        controller.SetState(TrainController.TrainState.Departing);
                    }
                    else if (currentTime == controller.GetDepartureTime() - 1)
                    {
                        controller.AboutToDepartAlert();
                    }

                }

            }

            foreach (int index in removeIndex)
            {
                trainID.RemoveAt(index);
            }
            removeIndex.Clear();
        }
    }

    public string GetRandomCurrentTrainID()
    {
        if (currentTrains.Count == 0)
        {
            return "none";
        }
        return currentTrains[Random.Range(0, currentTrains.Count)].GetID();
    }

    public Cart.Type GetRandomCurrentCartType()
    {
        if (currentTrains.Count == 0)
        {
            return Cart.Type.Economy;
        }
        return currentTrains[Random.Range(0, currentTrains.Count)].GetRandomCartType();
    }

    public bool CheckTrainIDValidity(string id)
    {
        foreach (TrainController train in currentTrains)
        {
            if (id == train.GetID() && !train.IsTrainFull())
            {
                return true;
            }
        }
        return false;
    }

    public void SetTrainsClickable(bool allowClick)
    {
        foreach (TrainController train in currentTrains)
        {
            if (allowClick)
            {
                train.SetState(TrainController.TrainState.Boarding);
            }
            else
            {
                train.SetState(TrainController.TrainState.NotBoarding);
            }
            
        }
    }

    public void UpdateNumTracks(int tracks)
    {
        allTrains = new List<GameObject>[tracks];
        numActiveTracks = tracks;
    }

    private void ShowDecor()
    {
        int top = PlayerPrefs.GetInt("decor_top");
        int left = PlayerPrefs.GetInt("decor_left");
        int right = PlayerPrefs.GetInt("decor_right");

        topSlot.SetActive(false);
        leftSlot.SetActive(false);
        rightSlot.SetActive(false);

        if (top > 1)
        {
            topSlot.SetActive(true);
            topSlot.GetComponent<Image>().sprite = topDecor[top].sprite;
        }

        if (left > 1)
        {
            leftSlot.SetActive(true);
            leftSlot.GetComponent<Image>().sprite = items[left].sprite;
        }

        if (right > 1)
        {
            rightSlot.SetActive(true);
            rightSlot.GetComponent<Image>().sprite = items[right].sprite;
        }
    }

    public int GetCartQuality()
    {
        return PlayerPrefs.GetInt("cartQuality");
    }

}
