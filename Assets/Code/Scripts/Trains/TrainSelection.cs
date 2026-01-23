using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TrainSelection : MonoBehaviour, IPointerClickHandler
{
    private TrainController trainController;

    // CART CONTROLS
    private bool isClickable = false;
    private bool isFull = false;

    // CART TYPE
    [SerializeField] private Cart cartInfo;
    private int railNumber;

    // VISUALS
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private Sprite filled;
    [SerializeField] private GameObject popup;

    private void Start()
    {
        if (cartInfo.cartType == Cart.Type.Standard)
        {
            popup = LevelManager.instance.GetStandardCartPopup();
        }
        else if (cartInfo.cartType == Cart.Type.Economy)
        {
            popup = LevelManager.instance.GetEconomyCartPopup();
        }

        popup.SetActive(false);
    }

    public void SetThisClickable(bool newIsClickable)
    {
        isClickable = newIsClickable;
    }

    public void SetController(TrainController newController)
    {
        trainController = newController;
    }

    public void SetRailNumber(int newNumber)
    {
        railNumber = newNumber;
    }

    public float getStartingPos()
    {
        return cartInfo.cartStartingPoint;
    }

    public float getHeight()
    {
        return cartInfo.cartHeight;
    }

    public Cart.Type GetCartType()
    {
        return cartInfo.cartType;
    }

    public bool isItFull()
    {
        return isFull;
    }

    public void SetCartPopup(GameObject newPopup)
    {
        popup = newPopup;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        /*if (isClickable && !isFull)
        {
            // ticket info was wrong
            Cart.Type ticketCartType = Kiosk.instance.GetCurrentCrabTicket();
            if (ticketCartType != cartInfo.cartType && Kiosk.instance.IsCrabValid()) // if the crab is otherwise valid, but chose the wrong cart
            {
                if (ticketCartType == Cart.Type.Economy)
                {
                    // chosen cart was an upgrade, no rating complaints
                    Kiosk.instance.UpgradedCart();
                }
                else if (ticketCartType == Cart.Type.Standard && cartInfo.cartType == Cart.Type.Deluxe)
                {
                    // chosen cart was an upgrade, no rating complaints
                    Kiosk.instance.UpgradedCart();
                }
                else
                {
                    Kiosk.instance.DowngradedCart(); // chosen cart was a downgrade, rating goes down
                }
            }

            // disappear crab
            Kiosk.instance.SetState(Kiosk.KioskState.CrabLeaving);

            // add to how many coins the train has collected 
            trainController.AddToCrabsOnTrain(cartInfo.ticketCost);

            // show that cart is full
            spriteRenderer.sprite = filled;

            LevelManager.instance.SetTrainsClickable(false);
            isFull = true;

            trainController.CheckIfFull();
        }*/

        if (isClickable)
        {
            popup.SetActive(true);
            popup.GetComponent<CartPopup>().Populate(railNumber);
        }
    }

    public int DepartTrain()
    {
       return popup.GetComponent<CartPopup>().DepartTrain(railNumber);
    }


}
