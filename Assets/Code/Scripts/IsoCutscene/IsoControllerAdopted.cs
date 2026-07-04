using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class IsoControllerAdopted : IsoController, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Sprite[] rolledIsoSprites;
    [SerializeField] private Sprite[] walkIsoSprites;
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
        int colorIdx = PlayerPrefs.GetInt("IsoColor");
        walkingSprite.GetComponent<Image>().sprite = walkIsoSprites[colorIdx];
        rollingSprite.GetComponent<Image>().sprite = rolledIsoSprites[colorIdx];

        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(WaitThenSwitchStates());
    }
}
