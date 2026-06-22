using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class IsoCaught : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Emotion emotion;

    public void OnPointerEnter(PointerEventData eventData)
    {
        emotion.PlayHappy();
    }

    public void GetAdopted()
    {
        emotion.PlayHappy();
    }

    public void GetReleased()
    {
        emotion.PlaySad();
    }
    
    //TODO: idle anim & SFX?
}
