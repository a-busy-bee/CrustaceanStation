using UnityEngine;
using UnityEngine.EventSystems;
public class ShuttleSelection : TransportSelection, IPointerClickHandler
{
    override public void OnPointerClick(PointerEventData eventData)
    {
        if (isClickable)
        {
            PopupManager.instance.ShowPopup(PopupManager.Type.shuttle);
        }
    }

    override public int Depart()
    {
        return PopupManager.instance.DepartShuttle();
    }

    
}
