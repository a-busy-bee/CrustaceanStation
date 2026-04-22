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

        if (TutorialManager.instance != null
            && TutorialManager.instance.GetCurrTutorialState() == Tutorial.TutorialState.preferences
            && !newIsClickable) TutorialManager.instance.ProgressTutorial();
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
            if (TutorialManager.instance != null) TutorialManager.instance.ProgressTutorial();

        }
    }

    virtual public int Depart()
    {
        //return popup.GetComponent<CartPopup>().DepartTrain(railNumber);
        return PopupManager.instance.DepartTrain(cartInfo.cartType);
        //return PopupManager.instance.DepartTrain(railNumber, cartInfo.cartType);
    }


}
