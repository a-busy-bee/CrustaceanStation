using UnityEngine;

public class CrabController : MonoBehaviour
{
    [SerializeField] private CrabInfo crabInfo;
    private RectTransform rectTransform;

    [SerializeField] private Canvas canvas;


    // TICKET AND ID
    private GameObject ticketAndIDParentObject;
    [SerializeField] private GameObject ticketPrefab;
    private GameObject ticket;
    [SerializeField] private GameObject idPrefab;
    private GameObject id;
    private CrabSelector crabSelector;
    private string trainID = "";
    private Cart.Type cartType;


    // VALIDITY
    private bool isValid = true;


    //MOVEMENT
    private bool isMoving = false;
    private bool approachingKiosk = false;
    private bool leavingKiosk = false;
    private Vector3 kioskEndPos; // where we want the crab to be when it's at the kiosk
    private Vector3 kioskStartPos; // where we want the crab to pop out from when it approaches the kiosk
    private Vector3 currentVelocity;

    private bool presented = false;


    //MISC
    private Clock clock;
    private Kiosk kiosk;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        kioskStartPos = new Vector3(-470, -500, 0);
        kioskEndPos = new Vector3(-470, 89, 0); 

        if (crabInfo.type == CrabInfo.CrabType.scopeCreep)
        {
            kioskEndPos.y = 31;
        }
        else if (crabInfo.type == CrabInfo.CrabType.catfish)
        {
            kioskEndPos.y = 45.8f;
        }
        else if (crabInfo.type == CrabInfo.CrabType.horseshoe)
        {
            kioskEndPos.y = 129;
        }
        else if (crabInfo.type == CrabInfo.CrabType.isopod)
        {
            kioskEndPos.y = 71;
        }
        else if (crabInfo.type == CrabInfo.CrabType.seamonkeys)
        {
            kioskEndPos.y = 111;
        }
        else if (crabInfo.type == CrabInfo.CrabType.ittybitty)
        {
            kioskEndPos.y = 115;
        }
        else if (crabInfo.type == CrabInfo.CrabType.isopodTiny || crabInfo.type == CrabInfo.CrabType.hermit)
        {
            kioskEndPos.y = 102;
        }

        rectTransform.anchoredPosition = kioskStartPos;

        crabInfo.crabName = CrabNameGenerator.instance.GetNameByType(crabInfo.type);
	}

    public void SetCanvas(Canvas newCanvas)
    {
        canvas = newCanvas;
    }
    public void SetCrabSelector(CrabSelector newSelector)
    {
        crabSelector = newSelector;
    }

    public void SetClockAndKiosk(Clock newClock, Kiosk newKiosk)
    {
        clock = newClock;
        kiosk = newKiosk;
    }

    public void SetTicketAndIDParentObject(GameObject newParent)
    {
        ticketAndIDParentObject = newParent;
    }

    public void PresentTicketAndID()
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
            trainID = ticket.GetComponent<Ticket>().GetRandomTrainID();
        }
        else
        {
            trainID = clock.GetRandomCurrentTrainID();

        }
        ticket.GetComponent<Ticket>().SetTrainID(trainID);

        // TRAIN CART TYPE FORGERY (OR NOT)
        cartType = clock.GetRandomCurrentCartType();
        ticket.GetComponent<Ticket>().SetSprite(cartType);
    }

    public string GetTrainID()
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
    public void RemoveTicketAndID()
    {
        Destroy(ticket);
        Destroy(id);
    }

    public void MakeAppear()
    {
        isMoving = true;
        approachingKiosk = true;
        leavingKiosk = false;
    }
    public void MakeDisappear()
    {
        RemoveTicketAndID();
        isMoving = true;
        approachingKiosk = false;
        leavingKiosk = true;
    }

	private void Update()
    {
        if (isMoving)
        {
            if (approachingKiosk)
            {
                rectTransform.anchoredPosition = Vector3.SmoothDamp(rectTransform.anchoredPosition, kioskEndPos, ref currentVelocity, 0.25f);

                if (Vector2.Distance(rectTransform.anchoredPosition, kioskEndPos) < 10f && !presented)
                {
                    presented = true;
                    PresentTicketAndID();
                    kiosk.EnableButtons();
                }
                else if (Vector2.Distance(rectTransform.anchoredPosition, kioskEndPos) < 0.1f)
                {
                    approachingKiosk = false;
                    isMoving = false;
                    rectTransform.anchoredPosition = kioskEndPos;



                }
                
            }
            else if (leavingKiosk)
            {
                rectTransform.anchoredPosition = Vector3.SmoothDamp(rectTransform.anchoredPosition, kioskStartPos, ref currentVelocity, 0.4f);

                if (Vector2.Distance(rectTransform.anchoredPosition, kioskStartPos) < 0.1f)
                {
                    leavingKiosk = false;
                    isMoving = false;
                    rectTransform.anchoredPosition = kioskStartPos;

                    kiosk.DisableButtons();
                }

            }


        }
    }


}
