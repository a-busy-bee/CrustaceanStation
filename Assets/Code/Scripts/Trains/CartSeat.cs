using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CartSeat : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private CartPopup cartPopup;
    private int row = -1;
    private int column = -1;

    // SPRITE
    private Image image;
    [SerializeField] private Color emptyAlpha;
    [SerializeField] private Color ghostAlpha;
    [SerializeField] private Color baseColor;
    
    private Mini currMini;
    private bool isTaken;
    private bool hasSelected;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void InitSeat(CartPopup newPopup, int newRow, int newColumn)
    {
        cartPopup = newPopup;
        row = newRow;
        column = newColumn;
    }

    public void HasSelected(bool newHasSelected)
    {
        hasSelected = newHasSelected;
    }

    public (int, int) GetID()
    {
        return (row, column);
    }

    public void SetCharacter(Mini newMini)
    {
        currMini = newMini;
    }

    public void Populate(Mini mini)
    {
        // if empty, show empty
        if (mini.miniType == CartPopup.MiniType.empty)
        {
            image.sprite = null;
            image.color = emptyAlpha;
            isTaken = false;
        }
        // if filled, show sprite
        else
        {
            image.sprite = mini.miniSprite;
            image.color = baseColor;
            isTaken = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isTaken || hasSelected) return;
        
        // show ghost sprite
        image.sprite = currMini.miniSprite;
        image.color = ghostAlpha;

        cartPopup.PlayAnim(row, column);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isTaken || hasSelected) return;

        // hide ghost sprite
        image.sprite = null;
        image.color = emptyAlpha;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTaken || hasSelected) return;

        image.sprite = currMini.miniSprite;
        image.color = baseColor;

        isTaken = true;

        cartPopup.SeatCharacter(row, column);
    }

}
 