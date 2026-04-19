using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    [SerializeField] private RectTransform pointerTransform;
    [SerializeField] private RectTransform[] waypoints; // indices should correspond to states
    [SerializeField] private GameObject[] texts;  // indices should correspond to states
    [SerializeField] private GameObject clickAnyWhereToContinue;

    [Header("Misc")]
    [SerializeField] private Button approveButton;
    [SerializeField] private Button rejectButton;


    private bool isMoving = false;
    private Vector2 currentVelocity;
    private bool isFirstCrab;

    public enum TutorialState
    {
        setup,
        photos,
        names,
        names2,
        names3,
        approve,
        ticketType,
        preferences,
        trainSwitch,
        endDay1,
        clock,
        reject,
        multiples,
        oversize
    }
    private TutorialState tutorialState;

    public TutorialState GetTutorialState()
    {
        return tutorialState;
    }

    private void Awake()
    {
        tutorialState = TutorialState.setup;
    }

	private void Start()
	{
		if (PlayerPrefs.GetInt("tutorialState") != 0) return;
        isFirstCrab = true;
	}

	public void SetState(TutorialState tutorialStateNew)
    {
        TutorialState prev = tutorialState;
        tutorialState = tutorialStateNew;
        switch (tutorialState)
        {
            case TutorialState.setup:
                // wait for level to popup
                //ShowPointer();
                if (prev != TutorialState.setup) return;
                isFirstCrab = true;
                pointer.SetActive(false);
                ShowText();

                break;

            case TutorialState.photos:
                ShowPointer();
                break;

            case TutorialState.names:
                Kiosk.instance.GetCurrCrab().GetComponent<CrabController>().GetID().transform.GetChild(1).GetComponent<IDHover>().ForceMagnifyOff();
                ShowPointer();
                break;
            case TutorialState.names2:
                ShowPointer();
                break;
            case TutorialState.names3:
                ShowPointer();
                break;
            case TutorialState.approve:
                ShowPointer();
                break;
            case TutorialState.ticketType:
                ShowPointer();
                break;
            case TutorialState.preferences:
                ShowPointer();
                break;
            case TutorialState.trainSwitch:
                ShowPointer();
                break;
            case TutorialState.endDay1:
                if (prev == TutorialState.endDay1) return;
                isFirstCrab = false;
                HideText();
                HidePointer();
                ShowText();
                break;
        }
        PlayerPrefs.SetInt("tutorialState", (int)tutorialState);
    }

    public void ShowPointer()
    {
        pointer.SetActive(true);
        isMoving = true;
        //pointer.GetComponent<RectTransform>().anchoredPosition = waypoints[(int) tutorialState].anchoredPosition;
    }
    public void HidePointer()
    {
        pointer.SetActive(false);
    }

    public void ShowText()
    {
        texts[(int)tutorialState].SetActive(true);

        if (tutorialState == TutorialState.approve || tutorialState == TutorialState.ticketType || tutorialState == TutorialState.preferences || tutorialState == TutorialState.trainSwitch) return;
        clickAnyWhereToContinue.SetActive(true);
    }

    public void HideText()
    {
        texts[(int)tutorialState].SetActive(false);
    }

    public void Continue()
    {
        clickAnyWhereToContinue.SetActive(false);
        HideText();

        if (tutorialState == TutorialState.endDay1)
        {
            // ignore for now
            return;
        }
        else
        {
            SetState((TutorialState)((int)tutorialState + 1));
        }
    }

    public void ProgressTutorial(TutorialState tutorialState)
    {
        SetState(tutorialState);
    }

    private void Update()
    {
        if (isMoving)
        {
            pointerTransform.anchoredPosition = Vector2.SmoothDamp(pointerTransform.anchoredPosition, waypoints[(int)tutorialState].anchoredPosition, ref currentVelocity, 0.25f);

            if (Vector2.Distance(pointerTransform.anchoredPosition, waypoints[(int)tutorialState].anchoredPosition) < 1f)
            {
                isMoving = false;
                ShowText();

                if (tutorialState == TutorialState.photos)
                {
                    Kiosk.instance.GetCurrCrab().GetComponent<CrabController>().GetID().transform.GetChild(1).GetComponent<IDHover>().ForceMagnifyOn();
                }
                else if (tutorialState == TutorialState.names2)
                {
                    Kiosk.instance.GetCurrCrab().GetComponent<CrabController>().GetTicket().GetComponent<Ticket>().BringForward();
                    SetState(TutorialState.names3);
                }
            }
        }
    }

    public bool GetIsFirstCrab()
    {
        return isFirstCrab;
    }

}
