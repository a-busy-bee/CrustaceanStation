using UnityEngine;

public class DeskManager : MonoBehaviour
{
    public int pg;
    public Decor decor;
    public void DisplayOptions(bool isTopisTopDecorOpen)
    {
        if (isTopisTopDecorOpen)
        {
            // hide options
            int item = 1;
            foreach (Transform opt in transform)
            {
                int index = item % 7 + (6 * (pg - 1));
                //Debug.Log(index);
                //Debug.Log(decor.items.Length);
                if (index >= decor.topDecor.Length)
                {
                    //Debug.Log("hide first step");
                    opt.gameObject.SetActive(false);
                }
                else
                {
                    //Debug.Log("show first step");
                    opt.gameObject.SetActive(true);
                }
                item++;
            }
        }
        else
        {
            // hide options
            int item = 1;
            foreach (Transform opt in transform)
            {
                int index = item % 7 + (6 * (pg - 1));
                //Debug.Log(index);
                //Debug.Log(decor.items.Length);
                if (index >= decor.items.Length)
                {
                    //Debug.Log("hide first step");
                    opt.gameObject.SetActive(false);
                }
                else
                {
                    //Debug.Log("show first step");
                    opt.gameObject.SetActive(true);
                }
                item++;
            }
        }


        // show the image sprites
        foreach (DisplayItem opt in GetComponentsInChildren<DisplayItem>())
        {
            opt.Display(pg, isTopisTopDecorOpen);
        }
    }

    public void DisplayKioskStyles()
    {
        int item = 1;

        foreach (Transform opt in transform)
        {
            int index = item % 4 + (3 * (pg - 1));

            if (index >= decor.kioskStyles.Length)
            {
                opt.gameObject.SetActive(false);
            }
            else
            {
                opt.gameObject.SetActive(true);
            }
            item++;
        }

        foreach (DisplayItem opt in GetComponentsInChildren<DisplayItem>())
        {
            opt.DisplayKioskStyleItem(pg);
        }
    }

}
 