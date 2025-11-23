using System.Collections;
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

    [Header("Goals")]
    [SerializeField] private RatingGoal ratingGoal;
    [SerializeField] private CrabCountGoal crabCountGoal;

    [Header("Buttons")]
    [SerializeField] private Button approveButton;
    [SerializeField] private Button rejectButton;

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
        currentCrab = Instantiate(crabSelector.ChooseCrab(), crabParentObject.transform);

        CrabController controller = currentCrab.GetComponent<CrabController>();
        controller.SetCanvas(canvas.GetComponent<Canvas>());
        controller.SetCrabSelector(crabSelector);
        controller.SetClockAndKiosk(clock, this);
        controller.SetTicketAndIDParentObject(ticketParentObject);
        controller.MakeAppear();
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

        if (!currentCrab.GetComponent<CrabController>().IsValid() || !trainExists)
        {
            //Debug.Log("approve, wrong");
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

        if (currentCrab.GetComponent<CrabController>().IsValid() && trainExists)
        {
            //Debug.Log("reject, wrong");
            wrong++;
        }

        DisappearCrab();
    }

    public bool IsCrabValid()
    {
        return currentCrab.GetComponent<CrabController>().IsValid();
    }

    public void DowngradedCart()
    {
        //Debug.Log("downgraded");
        wrong+=0.5f;
    }

    public void UpgradedCart()
    {
        //Debug.Log("upgraded");
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
        crabsToday++;
        total++;

        UpdateRating();

        crabCountGoal.IncrementGoal(crabsToday);

        currentCrab.GetComponent<CrabController>().MakeDisappear();
        StartCoroutine(WaitAMoment());
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
