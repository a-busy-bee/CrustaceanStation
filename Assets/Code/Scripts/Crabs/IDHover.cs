using UnityEngine;
using UnityEngine.EventSystems;

public class IDHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject magnifiedID;

    private void Start()
    {
        magnifiedID.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound(AudioManager.SoundNames.IdHover, true);
        magnifiedID.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound(AudioManager.SoundNames.IdHover, true);
        magnifiedID.SetActive(false);
    }

    public void ForceMagnifyOn()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundNames.IdHover, true);
        magnifiedID.SetActive(true);
    }

    public void ForceMagnifyOff()
    {
        AudioManager.instance.PlaySound(AudioManager.SoundNames.IdHover, true);
        magnifiedID.SetActive(false);
    }
}
