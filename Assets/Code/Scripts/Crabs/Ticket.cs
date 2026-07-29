using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Ticket : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI nameText;
    protected string crabName;

    [SerializeField] protected GameObject blur;
    [SerializeField] protected RectTransform rectTransform;
    protected ID id;

    // RAIL DIRECTION
    //[SerializeField] private GameObject direction;
    //private float[] zRotations;
    //private Rail.RailDirection railDirection;

    // SPRITES
    [SerializeField] protected Sprite[] ticketSprites;
    [SerializeField] protected Image ticketImg;
    [SerializeField] protected Image blurImg;

    [SerializeField] protected CanvasGroup canvasGroup;

    public void SetName(string newName)
    {
        crabName = newName;
        nameText.text = crabName;
    }

    /*public void SetTrainDirection(Rail.RailDirection newDirection)
    {
        railDirection = newDirection;

        direction.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, zRotations[(int)railDirection]);
    }
    public Rail.RailDirection GetRailDirection()
    {
        return railDirection;
    }*/

    public void SetID(ID newID)
    {
        id = newID;
    }

    /*public Rail.RailDirection GetRandomTrainID()
    {
        return (Rail.RailDirection)Random.Range(0, 4);
    }*/


    public void PushBack()
    {
        rectTransform.rotation = Quaternion.Euler(0, 0, -28.8f);
        rectTransform.anchoredPosition = new Vector3(76, 85.798f, 64);

        // remove blur
        blur.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    virtual public void BringForward()
    {
        //Debug.Log("clicked shuttle ticket");
        // rotate
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);

        // move position
        rectTransform.anchoredPosition = new Vector3(100, 80, 0);

        // remove blur
        blur.SetActive(false);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        rectTransform.SetAsLastSibling();

        id.PushBack();
        AudioManager.instance.PlaySound(AudioManager.SoundNames.Ticket, true);
    }
    
    public void SetSprite(Cart.Type cartType)
    {
        if (cartType == Cart.Type.Economy)
        {
            ticketImg.sprite = ticketSprites[0];
            blurImg.sprite = ticketSprites[0];
        }
        else if (cartType == Cart.Type.Standard)
        {
            ticketImg.sprite = ticketSprites[1];
            blurImg.sprite = ticketSprites[1];
        }
        else if (cartType == Cart.Type.Deluxe)
        {
            ticketImg.sprite = ticketSprites[2];
            blurImg.sprite = ticketSprites[2];
        }
    }
}
