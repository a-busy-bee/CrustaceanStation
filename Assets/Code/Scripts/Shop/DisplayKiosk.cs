using UnityEngine;
using UnityEngine.UI;

public class DisplayKiosk : MonoBehaviour
{
    private GameObject target;
    public Decor decor;

    // update the kiosk slot item
    public void DisplayDecoItem(int slot, int index)
    { 
        Debug.Log(transform.GetChild(slot).gameObject);
        target = transform.GetChild(slot).gameObject;
        target.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = decor.items[index].sprite;
    }

    public void DisplayKioskStyle()
    { 
    
    }
}
 