using UnityEngine;

public class LevelManager : LevelManagerBase
{
    [SerializeField] private LevelPopup levelPopup;

    override protected void Start()
    {
        SaveManager.instance.SaveProgressionData(SaveManager.ProgressionType.currDay, "5");
        SetState(LMState.Setup);
    }

    // State machine go brrrrr
    override public void SetState(LMState newState)
    {
        LMState prevState = lmState;
        lmState = newState;

        switch (lmState)
        {
            case LMState.Setup:
                {
                    levelPopup.BeginPopup();
                }
                break;

            case LMState.Goal:
                {
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

                }
                break;


            case LMState.Summary: // TODO: have summary show after all characters are seen
                {

                    Kiosk.instance.SetState(Kiosk.KioskState.EndOfDay);
                    SaveManager.instance.SetProgression_IncrementCurrDay();

                    foreach (Rail rail in rails)
                    {
                        rail.Depart();
                    }

                    // show prefab
                    transparentOverlay.SetActive(true);
                    summaryMenu.SetActive(true);
                    PerformanceManager.instance.InitSummary();
                    //summaryMenu.GetComponent<Summary>().SetRating(ratingGoalScript.GetRating());
                    //summaryMenu.GetComponent<Summary>().SetCrabsProcessed(Kiosk.instance.GetTotalCrabs());

                    dayStarted = false;
                    AddMail();
                }
                break;
        }
    }

    public void AddMail()
    {
        //     string type = "generic";
        //     if (Random.Range(1, 4) == 2) type = "plot";
        //     PlotManager.instance.AddMail("feedbackForm", type, 1);

        Constants constants = Constants.instance;
        int currDay = SaveManager.instance.GetProgression_CurrDay();


        // LETTERS
        if (constants.LETTER_dayToIdxCrustyCo.ContainsKey(currDay))
        {
            PlotManager.instance.AddMail("letter", "crustyCoDay", currDay);
        }
        else if (constants.LETTER_dayToIdxCrustyCoEndings.ContainsKey(currDay))
        {
            if (PlotManager.instance.IsGoodEnding())
            {
                PlotManager.instance.AddMail("letter", "crustyCoDay_GoodEnding", currDay);
            }
            else
            {
                PlotManager.instance.AddMail("letter", "crustyCoDay_BadEnding", currDay);
            }
        }

        if (currDay == 3)
        {
            if (SaveManager.instance.GetProgression_EatenBeforeDay3())
            {
                PlotManager.instance.AddMail("letter", "crustyCoID", 3);
            }
            else
            {
                PlotManager.instance.AddMail("letter", "crustyCoID", 2);
            }
        }

        if (constants.LETTER_dayToIdxFamily.ContainsKey(currDay))
        {
            PlotManager.instance.AddMail("letter", "family", currDay);
        }

        if (constants.LETTER_dayToIdxMailkeeper.ContainsKey(currDay))
        {
            PlotManager.instance.AddMail("letter", "mailkeeper", currDay);
        }


        //FEEDBACK
        PlotManager.instance.AddMail("feedbackForm", "generic", 1);
        
        CrabInfo.SpecialCharacter[] specialsToday = constants.SELECTOR_dayToCharacter[currDay];
        for (int i = 0; i < specialsToday.Length; i++)
        {
            if (Constants.instance.FEEDBACK_characterToDayToIdx[specialsToday[i]].ContainsKey(currDay)) 
                PlotManager.instance.AddMail("feedbackForm", specialsToday[i].ToString(), currDay);
        }
    }

}
