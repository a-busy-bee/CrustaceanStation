using UnityEngine;
using UnityEngine.EventSystems;

public class Food : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private Animator hoverAnimator;
    [SerializeField] private GameObject particlesObject;
    [SerializeField] private Animator particlesObjectAnimator;

    private void Start()
    {
        hoverAnimator.enabled = false;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        hoverAnimator.enabled = true;
        hoverAnimator.Play("OnHover");
    }

    public void OnPointerClick(PointerEventData data)
    {
        hoverAnimator.enabled = true;
        hoverAnimator.Play("StopHover");
        particlesObject.SetActive(true);
        particlesObjectAnimator.Play("Particles");
        Debug.Log("clicked");
    }

    public void OnPointerExit(PointerEventData data)
    {
        hoverAnimator.enabled = true;
        hoverAnimator.Play("StopHover");
    }
}
