using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Food : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private Animator hoverAnimator;
    [SerializeField] private GameObject particlesObject;
    [SerializeField] private Animator particlesObjectAnimator;
    private Animator animator;
    private bool active = false;

    private void Start()
    {
        hoverAnimator.enabled = false;
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (active) { return; }
        hoverAnimator.enabled = true;
        hoverAnimator.Play("OnHover");
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (active) { return; }
        hoverAnimator.enabled = true;
        //hoverAnimator.Play("StopHover");
        StartCoroutine(LiftBottleAndParticles());
    }

    private IEnumerator LiftBottleAndParticles()
    {
        active = true;
        animator.Play("Lift");
        yield return new WaitForSeconds(1f);

        particlesObject.SetActive(true);
        particlesObjectAnimator.Play("Particles");
        yield return new WaitForSeconds(1f);

        animator.Play("PutDown");
        yield return new WaitForSeconds(0.2f);
        active = false;
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (active) { return; }
        hoverAnimator.enabled = true;
        hoverAnimator.Play("StopHover");
    }
}
