using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapTransition : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject blur;
    [SerializeField] private MapManager mapManager;

	private void Start()
    {
        blur.SetActive(false);
    }

	public void OnPointerEnter(PointerEventData eventData)
    {
        blur.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        blur.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // drop down map
        mapManager.Dropdown();
    }
}
