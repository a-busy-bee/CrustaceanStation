using UnityEngine;
using UnityEngine.EventSystems;

public class MailKeeper : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform rectTransform;
    private bool isMoving = false;
    private Vector3 endPos = new Vector3(537, 118, 0);
    private Vector3 currentVelocity;
    private Animator animator;


    [Header("Tapped")]
    [SerializeField] private GameObject coin;
    private bool isTapped = false;

    private void Awake()
    {
        isMoving = true;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isMoving)
        {

            rectTransform.anchoredPosition = Vector3.SmoothDamp(rectTransform.anchoredPosition, endPos, ref currentVelocity, 0.25f);

            if (Vector2.Distance(rectTransform.anchoredPosition, endPos) < 0.1f)
            {
                isMoving = false;
                rectTransform.anchoredPosition = endPos;

            }


        }
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
