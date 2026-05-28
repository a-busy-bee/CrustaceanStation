using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class CartSeatLarge : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CartSeat cartSeat;

    public void InitSeat(CartSeat cartSeatNew)
    {
        cartSeat = cartSeatNew;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!cartSeat.GetCurrMini().isLarge) return;
        cartSeat.PointerEnterLarge();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!cartSeat.GetCurrMini().isLarge) return;
        cartSeat.PointerExitLarge();
    }
}
