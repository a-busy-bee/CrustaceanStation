using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class Switch : MonoBehaviour, IPointerClickHandler
{
    // when clicked, report to Rail to depart train

    private TransportPath path;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void SetPath(TransportPath newPath)
    {
        path = newPath;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (TutorialManager.instance.GetIsTutorial() && TutorialManager.instance.GetCurrTutorialState() < Tutorial.TutorialState.trainSwitch) return;
        Debug.Log("click");
        animator.enabled = true;
        animator.Play("SwitchOn");

        if (TutorialManager.instance.GetIsTutorial()) TutorialManager.instance.ProgressTutorial();

        //play switch animation
        // depart train
        StartCoroutine(WaitThenReset());
    }

    IEnumerator WaitThenReset()
    {
        yield return new WaitForSeconds(0.15f);
        path.Depart();

        yield return new WaitForSeconds(2);
        animator.Play("SwitchOff");
    }
}
