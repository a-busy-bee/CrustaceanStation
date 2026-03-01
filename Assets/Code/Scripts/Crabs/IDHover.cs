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
        magnifiedID.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        magnifiedID.SetActive(false);
    }
}
