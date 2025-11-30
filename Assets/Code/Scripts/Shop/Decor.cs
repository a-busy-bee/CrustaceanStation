using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Decor : MonoBehaviour
{
    public GameObject KioskStyle, DeskItems, Next, Prev;
    public DeskManager desk;
    public DisplayKiosk displayKiosk;
    // I'm not exactly sure on how to save whether an item is bought or not across sessions without using a bunch of player.prefs...
    public DecorItems[] items;
    
    // kiosk stuff? idk if player can switch kiosk styles, or if it's just upwards upgrading


    public int decoItem1;
    public int decoItem2;
    public int kioskType;

    public int pg = 1;
    // max pages for each type of item page
    private int kioskPg = 1; 
    private int deskPg = 2;
    private int currDecoSlot;
    private enum ItemType
    { 
        Kiosk,
        DeskItems
    }
    private ItemType type;

    [SerializeField] private bool debug;

    void Start()
    {
        // reset values
        if (debug) 
        {
            PlayerPrefs.SetInt("decoItem1", 0);
            PlayerPrefs.SetInt("decoItem2", 0);
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
        decoItem1 = PlayerPrefs.GetInt("decoItem1");
        decoItem2 = PlayerPrefs.GetInt("decoItem2");
        kioskType = PlayerPrefs.GetInt("kioskType");
        UpgradeKiosk();
    }

    public void UpgradeKiosk()   
    {
        KioskStyle.SetActive(true);
        DeskItems.SetActive(false);
        type = ItemType.Kiosk;
        pg = 1;
        DisplayPage(pg);
    }
    
    // keep track of which slot we are on
    public void selectDeco1() 
    {
        currDecoSlot = 1; // left
        DeskDeco();
    }

    public void selectDeco2()
    {
        currDecoSlot = 2; // right
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

        if (type == ItemType.Kiosk) // kiosk upgrade not implemented yet
        {
            DisplayNextPrev(pg, kioskPg);
            desk.pg = pg;
        }
        else if (type == ItemType.DeskItems) // max 2 pages rn
        {
            // display next/prev depending on pg, change the button images and prices
            DisplayNextPrev(pg, deskPg);
            desk.pg = pg;
            desk.DisplayOptions();
            
        
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
        
    }

    // change the item placed in the slot on the kiosk
    public void setDecoSlotItem(int index)
    {
        if (currDecoSlot == 1)
        {
            PlayerPrefs.SetInt("decoItem1", index);
            displayKiosk.DisplayDecoItem(currDecoSlot, index);
        }
        else if (currDecoSlot == 2)
        {
            PlayerPrefs.SetInt("decoItem2", index);
            displayKiosk.DisplayDecoItem(currDecoSlot, index);
        }
        
    }
}
