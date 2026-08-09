using TMPro;
using UnityEngine;

public class HeadlineObject : MonoBehaviour
{
    [SerializeField] private GameObject headlineObject;
    [SerializeField] private TextMeshProUGUI headlineText;

    public void SetText(float fontSize, string text)
    {
        AudioManager.instance.PlaySound(AudioManager.SoundNames.Ticket, true);
        headlineText.text = text;
        headlineText.fontSize = fontSize;
    }
}
