using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : LevelManagerBase
{
    [SerializeField] private Tutorial tutorial;
    private int tutorialState;

    override protected void Awake()
    {
        //SaveManager.instance.ResetData();
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

                    tutorialState = SaveManager.instance.GetProgression_TutorialState();
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
                }
                break;


            case LMState.Summary: // TODO: have summary show after all characters are seen
                {
                    StartCoroutine(WaitForSummary());
                }
                break;
        }
    }

    private IEnumerator WaitForSummary()
    {
        yield return new WaitForSeconds(0.5f);

        KioskBase.instance.SetState(KioskBase.KioskState.EndOfDay);

        foreach (Rail rail in rails)
        {
            rail.Depart();
        }

        // show prefab
        transparentOverlay.SetActive(true);
        summaryMenu.SetActive(true);

        dayStarted = false;
        PlotManager.instance.AddMail("letter", "crustyCoID", 4); 
        SaveManager.instance.SaveProgressionData(SaveManager.ProgressionType.newGame, false.ToString());
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
        tutorial.SetState((Tutorial.TutorialState)SaveManager.instance.GetProgression_TutorialState());
    }

    public void OnSkip()
    {
        PlotManager.instance.AddMail("letter", "crustyCoIdx", 1);
        SaveManager.instance.SaveProgressionData(SaveManager.ProgressionType.newGame, false.ToString());
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneManager.LoadScene("Home");
    }

    override public bool IsFirstCrabTutorial()
    {
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
