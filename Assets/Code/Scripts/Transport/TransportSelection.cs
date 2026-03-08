using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TransportSelection : MonoBehaviour, IPointerClickHandler
{
    // CART CONTROLS
    protected bool isClickable = false;
    // CART TYPE
    [SerializeField] protected Cart cartInfo;

    public void SetThisClickable(bool newIsClickable)
    {
        isClickable = newIsClickable;
    }

    public Cart.Type GetCartType()
    {
        return cartInfo.cartType;
    }

    virtual public void OnPointerClick(PointerEventData eventData)
    {
        if (isClickable)
        {
            PopupManager.instance.ShowPopup(PopupManager.Type.train, cartInfo.cartType);
            //PopupManager.instance.ShowPopup(railNumber, cartInfo.cartType);
            //popup.SetActive(true);
            //popup.GetComponent<CartPopup>().Populate(railNumber);
        }
    }

    virtual public int Depart()
    {
        //return popup.GetComponent<CartPopup>().DepartTrain(railNumber);
        return PopupManager.instance.DepartTrain(cartInfo.cartType);
        //return PopupManager.instance.DepartTrain(railNumber, cartInfo.cartType);
    }


}
