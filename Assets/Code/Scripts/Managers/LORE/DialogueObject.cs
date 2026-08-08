using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DialogueObject : MonoBehaviour
{
    [Header("Long Dialogue")]
    [SerializeField] private GameObject longDialogue;
    [SerializeField] private TextMeshProUGUI longDialogueText;
    [SerializeField] private Animator longDialogueAnimator;

    [Header("Short Dialogue")]
    [SerializeField] private GameObject shortDialogue;
    [SerializeField] private TextMeshProUGUI shortDialogueText;
    [SerializeField] private Animator shortDialogueAnimator;

    private Coroutine shortDialogueHideRoutine;
    private Coroutine longDialogueHideRoutine;
    private Coroutine waitAFrameRoutine;
    private Coroutine typewriteRoutine;

    private float maxWidth = 527.42f;
    private float typingSpeed = 0.015f;
    private string currText = "";

    public enum DialogueState
    {
        NotAppeared,
        Appearing,
        Typing,
        Idle,
        Disappearing
    }
    private DialogueState dialogueState = DialogueState.NotAppeared;
    private void Start()
    {
        longDialogue.SetActive(false);
        shortDialogue.SetActive(false);
    }

    public DialogueState GetDialogueState()
    {
        return dialogueState;
    }

    public void SetState(DialogueState newState)
    {
        DialogueState prevState = dialogueState;
        dialogueState = newState;

        switch (dialogueState)
        {
            case DialogueState.NotAppeared:
                if (shortDialogueHideRoutine != null) StopCoroutine(shortDialogueHideRoutine);
                if (longDialogueHideRoutine != null) StopCoroutine(longDialogueHideRoutine);
                if (waitAFrameRoutine != null) StopCoroutine(waitAFrameRoutine);
                if (typewriteRoutine != null) StopCoroutine(typewriteRoutine);
                shortDialogue.SetActive(false);
                longDialogue.SetActive(false);

                break;

            case DialogueState.Appearing:
                AudioManager.instance.PlaySound(AudioManager.SoundNames.PopupSmall, true);
                if (shortDialogueHideRoutine != null) StopCoroutine(shortDialogueHideRoutine);
                if (longDialogueHideRoutine != null) StopCoroutine(longDialogueHideRoutine);
                if (waitAFrameRoutine != null) StopCoroutine(waitAFrameRoutine);
                if (typewriteRoutine != null) StopCoroutine(typewriteRoutine);

                if (shortDialogue.activeInHierarchy) shortDialogueAnimator.ResetTrigger("Hide");
                if (longDialogue.activeInHierarchy) longDialogueAnimator.ResetTrigger("Hide");

                shortDialogue.SetActive(false);
                longDialogue.SetActive(false);
                waitAFrameRoutine = StartCoroutine(WaitAFrame());
                break;

            case DialogueState.Typing:
                
                break;

            case DialogueState.Idle:
                if (longDialogue.activeSelf)
                {
                    longDialogueText.maxVisibleCharacters = currText.Length;
                    longDialogue.GetComponent<CanvasGroup>().alpha = 1;
                    //longDialogueAnimator.Play("Blob");
                }
                else if (shortDialogue.activeSelf)
                {
                    shortDialogueText.maxVisibleCharacters = currText.Length;
                    shortDialogue.GetComponent<CanvasGroup>().alpha = 1;
                    //shortDialogueAnimator.Play("Blob");
                }

                break;

            case DialogueState.Disappearing:
                AudioManager.instance.PlaySound(AudioManager.SoundNames.PopupClose, true);
                if (waitAFrameRoutine != null) StopCoroutine(waitAFrameRoutine);
                if (typewriteRoutine != null) StopCoroutine(typewriteRoutine);

                if (shortDialogue.activeSelf)
                {
                    //shortDialogueText.text = "";
                    //shortDialogue.SetActive(false);
                    shortDialogueAnimator.SetTrigger("Hide");
                    shortDialogueHideRoutine = StartCoroutine(WaitForAnimAndHide(shortDialogue, shortDialogueAnimator, shortDialogueText));
                }
                else if (longDialogue.activeSelf)
                {
                    //longDialogueText.text = "";
                    //longDialogue.SetActive(false);
                    longDialogueAnimator.SetTrigger("Hide");
                    longDialogueHideRoutine = StartCoroutine(WaitForAnimAndHide(longDialogue, longDialogueAnimator, longDialogueText));
                }

                break;
        }
    }

    public void ShowDialogue(string text)
    {
        currText = text;
        SetState(DialogueState.Appearing);
    }

    public void ClearDialogue()
    {
        SetState(DialogueState.Disappearing);
    }

    IEnumerator WaitAFrame()
    {
        //yield return new WaitForEndOfFrame();

        shortDialogue.SetActive(true);
        //shortDialogue.GetComponent<CanvasGroup>().alpha = 0;
        shortDialogueText.text = currText;

        yield return new WaitForEndOfFrame();

        //AudioManager.instance.PlaySound(AudioManager.SoundNames.PopupSmall, true);
        
        if (shortDialogueText.gameObject.GetComponent<RectTransform>().rect.width > maxWidth)
        {
            longDialogue.SetActive(true);
            longDialogueText.text = currText;
            longDialogueAnimator.Play("CalloutAppear", 0, 0f);

            typewriteRoutine = StartCoroutine(TypeWrite(false));

            shortDialogueText.text = "";
            shortDialogue.SetActive(false);
        }
        else
        {
            Debug.Log("short");
            //shortDialogue.GetComponent<CanvasGroup>().alpha = 1;
            shortDialogueAnimator.Play("CalloutAppear", 0, 0f);
            typewriteRoutine = StartCoroutine(TypeWrite(true));
        }
        
        SetState(DialogueState.Typing);
    }

    IEnumerator TypeWrite(bool isShort)
    {
        if (isShort)
        {
            shortDialogueText.maxVisibleCharacters = 0;
            while (shortDialogueText.maxVisibleCharacters < currText.Length)
            {
                shortDialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        else
        {
            longDialogueText.maxVisibleCharacters = 0;

            while (longDialogueText.maxVisibleCharacters < currText.Length)
            {
                longDialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        SetState(DialogueState.Idle);
    }

    IEnumerator WaitForAnimAndHide(GameObject dialogueObj, Animator animator, TextMeshProUGUI text)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("CalloutHide")) yield return null;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length / 2.0f);
        animator.ResetTrigger("Hide");
        text.text = "";
        dialogueObj.SetActive(false);
        
        SetState(DialogueState.NotAppeared);
    }

    public void Skip()
    {
        if (dialogueState == DialogueState.Typing)
        {
            if (typewriteRoutine != null) StopCoroutine(typewriteRoutine);
            SetState(DialogueState.Idle);
        }
        else if (dialogueState == DialogueState.Idle)
        {
            SetState(DialogueState.Disappearing);
        }
    }

}
