using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UI;
using Unity.VisualScripting;
using NUnit.Framework.Interfaces;

public class Decor : MonoBehaviour
{
    public GameObject KioskStyle, DeskItems, Next, Prev;
    public DeskManager desk;
    public DeskManager deskKiosk;
    public DisplayKiosk displayKiosk;
    // I'm not exactly sure on how to save whether an item is bought or not across sessions without using a bunch of player.prefs...
    public DecorItems[] items;
    public DecorItems[] topDecor;
    public KioskStyle[] kioskStyles;
    [SerializeField] private GameObject startingText;
    

    // kiosk stuff? idk if player can switch kiosk styles, or if it's just upwards upgrading


    // Saved slot item (if any)
    public int decor_left;
    public int decor_right;
    public int decor_top;
    public int kioskType;

    private enum SlotType
    {
        left,
        right,
        top,
        kiosk
    }
    private SlotType currentSlot;

    private bool isTopDecorOpen = false;

    public int pg = 1;
    // max pages for each type of item page
    private int kioskPg = 1; 
    private int deskPg = 2;
    //private int currDecoSlot;

    private GameObject currSelector;
    private enum ItemType
    {
        Kiosk,
        DeskItems
    }
    private ItemType type;

    [SerializeField] private bool debug;

    private void Awake()
    {
        //PlayerPrefs.SetInt("coins", 1000);
        // TODO: if reset in settings, reset playerprefs & scriptable objects (like crabdex)
        if (PlayerPrefs.GetInt("ResetDecor") == 1)
        {
            PlayerPrefs.SetInt("ResetDecor", 0);

            foreach (DecorItems item in items)
            {
                if (item != null)
                {
                    item.bought = false;
                }
            }

            foreach (DecorItems item in topDecor)
            {
                if (item != null)
                {
                    item.bought = false;
                }
            }
        }
    }


    void Start()
    {
        // reset values
        if (debug)
        {
            PlayerPrefs.SetInt("decor_left", 0);
            PlayerPrefs.SetInt("decor_right", 0);
            PlayerPrefs.SetInt("decor_top", 0);
            PlayerPrefs.SetInt("kioskType", 0);
            foreach (DecorItems item in items)
            {
                // the first elem is the default/no item, which is null for now
                if (item != null)
                {
                    item.bought = false;
                }

            }
        }
        // player prefs
        decor_left = PlayerPrefs.GetInt("decor_left");
        decor_right = PlayerPrefs.GetInt("decor_right");
        decor_top = PlayerPrefs.GetInt("decor_top");
        kioskType = PlayerPrefs.GetInt("kioskType");

        KioskStyle.SetActive(false);
        DeskItems.SetActive(false);
        startingText.SetActive(true);
    }

	public void Reset()
	{
        DisplayNextPrev(1, 1);
		KioskStyle.SetActive(false);
        DeskItems.SetActive(false);
        startingText.SetActive(true);
	}

	private void OnDisable()
	{
		if (currSelector != null)
        {
            currSelector.SetActive(false);
            currSelector = null;
        }
	}

	public void UpgradeKiosk()
    {
        startingText.SetActive(false);

        KioskStyle.SetActive(true);
        DeskItems.SetActive(false);
        type = ItemType.Kiosk;
        pg = 1;
        DisplayPage(pg);

        currentSlot = SlotType.kiosk;
        currSelector.SetActive(false);

    }

    public void SelectKioskStyle(int idx)
    {
        displayKiosk.DisplayKioskStyle(idx);
    }

    // keep track of which slot we are on
    public void selectDecor_Left(GameObject selector)
    {
        if (currSelector != null) currSelector.SetActive(false);
        currSelector = selector;
        currSelector.SetActive(true);
        startingText.SetActive(false);
        isTopDecorOpen = false;

        currentSlot = SlotType.left; // left
        DeskDeco();
    }

    public void selectDecor_Right(GameObject selector)
    {
        if (currSelector != null) currSelector.SetActive(false);
        currSelector = selector;
        currSelector.SetActive(true);
        startingText.SetActive(false);
        isTopDecorOpen = false;

        currentSlot = SlotType.right; // right
        DeskDeco();
    }

    public void selectDecor_Top(GameObject selector)
    {
        if (currSelector != null) currSelector.SetActive(false);
        currSelector = selector;
        currSelector.SetActive(true);
        startingText.SetActive(false);
        isTopDecorOpen = true;

        currentSlot = SlotType.top; // top
        DeskDeco();
    }

    // display the desk items page
    void DeskDeco() 
    {
        KioskStyle.SetActive(false);
        DeskItems.SetActive(true);

        type = ItemType.DeskItems;
        pg = 1;
        DisplayPage(pg);
    }

    public void NextPg()
    {
        pg++;
        DisplayPage(pg);
    }
    public void PrevPg() 
    { 
        pg--;
        DisplayPage(pg);
    }
    void DisplayPage(int pg)
    {
        if (isTopDecorOpen)
        {
            deskPg = 1 + (topDecor.Length - 1) / 6;
        }
        else
        {
            deskPg = 1 + (items.Length - 1) / 6;
        }

        if (type == ItemType.Kiosk) // kiosk upgrade not implemented yet
        {
            //kioskPg = 1 + (kioskStyles.Length - 1) / 3;
            DisplayNextPrev(pg, kioskPg);
            desk.pg = pg;
            deskKiosk.DisplayKioskStyles();
        }
        else if (type == ItemType.DeskItems) // max 2 pages rn
        {
            // display next/prev depending on pg, change the button images and prices
            DisplayNextPrev(pg, deskPg);
            desk.pg = pg;
            desk.DisplayOptions(isTopDecorOpen);
        }
    }

    // controls display of the next/prev page arrows
    void DisplayNextPrev(int currPg, int maxPg) 
    {
        if (maxPg == 1)
        {
            Next.SetActive(false);
            Prev.SetActive(false);
        }
        else if (currPg == 1)
        {
            Next.SetActive(true);
            Prev.SetActive(false);
        }
        else if (currPg == maxPg)
        {
            Next.SetActive(false);
            Prev.SetActive(true);
        }
        else
        {
            Next.SetActive(true);
            Prev.SetActive(true);
        }
    }

    public void setKioskStyle(int index) // to be implemented
    {
        if (currSelector != null)
        {
            currSelector.SetActive(false);
            currSelector = null;
        }
        PlayerPrefs.SetInt("kioskStyle", index);
        displayKiosk.DisplayKioskStyle(index);
    }

    // change the item placed in the slot on the kiosk
    public void setDecoSlotItem(int index)
    {
        if (currentSlot == SlotType.top)
        {
            PlayerPrefs.SetInt("decor_top", index);
            displayKiosk.DisplayDecoItem(3, index);
        }
        else if (currentSlot == SlotType.left)
        {
            PlayerPrefs.SetInt("decor_left", index);
            displayKiosk.DisplayDecoItem(1, index);

        }
        else if (currentSlot == SlotType.right)
        {
            PlayerPrefs.SetInt("decor_right", index);
            displayKiosk.DisplayDecoItem(2, index);
        }
    }
}
