using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ID : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    private string crabName;

    [SerializeField] private Image image;
    [SerializeField] private Image magnifyImage;
    [SerializeField] private CanvasGroup hover;
    private Sprite sprite;

    [SerializeField] private GameObject blur;
    [SerializeField] private RectTransform rectTransform;
    private Ticket ticket;

    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Image seatTypeImage; // x1 vs x2
    [SerializeField] private Image seatTypeImageMagnify;
    [SerializeField] private Sprite[] seatTypeSprites;
    public enum SeatType
    {
        singleSeat,
        doubleSeat
    }
    private SeatType seatType;

    public void SetSeatType(SeatType newSeatType)
    {
        seatType = newSeatType;

        Sprite sprite = seatTypeSprites[(int)seatType];
        seatTypeImage.sprite = sprite;
        seatTypeImageMagnify.sprite = sprite;
    }

    public void SetName(string newName)
    {
        crabName = newName;
        nameText.text = crabName;
    }

    public void SetIDPhoto(Sprite newSprite)
    {
        sprite = newSprite;
        image.sprite = sprite;
        magnifyImage.sprite = sprite;
    }

    public void SetTicket(Ticket newTicket)
    {
        ticket = newTicket;
    }

    public void PushBack()
    {

        rectTransform.rotation = Quaternion.Euler(0, 0, -28.8f);
        rectTransform.anchoredPosition = new Vector3(161, 203, 0);

        // remove blur
        blur.SetActive(true);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        hover.interactable = false;
        hover.blocksRaycasts = false;
    }

    public void BringForward()
    {
        // rotate
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);

        // move position
        rectTransform.anchoredPosition = new Vector3(130.77f, 193.7f, 44.85f);

        // remove blur
        blur.SetActive(false);
        rectTransform.SetAsLastSibling();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        hover.interactable = true;
        hover.blocksRaycasts = true;

        ticket.PushBack();
        AudioManager.instance.PlaySound(AudioManager.SoundNames.Ticket, true);
    }
}
