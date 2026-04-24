using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class KioskBase : MonoBehaviour
{
    public static KioskBase instance { get; protected set; }
    [SerializeField] protected Clock clock;


    // CURRENT CRAB
    protected GameObject currentCrab;
    protected bool isCurrentCrabCrustacean = false;
    protected CrabSelector crabSelector;
    protected float crabPositionInKiosk = -470;


    [Header("Kiosk Objects")]
    [SerializeField] protected GameObject crabParentObject; // in scene hierarchy: canvas > crabs
    [SerializeField] protected GameObject ticketParentObject;
    //[SerializeField] private TextMeshProUGUI coinCountText;


    //[Header("Goals")]
    //[SerializeField] private RatingGoal ratingGoal;
    //[SerializeField] private CrabCountGoal crabCountGoal;
    //protected int crabsToday = 0;
    //protected float wrong = 0f;
    //protected int total = 0;


    [Header("Buttons")]
    [SerializeField] protected Button approveButton;
    [SerializeField] protected Button rejectButton;
    //[SerializeField] private Button waitButton;


    /*[Header("Decor")]
    [SerializeField] private GameObject kioskChildren;
    [SerializeField] private GameObject kioskImage;
    [SerializeField] private KioskStyle[] kioskStyles;
    [SerializeField] private DecorItems[] items;
    [SerializeField] private DecorItems[] topDecor;
    [SerializeField] private GameObject leftSlot;
    [SerializeField] private GameObject rightSlot;
    [SerializeField] private GameObject topSlot;*/

    [Header("Debug")]
    [SerializeField] protected bool debugMode;
    [SerializeField] protected List<GameObject> charactersToForce = new List<GameObject>();

    public enum KioskState
    {
        NotOpenYet,         // before the start of the day
        Empty,              // a crab hasn't walked up to the kiosk yet
        CrabPresent,        // crab has presented its ID & ticket for review
        CrabApproved,       // crab has been approved & is selecting train
        CrabRejected,       // crab has been rejected
        CrabLeaving,        // crab is leaving 
        EndOfDay            // day is over, close kiosk
    }
    public KioskState kioskState { get; protected set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        SetState(KioskState.NotOpenYet);
        PlayerPrefs.SetInt("kioskStyle", 0);
    }

    // state machine go brrrrr
    virtual public void SetState(KioskState newState)
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
                    bool trainExists = true;
                    /*if (LevelManager.instance.CheckTrainIDValidity(currentCrab.GetComponent<CrabController>().GetTrainID()))
                    {
                        trainExists = true;
                    }*/

                    LevelManager.instance.SetTrainsClickable(true);

                    if (!currentCrab.GetComponent<CrabController>().IsValid() || !trainExists || !isCurrentCrabCrustacean)
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

                    bool trainExists = true;
                    /*if (LevelManager.instance.CheckTrainIDValidity(currentCrab.GetComponent<CrabController>().GetTrainID()))
                    {
                        trainExists = true;
                    }*/

                    if (currentCrab.GetComponent<CrabController>().IsValid() && trainExists && isCurrentCrabCrustacean)
                    {
                        //wrong++;
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

    virtual protected void SummonCrab()
    {
        var (chosen, chosenIdx) = crabSelector.ChooseCrab();

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


    public void OnApprove()
    {
        SetState(KioskState.CrabApproved);
    }

    public void OnReject()
    {
        SetState(KioskState.CrabRejected);
    }

    protected IEnumerator WaitForAnimEnd()
    {
        yield return new WaitForSeconds(0.5f);
        SetState(KioskState.CrabLeaving);
    }
    protected IEnumerator WaitBeforeSummon()
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
        SummonCrab();
    }

    public bool IsCrabValid()
    {
        return currentCrab.GetComponent<CrabController>().IsValid();
    }

    public CrabInfo GetCrabInfo()
    {
        return currentCrab.GetComponent<CrabController>().GetCrabInfo();
    }
    public Cart.Type GetCurrentCrabTicket()
    {
        return currentCrab.GetComponent<CrabController>().GetTicketType();
    }

    public CrabController GetCurrCrab()
    {
        return currentCrab.GetComponent<CrabController>();
    }

    public void GivePlayerCoins(int newCoins)
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + newCoins);
        //coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
    }

    public int GetTotalCrabs()
    {
        return -1;
    }

    public float GetCrabPositionInKiosk()
    {
        return crabPositionInKiosk;
    }

    public void EnableButtons()
    {
        rejectButton.interactable = true;
        approveButton.interactable = true;
        //waitButton.interactable = true;
    }
    public void DisableButtons()
    {
        rejectButton.interactable = false;
        approveButton.interactable = false;
        //waitButton.interactable = false;
    }

    // DECOR
    /*public void ShowDecor()
    {
        KioskStyle kioskStyle = kioskStyles[PlayerPrefs.GetInt("kioskStyle")];
        int top = PlayerPrefs.GetInt("decor_top");
        int left = PlayerPrefs.GetInt("decor_left");
        int right = PlayerPrefs.GetInt("decor_right");

        topSlot.SetActive(false);
        leftSlot.SetActive(false);
        rightSlot.SetActive(false);

        if (top > 1)
        {
            topSlot.SetActive(true);
            topSlot.GetComponent<Image>().sprite = topDecor[top].sprite;
        }

        if (left > 1)
        {
            leftSlot.SetActive(true);
            leftSlot.GetComponent<Image>().sprite = items[left].sprite;
        }

        if (right > 1)
        {
            rightSlot.SetActive(true);
            rightSlot.GetComponent<Image>().sprite = items[right].sprite;
        }

        kioskImage.GetComponent<Image>().sprite = kioskStyle.sprite;
        kioskImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(kioskStyle.position.x, kioskStyle.position.y, 0);
        kioskChildren.GetComponent<RectTransform>().anchoredPosition = new Vector3(kioskStyle.positionChildren.x, kioskStyle.positionChildren.y, 0);
        crabPositionInKiosk = kioskStyle.crabposition;
    }*/

    public void WrongTransport()
    {

    }
    public void DowngradedCart()
    {
        //wrong += 0.5f;
        PerformanceManager.instance.Incorrect();
    }

    public void UpgradedCart()
    {
        //total++;
        PerformanceManager.instance.Incorrect();
    }
}
