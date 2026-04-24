using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : LevelManagerBase
{
    override protected void Start()
    {
        SetState(LMState.Setup);
        PlayBGM(); // idk if this is clean to go here sry
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
                    InitTrains();

                    SetState(LMState.Goal);
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


                    // there was something about playing an audio that was here -lucy

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

                    string type = "generic";
                    if (Random.Range(1, 4) == 2) type = "plot";
                    PlotManager.instance.AddMail("feedbackForm", type, 1);
                }
                break;
        }
    }

}
