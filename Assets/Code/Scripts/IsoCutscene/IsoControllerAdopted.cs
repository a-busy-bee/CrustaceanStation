using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class IsoControllerAdopted : IsoController, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    protected override void Awake()
    {
        SetColor();

        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(WaitThenSwitchStates());
    }

    private void SetColor()
    {
        Color isoColor = Color.white;

        string hex = "#" + PlayerPrefs.GetString("IsoColor");
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            isoColor = color;
        }

        rollingSprite.GetComponent<Image>().color = isoColor;
        walkingSprite.GetComponent<Image>().color = isoColor;
    }
}
