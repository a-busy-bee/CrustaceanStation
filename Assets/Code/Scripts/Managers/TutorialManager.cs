using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : LevelManagerBase
{
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private bool debug;
    private int tutorialState;

    override protected void Awake()
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
        isTutorial = true;

        if (debug) PlayerPrefs.SetInt("tutorialState", 0);
    }
    override public void SetState(LMState newState)
    {
        LMState prevState = lmState;
        lmState = newState;

        switch (lmState)
        {
            case LMState.Setup:
                {
                    //Kiosk.instance.ShowDecor();
                    InitTrains();

                    tutorialState = PlayerPrefs.GetInt("tutorialState");
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
                        if (tutorialState > 6)
                        {
                            // start the clock
                            clock.gameObject.SetActive(true);
                            //clock.BeginDay();
                            StartCoroutine(BeginClock());
                        }
                        else
                        {
                            StartCoroutine(WaitThenSummonCrabs());
                        }

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
                    KioskBase.instance.SetState(KioskBase.KioskState.EndOfDay);

                    foreach (Rail rail in rails)
                    {
                        rail.Depart();
                    }

                    // show prefab
                    transparentOverlay.SetActive(true);
                    summaryMenu.SetActive(true);

                    dayStarted = false;
                    PlayerPrefs.SetInt("newGame", -1);
                }
                break;
        }
    }

    private IEnumerator WaitThenSummonCrabs()
    {
        yield return new WaitForSeconds(0.5f);
        KioskBase.instance.SetState(Kiosk.KioskState.Empty);
    }

    private IEnumerator BeginClock()
    {
        yield return null;
        clock.BeginDay();
    }

    public void SetTutorialState()
    {
        tutorial.SetState((Tutorial.TutorialState)PlayerPrefs.GetInt("tutorialState"));
    }

    public void OnSkip()
    {
        PlayerPrefs.SetInt("newGame", -1);
        SceneManager.LoadScene("Home");
    }

    override public bool IsFirstCrabTutorial()
    {
        Debug.Log("is first crab " + tutorial.GetIsFirstCrab());
        return tutorial.GetIsFirstCrab();
    }

    override public void ProgressTutorial()
    {
        tutorial.Continue();
    }

    override public Tutorial.TutorialState GetCurrTutorialState()
    {
        return tutorial.GetTutorialState();
    }


}
