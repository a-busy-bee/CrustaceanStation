using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CartSeat : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private TransportPopup cartPopup;
    //private PopupManager.Type type;
    private int row = -1;
    private int column = -1;

    // SPRITE
    private Image image;
    private Image imageLarge;
    [SerializeField] private Color emptyAlpha;
    [SerializeField] private Color ghostAlpha;
    [SerializeField] private Color baseColor;

    [SerializeField] private AudioManager audioManager;


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
        fear,        // sweat anim
        none
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
    private GameObject imageObject;
    private GameObject imageObjectLarge;
    private GameObject splatterObject;
    private ReactionType currReaction;


    // STATE
    private Mini currMini;
    private bool isTaken;
    private bool hasSelected;

    private void OnEnable()
    {
        if (reactionObject == null) return;
        reactionObject.SetActive(false);
    }

    public void InitSeat(TransportPopup newPopup, int newRow, int newColumn)
    {
        cartPopup = newPopup;
        row = newRow;
        column = newColumn;
        //type = PopupManager.Type.train;

        reactionObject = GetComponent<RectTransform>().GetChild(2).gameObject;
        reactionObject.SetActive(false);

        imageObject = GetComponent<RectTransform>().GetChild(1).gameObject;
        image = imageObject.GetComponent<Image>();

        splatterObject = GetComponent<RectTransform>().GetChild(0).gameObject;
        splatterObject.SetActive(false);

        if (GetComponent<RectTransform>().childCount == 4)
        {
            imageObjectLarge = GetComponent<RectTransform>().GetChild(3).gameObject;
            imageLarge = imageObjectLarge.GetComponent<Image>();

            imageObjectLarge.GetComponent<CartSeatLarge>().InitSeat(this);
        }
    }

    public void HasSelected(bool newHasSelected)
    {
        hasSelected = newHasSelected;
    }

    public (int, int) GetID()
    {
        return (row, column);
    }

    public Mini GetCurrMini()
    {
        return currMini;
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
        if (mini.miniType == PopupManager.MiniType.empty)
        {
            image.sprite = null;
            image.color = emptyAlpha;

            if (imageLarge != null)
            {
                imageLarge.sprite = null;
                imageLarge.color = emptyAlpha;
            }

            isTaken = false;
        }
        // if filled, show sprite
        else
        {
            if (mini.isLarge)
            {
                if (imageLarge != null)
                {
                    imageLarge.sprite = mini.miniSprite;
                    imageLarge.color = baseColor;
                }
                image.sprite = null;
                image.color = emptyAlpha;

            }
            else
            {
                image.sprite = mini.miniSprite;
                image.color = baseColor;


                if (imageLarge != null)
                {
                    imageLarge.sprite = null;
                    imageLarge.color = emptyAlpha;
                }
            }

            isTaken = true;
        }

        if (currReaction == ReactionType.fear)
        {
            image.sprite = null;
            image.color = emptyAlpha;

            if (imageLarge != null)
            {
                imageLarge.sprite = null;
                imageLarge.color = emptyAlpha;
            }

            isTaken = false;
            cartPopup.RemoveCharacter(row, column);

            splatterObject.SetActive(true);
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

    public void ShowGhostSpriteForLarge(Sprite sprite)
    {
        if (imageLarge != null)
        {
            image.sprite = null;
            image.color = emptyAlpha;

            imageLarge.sprite = sprite;
            imageLarge.color = ghostAlpha;
        }
    }

    public void HideGhostSpriteForLarge()
    {
        if (imageLarge != null)
        {
            imageLarge.sprite = null;
            imageLarge.color = emptyAlpha;
        }
    }

    public void SetSpriteForLarge(Sprite sprite)
    {
        if (imageLarge != null)
        {
            image.sprite = null;
            image.color = emptyAlpha;

            imageLarge.sprite = sprite;
            imageLarge.color = baseColor;
        }

        isTaken = true;
    }

    public void PopulateForLarge(Sprite sprite)
    {
        if (imageLarge != null)
        {
            image.sprite = null;
            image.color = emptyAlpha;

            imageLarge.sprite = sprite;
            imageLarge.color = baseColor;
        }

        isTaken = true;
    }

    public void PlayAnim(ReactionType reactionType)
    {
        // play anim of the mini beside the placed one, if possible
        //Debug.Log("play anim for " + row + " and " + column);
        reactionObject.SetActive(true);
        reactionObject.GetComponent<Animator>().Play(reactions[reactionType]);

        currReaction = reactionType;
    }

    public void StopAnim()
    {
        currReaction = ReactionType.none;
        reactionObject.SetActive(false);
    }

    public void PointerExitLarge()
    {
        if (isTaken || hasSelected) return;
        if (currMini.isLarge && (isTaken || !cartPopup.CheckIfBothSeatsAreOpen(row, column))) return;

        imageLarge.sprite = null;
        imageLarge.color = emptyAlpha;

        currReaction = ReactionType.none;

        reactionObject.SetActive(false);
        cartPopup.TurnOffAnims(row, column);
    }
    public void PointerEnterLarge()
    {
        if (!currMini.isLarge || isTaken || hasSelected) return;
        if (!cartPopup.CheckIfBothSeatsAreOpen(row, column)) return;

        imageLarge.sprite = currMini.miniSprite;
        imageLarge.color = ghostAlpha;
        PlayAnim(ReactionType.happy);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isTaken || hasSelected) return;
        if ((currMini.isMultiple || currMini.isLarge) && (isTaken || !cartPopup.CheckIfBothSeatsAreOpen(row, column))) return;

        if (currMini.isMultiple) cartPopup.HideGhostMultiple(row, column);

        // hide ghost sprite
        image.sprite = null;
        image.color = emptyAlpha;

        currReaction = ReactionType.none;

        if (!currMini.isLarge)
        {
            reactionObject.SetActive(false);
            cartPopup.TurnOffAnims(row, column);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTaken || hasSelected) return; // TODO: implement wiggle to give player feedback
        if ((currMini.isMultiple || currMini.isLarge) && (isTaken || !cartPopup.CheckIfBothSeatsAreOpen(row, column))) return; // TODO: implemenet smtg to give player feedback

        if (currMini.isMultiple)
        {
            cartPopup.SeatMultiple(currMini.multSprite, row, column);
            image.sprite = currMini.miniSprite;
            image.color = baseColor;
        }
        else if (currMini.isLarge)
        {
            if (imageLarge != null)
            {
                imageLarge.sprite = currMini.miniSprite;
                imageLarge.color = baseColor;

                image.sprite = null;
                image.color = emptyAlpha;
                cartPopup.SeatLargeSecondSeat(row, column);
            }
            else
            {
                cartPopup.SeatLarge(currMini.miniSprite, row, column - 1);
            }
        }
        else
        {
            image.sprite = currMini.miniSprite;
            image.color = baseColor;
        }


        isTaken = true;

        cartPopup.SeatCharacter(row, column);
        audioManager.Play("seat");

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if (isTaken || hasSelected) return;
        if ((currMini.isMultiple || currMini.isLarge) && !cartPopup.CheckIfBothSeatsAreOpen(row, column)) return;
        // if col is 1 or 3 and the prev seat has a large
        if ((column == 1 || column == 3) && cartPopup.IsPrevSeatWithLarge(row, column)) return;

        if (currMini.isMultiple)
        {
            image.sprite = currMini.miniSprite;
            image.color = ghostAlpha;

            cartPopup.ShowGhostMultiple(currMini.multSprite, row, column);
            PlayAnim(ReactionType.happy);
            cartPopup.PlayGhostAnim(row, column);
        }
        else if (currMini.isLarge)
        {
            image.sprite = null;
            image.color = emptyAlpha;
        }
        else
        {
            // show ghost sprite
            image.sprite = currMini.miniSprite;
            image.color = ghostAlpha;

            cartPopup.CheckRelationship(row, column);
        }

    }

    public void DisableRayCast()
    {
        image.raycastTarget = false;
    }

    public void EnableRayCast()
    {
        image.raycastTarget = true;
    }

    public void DisableRayCastLarge()
    {
        imageLarge.raycastTarget = false;
    }

    public void EnableRayCastLarge()
    {
        imageLarge.raycastTarget = true;
    }
}
 