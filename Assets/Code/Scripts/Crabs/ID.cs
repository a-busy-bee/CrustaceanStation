using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ID : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    private string crabName;

    [SerializeField] private Image image;
    private Sprite sprite;

    [SerializeField] private GameObject blur;
    [SerializeField] private RectTransform rectTransform;
    private Ticket ticket;

    [SerializeField] private CanvasGroup canvasGroup;

    public void SetName(string newName)
    {
        crabName = newName;
        nameText.text = crabName;
    }

    public void SetIDPhoto(Sprite newSprite)
    {
        sprite = newSprite;
        image.sprite = sprite;
    }

    public void SetTicket(Ticket newTicket)
    {
        ticket = newTicket;
    }

    public void PushBack()
    {
        rectTransform.rotation = Quaternion.Euler(0, 0, -28.8f);

        // move position
        rectTransform.anchoredPosition = new Vector3(-363, -335, 64);

        // remove blur
        blur.SetActive(true);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

    }

    public void BringForward()
    {
        // rotate
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);

        // move position
        rectTransform.anchoredPosition = new Vector3(-412.23f, -361.2999f, 44.85482f);

        // remove blur
        blur.SetActive(false);
        rectTransform.SetAsLastSibling();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        ticket.PushBack();
    }
}
