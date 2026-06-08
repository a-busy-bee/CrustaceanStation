using UnityEngine;

public class KioskTutorial : KioskBase
{
    private int numCrabs = 0;
    [SerializeField] private TutorialManager tutorialManager;
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
                    if (numCrabs == 6) TutorialManager.instance.SetState(LevelManagerBase.LMState.Summary);
                    if (prevState != KioskState.NotOpenYet)
                    {
                        numCrabs++;
                        StartCoroutine(WaitBeforeSummon());
                    }
                    else
                    {
                        numCrabs++;
                        SummonCrab();
                    }
                }
                break;

            case KioskState.CrabPresent:
                {
                    tutorialManager.SetTutorialState();
                    if (TutorialManager.instance.IsFirstCrabTutorial()) return;
                    EnableButtons();

                }
                break;

            case KioskState.CrabApproved:
                {
                    DisableButtons();
                    bool trainExists = true;
                    /*if (LevelManager.instance.CheckTrainIDValidity(currentCrab.GetComponent<CrabController>().GetTrainID()))
                    {
                        trainExists = true;
                    }*/

                    LevelManager.instance.SetTrainsClickable(true);

                    if (!currentCrab.GetComponent<CrabController>().IsValid() || !trainExists || !isCurrentCrabCrustacean)
                    {
                        //wrong++;
                    }

                }
                break;

            case KioskState.CrabRejected:
                {
                    DisableButtons();

                    bool trainExists = true;
                    /*if (LevelManager.instance.CheckTrainIDValidity(currentCrab.GetComponent<CrabController>().GetTrainID()))
                    {
                        trainExists = true;
                    }*/

                    if (currentCrab.GetComponent<CrabController>().IsValid() && trainExists && isCurrentCrabCrustacean)
                    {
                        //wrong++;

                        currentCrab.GetComponent<CrabController>().SetState(CrabController.CrabState.Emoting, "any and confused");
                    }
                    else
                    {
                        currentCrab.GetComponent<CrabController>().SetState(CrabController.CrabState.Emoting, "any");
                    }

                    StartCoroutine(WaitForAnimEnd());
                }
                break;

            case KioskState.CrabLeaving:
                {
                    //crabsToday++;
                    //total++;

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
        var (chosen, chosenIdx) = crabSelector.ChooseCrabTutorial();

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

}
