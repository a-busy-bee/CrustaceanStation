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


    [Header("Trains")]
    [SerializeField] private List<Rail> rails = new List<Rail>();  // all rails
    [SerializeField] private GameObject cartPopupStandard;
    [SerializeField] private GameObject cartPopupEconomy; 


    [Header("Goals & Summary")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject goalRating;
    [SerializeField] private RatingGoal ratingGoalScript;
    [SerializeField] private GameObject goalCrabCount;
    [SerializeField] private CrabCountGoal crabCountGoalScript;
    [SerializeField] private GameObject summaryMenu;
    private bool isRating = false;
    private bool dayStarted = false;


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

                    Kiosk.instance.ShowDecor();
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


            case LMState.Summary: // TODO: have summary show after all characters are seen
                {
                    Kiosk.instance.SetState(Kiosk.KioskState.EndOfDay);

                    // show prefab
                    transparentOverlay.SetActive(true);
                    summaryMenu.SetActive(true);
                    summaryMenu.GetComponent<Summary>().SetRating(ratingGoalScript.GetRating());
                    summaryMenu.GetComponent<Summary>().SetCrabsProcessed(Kiosk.instance.GetTotalCrabs());

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

    private void InitTrains()
    {
        int id = 0;
        // goes through all of the trainIDs (lines) and generate first train
        foreach (Rail rail in rails)
        {
            rail.SetRailNumber(id);
            rail.SummonTrain();

            id++;
        }
    }

    public int GetNumberOfRails()
    {
        return rails.Count;
    }

    public void RemoveCurrentTrain(TrainController controller)
    {
        //currentTrains.Remove(controller);

        //int id = controller.GetTrainLine();
        //trackManager[id - 1] = GenerateTrain(id);
        //StartCoroutine(WaitThenArriveTrain(trackManager[id - 1]));
    }

    public string GetRandomCurrentTrainID()
    {
        /*if (currentTrains.Count == 0)
        {
            return "none";
        }
        return currentTrains[Random.Range(0, currentTrains.Count)].GetID();*/

        return "temp";
    }

    public GameObject GetStandardCartPopup()
    {
        return cartPopupStandard;
    }

    public GameObject GetEconomyCartPopup()
    {
        return cartPopupEconomy;
    }

    public int GetCartQuality()
    {
        return PlayerPrefs.GetInt("cartQuality");
    }
    public Cart.Type GetRandomCurrentCartType()
    {
        int totalWeight = 30; // economy = 20, standard = 10
        int rand = Random.Range(0, totalWeight);

        if (rand < 10)
        {
            return Cart.Type.Standard;
        }
        else
        {
            return Cart.Type.Economy;
        }
    }

    public bool CheckTrainIDValidity(string id)
    {
        foreach (Rail rail in rails)
        {
            if (rail.CheckTrainValidity(id)) return true;
        }

        return false;
    }

    public void SetTrainsClickable(bool allowClick)
    {
        foreach (Rail rail in rails)
        {
            rail.SetTrainClickable(allowClick);
        }
    }

}
