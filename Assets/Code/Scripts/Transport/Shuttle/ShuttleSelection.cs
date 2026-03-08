using UnityEngine;
using UnityEngine.EventSystems;
public class ShuttleSelection : TransportSelection, IPointerClickHandler
{
    override public void OnPointerClick(PointerEventData eventData)
    {
        if (isClickable)
        {
            PopupManager.instance.ShowPopup(PopupManager.Type.shuttle);
            //PopupManager.instance.ShowPopup(railNumber, cartInfo.cartType);
            //popup.SetActive(true);
            //popup.GetComponent<CartPopup>().Populate(railNumber);
        }
    }

    override public int Depart()
    {
        //return popup.GetComponent<CartPopup>().DepartTrain(railNumber);
        return PopupManager.instance.DepartShuttle();
        //return PopupManager.instance.DepartTrain(railNumber, cartInfo.cartType);
    }

    
}
