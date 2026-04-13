using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Mailbox : MonoBehaviour
{
    [SerializeField] private GameObject notif;

    [Header("Tray Sprite")]
    [SerializeField] private Sprite[] trayFullness;
    [SerializeField] private Image trayImage;

    private int remainingMail = 0;

    private void Awake()
    {
        SetInteractable(false);
    }
    private void Start()
    {
        notif.SetActive(false);
        trayImage.sprite = trayFullness[0];                             // empty
    }

    public void SetSprite(int remainingLetters)
    {
        remainingMail = remainingLetters;

        if (remainingLetters == 0)
        {
            notif.SetActive(false);
            trayImage.sprite = trayFullness[0];
            SetInteractable(false);
        }
        else
        {
            notif.SetActive(true);

            if (remainingLetters < 2) trayImage.sprite = trayFullness[1];  // sorta full
            else trayImage.sprite = trayFullness[2];                    // really full

            SetInteractable(true);
        }
    }

    public void SetInteractable(bool interactable)
    {
        if (remainingMail == 0) GetComponent<Button>().interactable = false; 

        GetComponent<Button>().interactable = interactable;
    }

    public void OnSummonLetter()
    {
        if (remainingMail == 0) return;

        SetInteractable(false);
        MailroomManager.instance.SummonLetter();
    }
}
