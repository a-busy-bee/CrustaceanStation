using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Kiosk : MonoBehaviour
{
    [SerializeField] private Clock clock;
    //private bool isOpen = false;
    

    // CURRENT CRAB
    private GameObject currentCrab;
    private bool isCurrentCrabCrustacean = false;
    private CrabSelector crabSelector;
    private int crabSpeed = 5;
    

    [Header("Kiosk Objects")]
    [SerializeField] private GameObject crabParentObject; // in scene hierarchy: canvas > crabs
    [SerializeField] private GameObject ticketParentObject;
    [SerializeField] private TextMeshProUGUI coinCountText;

    
    [Header("Goals")]
    [SerializeField] private RatingGoal ratingGoal;
    [SerializeField] private CrabCountGoal crabCountGoal;
    private int crabsToday = 0;
    private float wrong = 0f;
    private int total = 0;


    [Header("Buttons")]
    [SerializeField] private Button approveButton;
    [SerializeField] private Button rejectButton;
    [SerializeField] private Button waitButton;
    //private bool isCrabBeingProcessed = false;


    /*// WAITING
    [System.Serializable]
    public struct WaitingCrab
    {
        public GameObject crab;
        public int id;

        public WaitingCrab(GameObject newCrab, int newID)
        {
            crab = newCrab;
            id = newID;
        }
    }

    [Header("Wait")]
    public List<WaitingCrab> waitingCrabs = new List<WaitingCrab>();
    private bool isWaiting;
    //private int currentCrabIdx; // for waiting crabs, ignore for now*/

    public enum KioskState
    {
        NotOpenYet,         // before the start of the day
        Empty,              // a crab hasn't walked up to the kiosk yet
        CrabPresent,        // crab has presented its ID & ticket for review
        CrabApproved,       // crab has been approved & is selecting train
        CrabWaiting,        // crab has been told to wait
        CrabRejected,       // crab has been rejected
        CrabLeaving,        // crab is leaving 
        EndOfDay            // day is over, close kiosk
    }
    public KioskState kioskState { get; private set; }

    private void Awake()
    {
        SetState(KioskState.NotOpenYet);
    }

    // state machine go brrrrr
    public void SetState(KioskState newState)
    {
        KioskState prevState = kioskState;
        kioskState = newState;

        switch (kioskState)
        {
            case KioskState.NotOpenYet:
                {
                    DisableButtons();

                    crabSelector = GetComponent<CrabSelector>();
                    coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
                    SetCrabSpeedUpgrade();
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
                    bool trainExists = false;
                    if (LevelManager.instance.CheckTrainIDValidity(currentCrab.GetComponent<CrabController>().GetTrainID()))
                    {
                        trainExists = true;
                    }

                    LevelManager.instance.SetTrainsClickable(true);

                    if (!currentCrab.GetComponent<CrabController>().IsValid() || !trainExists || !isCurrentCrabCrustacean)
                    {
                        wrong++;
                    }
                }
                break;

            case KioskState.CrabWaiting:
                {
                    //DisableButtons();
                    //SetState(KioskState.CrabLeaving);

                    // do nothing for now
                }
                break;

            case KioskState.CrabRejected:
                {
                    DisableButtons();

                    bool trainExists = false;
                    if (LevelManager.instance.CheckTrainIDValidity(currentCrab.GetComponent<CrabController>().GetTrainID()))
                    {
                        trainExists = true;
                    }

                    if (currentCrab.GetComponent<CrabController>().IsValid() && trainExists && isCurrentCrabCrustacean)
                    {
                        wrong++;

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
                    crabsToday++;
                    total++;

                    UpdateRating();

                    crabCountGoal.IncrementGoal(crabsToday);

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

    private void SummonCrab()
    {
        /*if (isCrabBeingProcessed) return;
        isCrabBeingProcessed = true;

        isWaiting = false;

        // check that the weather for that crab is valid
        WeatherType currWeather = WeatherManager.instance.GetCurrentWeather();

        var (chosen, chosenIdx) = crabSelector.ChooseCrab(); // failsafe in case there are no valid waiting crabs

        foreach (WaitingCrab waitingCrab in waitingCrabs)  // check if there are any new trains that waiting crabs can board
        {
            if (CheckWeather(waitingCrab.crab, currWeather)) // check if this waiting crab is cool with the current weather
            {
                CrabController crabController = waitingCrab.crab.GetComponent<CrabController>();

                if (clock.CheckTrainIDValidity(crabController.GetTrainID()))
                {
                    chosen = waitingCrab.crab;
                    chosenIdx = waitingCrab.id;
                    break;
                }
            }

        }
        */
        WeatherType currWeather = WeatherManager.instance.GetCurrentWeather();
        var (chosen, chosenIdx) = crabSelector.ChooseCrab();

        bool validWeather = CheckWeather(chosen, currWeather);

        while (!validWeather)   // check if the chosen crab is cool with the current weather
        {
            (chosen, chosenIdx) = crabSelector.ChooseCrab();
            validWeather = CheckWeather(chosen, currWeather);
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

        Crabdex.instance.HasBeenDiscovered(controller.GetCrabInfo()); // crabdex!!!

        isCurrentCrabCrustacean = Crabdex.instance.IsCrustacean(controller.GetCrabdexName());
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

    public void OnApprove()
    {
        SetState(KioskState.CrabApproved);
    }

    public void OnReject()
    {
        SetState(KioskState.CrabRejected);
    }

    /*public void OnWait()
    {
        DisableButtons();

        isWaiting = true;

        WaitingCrab crab = new WaitingCrab (currentCrab, currentCrabIdx);
        waitingCrabs.Add(crab);

        if (!currentCrab.GetComponent<CrabController>().IsValid() || !isCurrentCrabCrustacean)
        {
            wrong++;
        }

        crabsToday++;
        total++;

        UpdateRating();

        crabCountGoal.IncrementGoal(crabsToday);

        currentCrab.GetComponent<CrabController>().SetState(CrabController.CrabState.Waiting);
        StartCoroutine(WaitAMoment());
    }*/

    private IEnumerator WaitForAnimEnd()
    {
        yield return new WaitForSeconds(0.5f);
        SetState(KioskState.CrabLeaving);
    }
    private IEnumerator WaitBeforeSummon()
    {
        yield return new WaitForSeconds(crabSpeed);
        SummonCrab();
    }

    public bool IsCrabValid()
    {
        return currentCrab.GetComponent<CrabController>().IsValid();
    }

    public void DowngradedCart()
    {
        wrong += 0.5f;
    }

    public void UpgradedCart()
    {
        total++;
    }

    public Cart.Type GetCurrentCrabTicket()
    {
        return currentCrab.GetComponent<CrabController>().GetTicketType();
    }

    public void GivePlayerCoins(int newCoins)
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + (int)(newCoins * ratingGoal.GetRating()));
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
    }

    private void UpdateRating()
    {
        ratingGoal.UpdateRating((total - wrong) / (float)total);
    }

    public void SetCrabSpeedUpgrade()
    {
        int dropRate = PlayerPrefs.GetInt("crabDropRate");
        if (dropRate == 0)
        {
            crabSpeed = Random.Range(3, 5);
        }
        else if (dropRate == 1)
        {
            crabSpeed = Random.Range(2, 4);
        }
        else if (dropRate == 2)
        {
            crabSpeed = Random.Range(1, 3);
        }
        else if (dropRate == 3)
        {
            crabSpeed = Random.Range(1, 2);
        }

    }

    public int GetTotalCrabs()
    {
        return crabsToday;
    }

    public void EnableButtons()
    {
        rejectButton.interactable = true;
        approveButton.interactable = true;
        waitButton.interactable = true;
    }
    public void DisableButtons()
    {
        rejectButton.interactable = false;
        approveButton.interactable = false;
        waitButton.interactable = false;
    }

}
