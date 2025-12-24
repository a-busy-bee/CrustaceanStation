using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayKiosk : MonoBehaviour
{
    private GameObject target;
    public Decor decor;

    [Header("Empty")]
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private GameObject leftSlotEmpty;
    [SerializeField] private GameObject rightSlotEmpty;
    [SerializeField] private GameObject topSlotEmpty;

    [Header("Slot")]
    [SerializeField] private GameObject leftSlot;
    [SerializeField] private GameObject rightSlot;
    [SerializeField] private GameObject topSlot;
    [SerializeField] private GameObject kiosk;
    [SerializeField] private GameObject kioskChildren;
    
    private void Start()
    {
        int top = PlayerPrefs.GetInt("decor_top");
        int left = PlayerPrefs.GetInt("decor_left");
        int right = PlayerPrefs.GetInt("decor_right");

        if (top > 1)
        {
            topSlotEmpty.SetActive(false);
            topSlot.SetActive(true);
            topSlot.GetComponent<Image>().sprite = decor.topDecor[top].sprite;
        }
        else
        {
            topSlot.SetActive(false);
            topSlotEmpty.SetActive(true);
        }

        if (left > 1)
        {
            leftSlotEmpty.SetActive(false);
            leftSlot.SetActive(true);
            leftSlot.GetComponent<Image>().sprite = decor.items[left].sprite;
        }
        else
        {
            leftSlot.SetActive(false);
            leftSlotEmpty.SetActive(true);
        }

        if (right > 1)
        {
            rightSlotEmpty.SetActive(false);
            rightSlot.SetActive(true);
            rightSlot.GetComponent<Image>().sprite = decor.items[right].sprite;
        }
        else
        {
            rightSlot.SetActive(false);
            rightSlotEmpty.SetActive(true);
        }

    }



    // update the kiosk slot item
    public void DisplayDecoItem(int slot, int index) // slot = 1 (left), 2 (right), 3 (top)
    {
        DecorItems[] decorItems = decor.items;
        if (slot == 3)
        {
            decorItems = decor.topDecor;
        }

        // If the slot is empty
        if (index == 1)
        {
            if (slot == 1)
            {
                leftSlot.SetActive(false);
                leftSlotEmpty.SetActive(true);
            }
            else if (slot == 2)
            {
                rightSlot.SetActive(false);
                rightSlotEmpty.SetActive(true);
            }
            else if (slot == 3)
            {
                topSlot.SetActive(false);
                topSlotEmpty.SetActive(true);
            }
        }

        else
        {
            // set sprites otherwise
            if (slot == 1)
            {
                leftSlotEmpty.SetActive(false);
                leftSlot.SetActive(true);
                leftSlot.GetComponent<Image>().sprite = decorItems[index].sprite;
            }
            else if (slot == 2)
            {
                rightSlotEmpty.SetActive(false);
                rightSlot.SetActive(true);
                rightSlot.GetComponent<Image>().sprite = decorItems[index].sprite;
            }
            else if (slot == 3)
            {
                topSlotEmpty.SetActive(false);
                topSlot.SetActive(true);
                topSlot.GetComponent<Image>().sprite = decorItems[index].sprite;
            }

        }

        //target = transform.GetChild(slot).gameObject;

    }

    public void DisplayKioskStyle(int idx)
    {
        KioskStyle style = decor.kioskStyles[idx];
        kiosk.GetComponent<Image>().sprite = style.sprite;
        kiosk.GetComponent<RectTransform>().anchoredPosition = new Vector3(style.shopPosition.x, style.shopPosition.y, 0);
        kioskChildren.GetComponent<RectTransform>().anchoredPosition = new Vector3(style.shopPositionChildren.x, style.shopPositionChildren.y, 0);
    }
}
 