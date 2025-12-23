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
    //private List<GameObject>[] allTrains;                                       // all scheduled trains
    private GameObject[] trackManager;  // null = no train on track, !null = train on track
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
                    kiosk.ShowDecor();
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
                        train.SetState(TrainController.TrainState.Departing);
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

    private GameObject GenerateTrain(int trainID)
    {
        int arrival = clock.GetCurrentTime(); // new start time
        int timeSpent = Random.Range(3, 6); // TODO: make longer trains stay in station longer?

        int departure = arrival + timeSpent;

        if (departure > Constants.CLOCK_END_TIME)                // latest departure time, so we're done here
        {
            departure = Constants.CLOCK_END_TIME;
        }

        GameObject train = Instantiate(trainPrefab, trainParent.transform);

        train.SetActive(true);
        train.GetComponent<TrainController>().SetKiosk(kiosk);
        train.GetComponent<TrainController>().InitTrain(arrival, departure, trainID);

        return train;
    }

    private void InitTrains()
    {
        // goes through all of the trainIDs (lines) and generate first train
        for (int i = 0; i < numActiveTracks; i++)
        {
            trackManager[i] = GenerateTrain(i + 1);
            StartCoroutine(WaitThenArriveTrain(trackManager[i]));
        }
    }

    public void CheckTrains(int currentTime)
    {
        // checks for new arrivals and departures
        for (int i = 0; i < trackManager.Length; i++)
        {
            GameObject train = trackManager[i];

            TrainController controller = train.GetComponent<TrainController>();
            if (controller != null)
            {
                if (currentTime == controller.GetDepartureTime() - 1)
                {
                    controller.AboutToDepartAlert();
                }

            }

        }
    }

    public void RemoveCurrentTrain(TrainController controller)
    {  
        currentTrains.Remove(controller);

        int id = controller.GetTrainLine();
        trackManager[id - 1] = GenerateTrain(id);
        StartCoroutine(WaitThenArriveTrain(trackManager[id - 1]));
    }

    public string GetRandomCurrentTrainID()
    {
        if (currentTrains.Count == 0)
        {
            return "none";
        }
        return currentTrains[Random.Range(0, currentTrains.Count)].GetID();
    }
    public int GetCartQuality()
    {
        return PlayerPrefs.GetInt("cartQuality");
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
                train.SetBoarding(true);
            }
            else
            {
                train.SetBoarding(false);
            }
            
        }
    }

    private void UpdateNumTracks(int tracks)
    {
        //allTrains = new List<GameObject>[tracks];
        trackManager = new GameObject[tracks];
        numActiveTracks = tracks;
    }

    private IEnumerator WaitThenArriveTrain(GameObject train)
    {
        yield return new WaitForSeconds(Random.Range(1, 3));

        TrainController controller = train.GetComponent<TrainController>();

        if (controller != null)
        {
            currentTrains.Add(controller);
            controller.SetState(TrainController.TrainState.Arriving);
        }
        
    }

}
