using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItem : MonoBehaviour
{
    public Decor decor;
    public Shop shop;
    public int pg;
    public int item;
    public int index;

    public TextMeshProUGUI price;
    public Image priceCoin;
    public Image sprite;
    public TextMeshProUGUI kioskStyleName;

    bool topDecorOpen = false;

    public void Display(int pg, bool isTopDecorOpen)
    {
        topDecorOpen = isTopDecorOpen;
        index = item % 7 + (6 * (pg - 1));
        //Debug.Log(index);
        //Debug.Log(decor.items.Length);

        if (topDecorOpen)
        {
            DisplayCostAndSprite(decor.topDecor);
        }
        else
        {
            DisplayCostAndSprite(decor.items);
        }
    }

    public void DisplayKioskStyleItem(int pg)
    {
        index = item % 4 + (3 * (pg - 1));

        KioskStyle[] styles = decor.kioskStyles;

        // show price if haven't bought yet
        if (!styles[index].bought)
        {
            price.text = styles[index].cost.ToString();
            priceCoin.enabled = true;
        }
        else
        {
            price.text = "";
            priceCoin.enabled = false;
        }

        kioskStyleName.text = styles[index].styleName;
    }

    private void DisplayCostAndSprite(DecorItems[] decorItems)
    {
        if (decorItems[index].isRemoveItem)
        {
            price.text = "";
            priceCoin.enabled = false;
        }
        else
        {
            // show price if haven't bought yet
            if (!decorItems[index].bought)
            {
                price.text = decorItems[index].cost.ToString();
                priceCoin.enabled = true;
            }
            else
            {
                price.text = "";
                priceCoin.enabled = false;
            }
        }

        sprite.sprite = decorItems[index].iconSprite;
    }

    public void KioskUpgrade() // to be implemented
    {
        SelectKioskStyle(decor.kioskStyles[index]);
    }

    private void SelectKioskStyle(KioskStyle style)
    {
        if (style.bought)
        {
            decor.setKioskStyle(index);
        }

        else
        {
            if (PlayerPrefs.GetInt("coins") >= style.cost)
            {
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - style.cost);
                shop.coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
                style.bought = true;
                price.text = "";
                priceCoin.enabled = false;
                decor.setKioskStyle(index);
            }
        }
    }

    // set the deco slot to the selected item if bought, otherwise check if we can buy -> set the deco slot, change player pref
    public void DeskBuy()
    {
        if (topDecorOpen)
        {
            Select(decor.topDecor[index]);
        }
        else
        {
            Select(decor.items[index]);
        }
    }

    private void Select(DecorItems item)
    {
        if (item.bought)
        {
            decor.setDecoSlotItem(index);
        }
        else
        {
            if (PlayerPrefs.GetInt("coins") >= item.cost)
            {
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - item.cost);
                shop.coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
                item.bought = true;
                price.text = "";
                priceCoin.enabled = false;
                decor.setDecoSlotItem(index);
            }
        }
    }
    
}