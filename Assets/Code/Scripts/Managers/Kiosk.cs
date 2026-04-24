using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Kiosk : KioskBase
{
    // state machine go brrrrr
    override public void SetState(KioskState newState)
    {
        KioskState prevState = kioskState;
        kioskState = newState;

        switch (kioskState)
        {
            case KioskState.NotOpenYet:
                {
                    DisableButtons();

                    crabSelector = GetComponent<CrabSelector>();
                }
                break;

            case KioskState.Empty:
                {
                    if (prevState != KioskState.NotOpenYet)
                    {
                        StartCoroutine(WaitBeforeSummon());
                    }
                    else
                    {
                        SummonCrab();
                    }
                }
                break;

            case KioskState.CrabPresent:
                {
                    EnableButtons();
                }
                break;

            case KioskState.CrabApproved:
                {
                    DisableButtons();
                    

                    LevelManager.instance.SetTrainsClickable(true);

                    if (!currentCrab.GetComponent<CrabController>().IsValid())
                    {
                        //wrong++;
                        Debug.Log("approved, incorrect");
                        PerformanceManager.instance.Incorrect();
                    }
                    else
                    {
                        Debug.Log("approved, correct");
                        PerformanceManager.instance.Correct();
                    }

                    DialogueManager.instance.ClearDialogue();
                }
                break;
 
            case KioskState.CrabRejected:
                {
                    DisableButtons();

                    if (currentCrab.GetComponent<CrabController>().IsValid())
                    {
                        Debug.Log("rejected, incorrect");
                        PerformanceManager.instance.Incorrect();
                        currentCrab.GetComponent<CrabController>().SetState(CrabController.CrabState.Emoting, "any and confused");
                    }
                    else
                    {
                        Debug.Log("rejected, correct");
                        PerformanceManager.instance.Correct();
                        currentCrab.GetComponent<CrabController>().SetState(CrabController.CrabState.Emoting, "any");
                    }

                    StartCoroutine(WaitForAnimEnd());
                }
                break;

            case KioskState.CrabLeaving:
                {
                    //crabCountGoal.IncrementGoal(crabsToday);

                    currentCrab.GetComponent<CrabController>().SetState(CrabController.CrabState.Leaving);

                    SetState(KioskState.Empty);
                }
                break;

            case KioskState.EndOfDay:
                {
                    currentCrab.GetComponent<CrabController>().SetState(CrabController.CrabState.Leaving);
                }
                break;
        }
    }

    override protected void SummonCrab()
    {
        WeatherType currWeather = WeatherManager.instance.GetCurrentWeather();
        var (chosen, chosenIdx) = crabSelector.ChooseCrab();

        bool validWeather = CheckWeather(chosen, currWeather);

        while (!validWeather)   // check if the chosen crab is cool with the current weather
        {
            (chosen, chosenIdx) = crabSelector.ChooseCrab();
            validWeather = CheckWeather(chosen, currWeather);
        }

        if (debugMode && charactersToForce.Count != 0)
        {
            chosen = charactersToForce[0];
            charactersToForce.RemoveAt(0);
        }

        crabSelector.AddToQueue(chosenIdx);
        currentCrab = Instantiate(chosen, crabParentObject.transform);
        //currentCrabIdx = chosenIdx;

        CrabController controller = currentCrab.GetComponent<CrabController>();
        //controller.SetCanvas(canvas.GetComponent<Canvas>());
        controller.SetCrabSelector(crabSelector);
        controller.SetClockAndKiosk(clock, this);
        controller.SetTicketAndIDParentObject(ticketParentObject);
        controller.SetState(CrabController.CrabState.Summoned);

        //Crabdex.instance.HasBeenDiscovered(controller.GetCrabInfo()); // crabdex!!!
        //isCurrentCrabCrustacean = Crabdex.instance.IsCrustacean(controller.GetCrabdexName());
    }

    private bool CheckWeather(GameObject crab, WeatherType currWeather)
    {
        CrabInfo.WeatherType[] chosenWeather = crab.GetComponent<CrabController>().GetFavoriteWeather();

        foreach (CrabInfo.WeatherType weatherType in chosenWeather)
        {
            if (weatherType == currWeather.weatherType)
            {
                return true;
            }
        }

        return false;
    }

    

}
