using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Kiosk : MonoBehaviour
{
    private CrabSelector crabSelector;
    private GameObject currentCrab;
    [SerializeField] private Clock clock;

    [SerializeField] private GameObject crabParentObject; // in scene hierarchy: canvas > crabs
    [SerializeField] private GameObject ticketParentObject;
    [SerializeField] private GameObject canvas;

    [SerializeField] private TextMeshProUGUI coinCountText;
    private bool isOpen = false;
    private int crabsToday = 0;
    private float wrong = 0f;
    private int total = 0;
    private bool isCurrentCrabCrustacean = false;

    [Header("Goals")]
    [SerializeField] private RatingGoal ratingGoal;
    [SerializeField] private CrabCountGoal crabCountGoal;

    [Header("Buttons")]
    [SerializeField] private Button approveButton;
    [SerializeField] private Button rejectButton;

    [Header("Wait")]
    private List<GameObject> waitingCrabs = new List<GameObject>();

    private int crabSpeed = 5;

    private void Awake()
    {
        rejectButton.interactable = false;
        approveButton.interactable = false;

        crabSelector = GetComponent<CrabSelector>();
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
        SetCrabSpeedUpgrade();
    }

    public void SummonCrab()
    {
        var (chosen, chosenIdx) = crabSelector.ChooseCrab();
        WeatherType currWeather = WeatherManager.instance.GetCurrentWeather();
        CrabInfo.WeatherType[] chosenWeather = chosen.GetComponent<CrabController>().GetFavoriteWeather();

        bool validWeather = false;
        foreach (CrabInfo.WeatherType weatherType in chosenWeather)
        {
            if (weatherType == currWeather.weatherType)
            {
                validWeather = true;
                break;
            }
        }

        while (!validWeather)
        {
            (chosen, chosenIdx) = crabSelector.ChooseCrab();
            currWeather = WeatherManager.instance.GetCurrentWeather();
            chosenWeather = chosen.GetComponent<CrabController>().GetFavoriteWeather();
            foreach (CrabInfo.WeatherType weatherType in chosenWeather)

            {
                if (weatherType == currWeather.weatherType)
                {
                    validWeather = true;
                    break;
                }
            }
        }

        crabSelector.AddToQueue(chosenIdx);
        currentCrab = Instantiate(chosen, crabParentObject.transform);

        CrabController controller = currentCrab.GetComponent<CrabController>();
        controller.SetCanvas(canvas.GetComponent<Canvas>());
        controller.SetCrabSelector(crabSelector);
        controller.SetClockAndKiosk(clock, this);
        controller.SetTicketAndIDParentObject(ticketParentObject);
        controller.MakeAppear();

        Crabdex.instance.HasBeenDiscovered(controller.GetCrabInfo()); // crabdex!!!

        isCurrentCrabCrustacean = Crabdex.instance.IsCrustacean(controller.GetCrabdexName());
    }
    public void OnApprove()
    {
        
        rejectButton.interactable = false;
        approveButton.interactable = false;

        bool trainExists = false;
        if (clock.CheckTrainIDValidity(currentCrab.GetComponent<CrabController>().GetTrainID()))
        {
            trainExists = true;
        }

        clock.SetTrainsClickable(true);

        if (!currentCrab.GetComponent<CrabController>().IsValid() || !trainExists || !isCurrentCrabCrustacean)
        {
            wrong++;
            
        }
    }

    public void OnReject()
    {
        rejectButton.interactable = false;
        approveButton.interactable = false;

        bool trainExists = false;
        if (clock.CheckTrainIDValidity(currentCrab.GetComponent<CrabController>().GetTrainID()))
        {
            trainExists = true;
        }

        if (currentCrab.GetComponent<CrabController>().IsValid() && trainExists && isCurrentCrabCrustacean)
        {
            wrong++;

            currentCrab.GetComponent<CrabController>().PlayEmotion("any and confused");
        }
        else
        {
            currentCrab.GetComponent<CrabController>().PlayEmotion("any");
        }

        DisappearCrab();
    }

    public void OnWait()
    {
        waitingCrabs.Add(currentCrab);
    }

    public bool IsCrabValid()
    {
        return currentCrab.GetComponent<CrabController>().IsValid();
    }

    public void DowngradedCart()
    {
        wrong+=0.5f;
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
    public void DisappearCrab()
    {
        StartCoroutine(WaitForAnimEnd());
    }

    private void UpdateRating()
    {
        ratingGoal.UpdateRating((total - wrong) / (float)total);
    }

    public void OpenKiosk()
    {
        isOpen = true;
    }

    public void CloseKiosk()
    {
        isOpen = false;
        currentCrab.GetComponent<CrabController>().MakeDisappear();
    }

    private IEnumerator WaitForAnimEnd()
    {
        yield return new WaitForSeconds(0.5f);

        crabsToday++;
        total++;

        UpdateRating();

        crabCountGoal.IncrementGoal(crabsToday);

        currentCrab.GetComponent<CrabController>().MakeDisappear();
        StartCoroutine(WaitAMoment());

    }
    private IEnumerator WaitAMoment()
    {
        yield return new WaitForSeconds(crabSpeed);

        Destroy(currentCrab);

        if (isOpen)
        {
            SummonCrab();
        }
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
    }

    public void DisableButtons()
    {
        rejectButton.interactable = false;
        approveButton.interactable = false;
    }

}
