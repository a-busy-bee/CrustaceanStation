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

    public void Display(int pg)
    {
        index = (item % 7 + (6 * (pg-1)));
        //Debug.Log(index);
        //Debug.Log(decor.items.Length);

        // show price if haven't bought yet
        if (!decor.items[index].bought)
        {
            price.text = decor.items[index].cost.ToString();
            priceCoin.enabled = true;
        }
        else 
        {
            price.text = "";
            priceCoin.enabled = false;
        }
        sprite.sprite = decor.items[index].sprite;
        
    }
 
    public void KioskUpgrade() // to be implemented
    { }

    // set the deco slot to the selected item if bought, otherwise check if we can buy -> set the deco slot, change player pref
    public void DeskBuy()
    { 
        DecorItems item = decor.items[index];
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