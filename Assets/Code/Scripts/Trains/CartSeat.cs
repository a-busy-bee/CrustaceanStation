using System;
using System.Collections.Generic;
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


    // ANIMATED REACTION
    public enum ReactionType
    {
        toxicSelf,  // toxic critter if paired with anyone nontoxic
        toxicOther, // nontoxic critter if paired with toxic
        happy,      // heart anim
        comrade,    // handshake anim
        neutral,    // :/ anim
        swords,     // crossed swords anim
        yummy,      // devour anim
        fear        // sweat anim
    }
    private Dictionary<ReactionType, string> reactions = new Dictionary<ReactionType, string>()
    {
        { ReactionType.toxicSelf,   "toxicSelf" },
        { ReactionType.toxicOther,  "toxicOther" },
        { ReactionType.happy,       "happy" },
        { ReactionType.comrade,     "comrade" },
        { ReactionType.neutral,     "neutral" },
        { ReactionType.swords,      "swords" },
        { ReactionType.yummy,       "yummy" }, 
        { ReactionType.fear,        "fear" }, 
    };
    private GameObject reactionObject;


    // STATE
    private Mini currMini;
    private bool isTaken;
    private bool hasSelected;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
 
    private void Start()
    {
        reactionObject = GetComponent<RectTransform>().GetChild(0).gameObject;
        reactionObject.SetActive(false);
    }

	private void OnEnable()
	{
        if (reactionObject == null) return;
		reactionObject.SetActive(false);
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

    public bool IsTaken()
    {
        return isTaken;
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

        if (currMini.isMultiple && !cartPopup.CheckIfBothSeatsAreOpen(row, column)) return;

        if (currMini.isMultiple)
        {
            image.sprite = currMini.miniSprite;
            image.color = ghostAlpha;

            cartPopup.ShowGhostMultiple(currMini.multSprite, row, column);

            PlayAnim(ReactionType.happy);
            cartPopup.PlayGhostAnim(row, column);
        }
        else
        {
            // show ghost sprite
            image.sprite = currMini.miniSprite;
            image.color = ghostAlpha;

            cartPopup.CheckRelationship(row, column);
        }

    }

    public void ShowGhostSpriteForMultiple(Sprite alt)
    {
        image.sprite = alt;
        image.color = ghostAlpha;
    }

    public void HideGhostSpriteForMultiple()
    {
        image.sprite = null;
        image.color = emptyAlpha;
        reactionObject.SetActive(false);
    }

    public void SetSpriteForMultiple(Sprite alt)
    {
        image.sprite = alt;
        image.color = baseColor;

        isTaken = true;
    }

    public void PopulateForMultiple(Sprite alt)
    {
        isTaken = true;
        image.sprite = alt;
        image.color = baseColor;
    }

    public void PlayAnim(ReactionType reactionType)
    {
        // play anim of the mini beside the placed one, if possible
        //Debug.Log("play anim for " + row + " and " + column);
        reactionObject.SetActive(true);
        reactionObject.GetComponent<Animator>().Play(reactions[reactionType]);
    }

    public void StopAnim()
    {
        reactionObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isTaken || hasSelected) return;
        if (currMini.isMultiple && (isTaken || !cartPopup.CheckIfBothSeatsAreOpen(row, column))) return;
        if (currMini.isMultiple) cartPopup.HideGhostMultiple(row, column);

        // hide ghost sprite
        image.sprite = null;
        image.color = emptyAlpha;
        reactionObject.SetActive(false);
        cartPopup.TurnOffAnims(row, column);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTaken || hasSelected) return;
        if (currMini.isMultiple) cartPopup.SeatMultiple(currMini.multSprite, row, column);

        image.sprite = currMini.miniSprite;
        image.color = baseColor;

        isTaken = true;

        cartPopup.SeatCharacter(row, column);
    }

}
 