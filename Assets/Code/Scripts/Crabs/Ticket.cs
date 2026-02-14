using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Ticket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    private string crabName;

    [SerializeField] private GameObject blur;
    [SerializeField] private RectTransform rectTransform;
    private ID id;

    // RAIL DIRECTION
    [SerializeField] private GameObject direction;
    private float[] zRotations;
    private Rail.RailDirection railDirection;

    // SPRITES
    [SerializeField] private Sprite[] ticketSprites;
    [SerializeField] private Image ticketImg;
    [SerializeField] private Image blurImg;

    private void Awake()
    {
        zRotations = new float[] { 0, 180, 270, 90 };
    }

    public void SetName(string newName)
    {
        crabName = newName;
        nameText.text = crabName;
    }

    public void SetTrainDirection(Rail.RailDirection newDirection)
    {
        railDirection = newDirection;

        direction.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, zRotations[(int)railDirection]);
    }
    public Rail.RailDirection GetRailDirection()
    {
        return railDirection;
    }

    public void SetID(ID newID)
    {
        id = newID;
    }

    public Rail.RailDirection GetRandomTrainID()
    {
        return (Rail.RailDirection)Random.Range(0, 4);
    }



    public void PushBack()
    {
        rectTransform.rotation = Quaternion.Euler(0, 0, -28.8f);

        // move position
        rectTransform.anchoredPosition = new Vector3(-445, -419.2f, 0);

        // remove blur
        blur.SetActive(true);
    }

    public void BringForward()
    {
        // rotate
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);

        // move position
        rectTransform.anchoredPosition = new Vector3(-384, -450.8f, 44.85482f);

        // remove blur
        blur.SetActive(false);
        rectTransform.SetAsLastSibling();

        id.PushBack();
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
