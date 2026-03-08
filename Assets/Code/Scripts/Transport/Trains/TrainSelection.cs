using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TrainSelection : TransportSelection, IPointerClickHandler
{
    override public int Depart()
    {
        //return popup.GetComponent<CartPopup>().DepartTrain(railNumber);
        return PopupManager.instance.DepartTrain(cartInfo.cartType);
        //return PopupManager.instance.DepartTrain(railNumber, cartInfo.cartType);
    }


}
