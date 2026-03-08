using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueObject : MonoBehaviour
{
    [SerializeField] private GameObject longDialogue;
    [SerializeField] private TextMeshProUGUI longDialogueText;

    [SerializeField] private GameObject shortDialogue;
    [SerializeField] private TextMeshProUGUI shortDialogueText;

    private float maxWidth = 527.42f;
    private float typingSpeed = 0.015f;
    private bool skip = false;

    private void Start()
    {
        longDialogue.SetActive(false);
        shortDialogue.SetActive(false);
    }
    public void ShowDialogue(string text)
    {
        shortDialogue.SetActive(true);
        shortDialogue.GetComponent<CanvasGroup>().alpha = 0;
        shortDialogueText.text = text;

        StartCoroutine(WaitAFrame(text));
    }

    IEnumerator WaitAFrame(string text)
    {
        yield return new WaitForEndOfFrame();
        if (shortDialogueText.gameObject.GetComponent<RectTransform>().rect.width > maxWidth)
        {
            longDialogue.SetActive(true);
            longDialogueText.text = text;
            StartCoroutine(TypeWrite(text, false));

            shortDialogueText.text = "";
            shortDialogue.SetActive(false);
        }
        else
        {
            StartCoroutine(TypeWrite(text, true));
            shortDialogue.GetComponent<CanvasGroup>().alpha = 1;
        }
    }

    IEnumerator TypeWrite(string text, bool isShort)
    {
        if (isShort)
        {
            shortDialogueText.maxVisibleCharacters = 0;

            while (shortDialogueText.maxVisibleCharacters < text.Length)
            {
                if (skip)
                {
                    shortDialogueText.maxVisibleCharacters = text.Length;
                    break;
                }
                shortDialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }

            shortDialogueText.maxVisibleCharacters = text.Length;
        }
        else
        {
            longDialogueText.maxVisibleCharacters = 0;

            while (longDialogueText.maxVisibleCharacters < text.Length)
            {
                if (skip)
                {
                    longDialogueText.maxVisibleCharacters = text.Length;
                    break;
                }
                longDialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }

            longDialogueText.maxVisibleCharacters = text.Length;
        }
    }

    public void ClearDialogue()
    {
        shortDialogueText.text = "";
        shortDialogue.SetActive(false);

        longDialogueText.text = "";
        longDialogue.SetActive(false);

        skip = false;
    }

    public void Skip()
    {
        if (skip)
        {
            ClearDialogue();
        }
        else
        {
            skip = true;
        }
    }

}
