using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverSFX : MonoBehaviour, IPointerEnterHandler
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }
    public void OnPointerEnter(PointerEventData data)
    {
        if (AudioManager.instance != null && button.interactable)
        {
            AudioManager.instance.PlaySound(AudioManager.SoundNames.Click, true);
        }
    }
}
