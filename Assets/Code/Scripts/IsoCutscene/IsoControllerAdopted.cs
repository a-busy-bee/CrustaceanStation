using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class IsoControllerAdopted : IsoController, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Sprite[] rolledIsoSprites;
    [SerializeField] private Sprite[] walkIsoSprites;

    protected new Vector2 startingPos = new Vector2(70, -281);


    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    private void Start()
    {
        int colorIdx = SaveManager.instance.GetIso_Color();
        walkingSprite.GetComponent<Image>().sprite = walkIsoSprites[colorIdx];
        rollingSprite.GetComponent<Image>().sprite = rolledIsoSprites[colorIdx];

        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(WaitThenSwitchStates());
    }

    protected override float Move() // helper func for Roll and Walk
    {
        float targetX = Random.Range(-429, 612);
        float targetY = Random.Range(-297, -281);

        targetPos = new Vector2(targetX, targetY);
        currPos = rectTransform.anchoredPosition;
        currProgress = 0f;

        return targetX; // return targetX to determine which direction to flip/roll
    }
}
