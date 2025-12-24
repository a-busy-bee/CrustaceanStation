using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shopkeeper : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Shop shop;
    [SerializeField] private GameObject wares;
    [SerializeField] private RectTransform rectTransform;
    private bool isMoving = false;
    private Vector3 endPos = new Vector3(470.9427f, -31.97021f, 0);
    private Vector3 currentVelocity;
    private bool presented = false;
    private Animator animator;


    [Header("Tapped")]
    [SerializeField] private GameObject coin;
    private bool isTapped = false;

    private void Awake()
    {
        isMoving = true;
        presented = false;
        wares.SetActive(false);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isMoving)
        {

            rectTransform.anchoredPosition = Vector3.SmoothDamp(rectTransform.anchoredPosition, endPos, ref currentVelocity, 0.25f);

            if (Vector2.Distance(rectTransform.anchoredPosition, endPos) < 10f && !presented)
            {
                presented = true;
                PresentWares();
            }
            else if (Vector2.Distance(rectTransform.anchoredPosition, endPos) < 0.1f)
            {
                isMoving = false;
                rectTransform.anchoredPosition = endPos;

            }


        }
    }

    private void PresentWares()
    {
        if (shop.GetCurrentMenu() != Shop.shopMenu.shopMain) return;
        //show all buttons
        wares.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTapped) return;

        isTapped = true;

        coin.SetActive(true);
        coin.GetComponent<Coin>().Clicked();
        
        animator.SetTrigger("tapped");
    }
}
