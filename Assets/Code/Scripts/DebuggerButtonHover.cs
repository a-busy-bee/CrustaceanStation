using UnityEngine;
using UnityEngine.EventSystems;
public class DebuggerButtonHover : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover detected on " + name);
    }
}