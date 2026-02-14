using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TrainSelection : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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
        if (isClickable)
        {
            PopupManager.instance.ShowPopup(railNumber, cartInfo.cartType);
            //popup.SetActive(true);
            //popup.GetComponent<CartPopup>().Populate(railNumber);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        PopupManager.instance.SetHoveredTrain(railNumber, cartInfo.cartType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PopupManager.instance.ResetHoveredTrain();
    }

    public int DepartTrain()
    {
        //return popup.GetComponent<CartPopup>().DepartTrain(railNumber);
        return PopupManager.instance.DepartTrain(railNumber, cartInfo.cartType);
    }


}
