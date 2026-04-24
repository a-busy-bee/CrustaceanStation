using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManagerBase : MonoBehaviour
{
    public static LevelManagerBase instance { get; protected set; }

    [Header("Main")]
    [SerializeField] protected Clock clock;


    [Header("Trains")]
    [SerializeField] protected List<Rail> rails = new List<Rail>();  // all rails
    [SerializeField] protected GameObject trainsOverlay;


    [Header("Goals & Summary")]
    [SerializeField] protected GameObject pauseMenu;
    [SerializeField] protected GameObject summaryMenu;
    //private bool isRating = false;
    protected bool dayStarted = false;


    // UI background 
    [Header("Other")]
    [SerializeField] protected GameObject transparentOverlay;
    [SerializeField] protected AudioManager audioManager;
    protected bool isTutorial = false;

    // STATE MACHINE
    public enum LMState
    {
        Setup,              // init trains
        Goal,               // show goal
        Game,               // game loop 
        Paused,             // tell animations & weather & sound to keep playing
        Summary             // end of the day, show option to go to the shop or start a new day
    }
    public LMState lmState { get; protected set; }


    virtual protected void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        //goalRating.SetActive(false);
        //goalCrabCount.SetActive(false);
        summaryMenu.SetActive(false);
        isTutorial = false;
    }

    virtual protected void Start()
    {
        SetState(LMState.Setup);
    }

    // State machine go brrrrr
    virtual public void SetState(LMState newState)
    {
        LMState prevState = lmState;
        lmState = newState;

        switch (lmState)
        {
            case LMState.Setup:
                {
                    //Kiosk.instance.ShowDecor();
                    InitTrains();

                    SetState(LMState.Goal);
                }
                break;

            case LMState.Goal:
                {
                    //StartCoroutine(ShowGoalForTheDay());
                    //TODO: do level popup animation
                    SetState(LMState.Game);
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

                    foreach (Rail rail in rails)
                    {
                        rail.Depart();
                    }

                    // show prefab
                    transparentOverlay.SetActive(true);
                    summaryMenu.SetActive(true);
                    //summaryMenu.GetComponent<Summary>().SetRating(ratingGoalScript.GetRating());
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
        SceneManager.LoadScene("BaseArea");
    }

    public bool HasStarted()    // only used to prevent pause screen from activating during goals, TODO: consider removing
    {
        return dayStarted;
    }

    protected void InitTrains()
    {
        int id = 0;
        // goes through all of the trainIDs (lines) and generate first train
        foreach (Rail rail in rails)
        {
            rail.Summon();

            id++;
        }
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

    public void SetTrainsClickable(bool allowClick)
    {
        //TODO: account for shuttles and vans
        if (allowClick)
        {
            trainsOverlay.SetActive(false);
        }
        else
        {
            StartCoroutine(WaitThenTurnOnOverlay());
        }

        foreach (Rail rail in rails)
        {
            rail.SetClickable(allowClick);
        }
    }

    protected IEnumerator WaitThenTurnOnOverlay()
    {
        yield return new WaitForSeconds(1); // TODO: have it fade in instead of just turning on
        trainsOverlay.SetActive(true);
    }

    public bool GetIsTutorial()
    {
        return isTutorial;
    }

    virtual public bool IsFirstCrabTutorial()
    {
        return false;
    }

    virtual public void ProgressTutorial()
    {

    }

    virtual public Tutorial.TutorialState GetCurrTutorialState()
    {
        return Tutorial.TutorialState.setup;
    }

}
