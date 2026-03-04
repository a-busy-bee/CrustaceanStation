using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class Switch : MonoBehaviour, IPointerClickHandler
{
    // when clicked, report to Rail to depart train

    private Rail rail;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void SetRail(Rail newRail)
    {
        rail = newRail;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        Debug.Log("click");
        animator.enabled = true;
        animator.Play("SwitchOn");

        //play switch animation
        // depart train
        StartCoroutine(WaitThenReset());
    }

    IEnumerator WaitThenReset()
    {
        yield return new WaitForSeconds(0.15f);
        rail.DepartTrain();

        yield return new WaitForSeconds(2);
        animator.Play("SwitchOff");
    }
}
