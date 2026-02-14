using UnityEngine;
using System.Collections.Generic;

public class CrabController : MonoBehaviour
{
    [SerializeField] private CrabInfo crabInfo;
    private RectTransform rectTransform;


    // TICKET AND ID
    private GameObject ticketAndIDParentObject;
    [SerializeField] private GameObject ticketPrefab;
    private GameObject ticket;
    [SerializeField] private GameObject idPrefab;
    private GameObject id;
    private CrabSelector crabSelector;
    private Rail.RailDirection trainID = Rail.RailDirection.North;
    private Cart.Type cartType;
    private bool presented = false;


    // VALIDITY
    private bool isValid = true;


    //MOVEMENT
    private Vector3 kioskEndPos; // where we want the crab to be when it's at the kiosk
    private Vector3 kioskStartPos; // where we want the crab to pop out from when it approaches the kiosk
    private Vector3 currentVelocity;


    //MISC
    private Kiosk kiosk;
    private Emotion emotion;

    // STATE MACHINE
    public enum CrabState
    {
        Summoned,           // crab moving up
        AtKiosk,            // crab at kiosk
        Waiting,            // crab was given the waiting sticker
        Emoting,            // play emotion
        Leaving,            // crab moving down
        Left,               // destroy crab object
    }
    public CrabState crabState { get; private set; }
    private bool isWaiting = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        crabInfo.crabName = CrabNameGenerator.instance.GetNameByType(crabInfo.type);
        emotion = GetComponent<RectTransform>().Find("Emotions").GetComponent<Emotion>();
    }

    private void Start()
    {
        float xPos = kiosk.GetCrabPositionInKiosk();
        if (crabInfo.isLarge) {
            xPos = -492;
        }

        kioskStartPos = new Vector3(xPos, -600, 0);
        kioskEndPos = new Vector3(xPos, crabInfo.kioskHeight, 0);

        rectTransform.anchoredPosition = kioskStartPos;
    }

    // State machine go brrrrr
    public void SetState(CrabState newState, string emotionToPlay = "")
    {
        CrabState prevState = crabState;
        crabState = newState;

        switch (crabState)
        {
            case CrabState.Summoned:
                {
                    // do nothing, logic already in Update loop
                }
                break;
            case CrabState.AtKiosk:
                {
                    rectTransform.anchoredPosition = kioskEndPos;
                }
                break;
            case CrabState.Waiting:
                {
                    // move crab offscreen & into waiting pool
                    isWaiting = true;
                }
                break;
            case CrabState.Emoting:
                {
                    emotion.PlayEmotion(emotionToPlay);
                }
                break;
            case CrabState.Leaving:
                {
                    RemoveTicketAndID();
                    //kiosk.SetState(Kiosk.KioskState.CrabLeaving);

                    // rest of the logic is in the update loop
                }
                break;
            case CrabState.Left:
                {
                    rectTransform.anchoredPosition = kioskStartPos;

                    // destroy crab
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void Update()
    {
        switch (crabState)
        {
            case CrabState.Summoned:
                {
                    rectTransform.anchoredPosition = Vector3.SmoothDamp(rectTransform.anchoredPosition, kioskEndPos, ref currentVelocity, 0.25f);

                    if (Vector2.Distance(rectTransform.anchoredPosition, kioskEndPos) < 5f && !presented)
                    {
                        presented = true;
                        PresentTicketAndID();

                        kiosk.SetState(Kiosk.KioskState.CrabPresent);
                    }
                    else if (Vector2.Distance(rectTransform.anchoredPosition, kioskEndPos) < 0.1f)
                    {
                        SetState(CrabState.AtKiosk);
                    }
                }
                break;
            case CrabState.Leaving:
                {
                    rectTransform.anchoredPosition = Vector3.SmoothDamp(rectTransform.anchoredPosition, kioskStartPos, ref currentVelocity, 0.4f);

                    if (Vector2.Distance(rectTransform.anchoredPosition, kioskStartPos) < 0.1f)
                    {
                        if (!isWaiting)
                        {
                            SetState(CrabState.Left);
                        }
                        else // TEMP
                        {
                            kiosk.SetState(Kiosk.KioskState.Empty);
                        }
                        
                        // TODO: if waiting, do waiting logic
                    }
                }
                break;
        }

    }

    private void PresentTicketAndID()
    {
        ticket = Instantiate(ticketPrefab, ticketAndIDParentObject.transform);
        id = Instantiate(idPrefab, ticketAndIDParentObject.transform);

        ticket.GetComponent<Ticket>().SetID(id.GetComponent<ID>());
        id.GetComponent<ID>().SetTicket(ticket.GetComponent<Ticket>());

        ticket.GetComponent<Ticket>().PushBack();
        id.GetComponent<ID>().BringForward();

        // SOMETIMES GENERATE MISMATCHING INFO
        string crabName = crabInfo.crabName;
        Sprite crabPhoto = crabInfo.sprite;
        if (Random.Range(0, 10) > 8)                    // RANDOM NAME
        {
            crabName = CrabNameGenerator.instance.GetAnyName();
            if (crabName != crabInfo.crabName)
            {
                isValid = false;
            }
        }
        if (Random.Range(0, 10) > 8)               // RANDOM SPRITE
        {
            crabPhoto = crabSelector.ChooseSprite();
            if (crabPhoto != crabInfo.sprite)
            {
                isValid = false;
            }
        }

        // FIGURE OUT WHICH DOCUMENT IS FORGED
        if (!isValid)
        {
            if (Random.Range(0, 10) > 8)                                       // FORGED ID
            {
                ticket.GetComponent<Ticket>().SetName(crabName);
                id.GetComponent<ID>().SetName(crabInfo.crabName);
            }
            else                                                               // FORGED TICKET
            {
                ticket.GetComponent<Ticket>().SetName(crabInfo.crabName);
                id.GetComponent<ID>().SetName(crabName);
            }
        }
        else // if not forged
        {
            ticket.GetComponent<Ticket>().SetName(crabName);
            id.GetComponent<ID>().SetName(crabName);
        }

        id.GetComponent<ID>().SetIDPhoto(crabPhoto);

        // TRAIN ID FORGERY (OR NOT)
        if (Random.Range(0, 10) > 8)                                            // MORE FORGERY! - TRAIN ID
        {
            trainID = LevelManager.instance.GetRandomTrainDirection();
        }
        else
        {
            trainID = LevelManager.instance.GetRandomCurrentTrainDirection();
        }
        ticket.GetComponent<Ticket>().SetTrainDirection(trainID);

        // TRAIN CART TYPE FORGERY (OR NOT)
        cartType = LevelManager.instance.GetRandomCurrentCartType();
        ticket.GetComponent<Ticket>().SetSprite(cartType);
    }
    private void RemoveTicketAndID()
    {
        Destroy(ticket);
        Destroy(id);
    }
    public void SetCrabSelector(CrabSelector newSelector)
    {
        crabSelector = newSelector;
    }
    public void SetClockAndKiosk(Clock newClock, Kiosk newKiosk)
    {
        //clock = newClock;
        kiosk = newKiosk;
    }
    public void SetTicketAndIDParentObject(GameObject newParent)
    {
        ticketAndIDParentObject = newParent;
    }
    public string GetCrabdexName()
    {
        return crabInfo.crabdexName;
    }
    public CrabInfo.WeatherType[] GetFavoriteWeather()
    {
        return crabInfo.favoriteWeatherTypes;
    }
    public Rail.RailDirection GetTrainID()
    {
        return trainID;
    }
    public Cart.Type GetTicketType()
    {
        return cartType;
    }
    public CrabInfo GetCrabInfo()
    {
        return crabInfo;
    }
    public bool IsValid()
    {
        return isValid;
    }
}
