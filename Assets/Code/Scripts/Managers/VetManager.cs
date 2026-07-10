using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VetManager : MonoBehaviour
{
    [SerializeField] private GameObject returnHomeButton;
    [SerializeField] private GameObject vetBill;
    [SerializeField] private GameObject vet;

    [Header("Iso")]
    [SerializeField] private Sprite[] rollingSprites;
    [SerializeField] private Sprite[] walkingSprites;
    [SerializeField] private Image isoWalkSprite;
    [SerializeField] private Image isoRollSprite;
    [SerializeField] private Emotion isoEmotion;

    private void Start()
    {
        returnHomeButton.SetActive(false);

        int colorIdx = PlayerPrefs.GetInt("IsoColor");
        isoWalkSprite.GetComponent<Image>().sprite = walkingSprites[colorIdx];
        isoRollSprite.GetComponent<Image>().sprite = rollingSprites[colorIdx];

        vet.GetComponent<SmoothLerp>().Move(new Vector2(-421, 127), 0.25f);
        StartCoroutine(WaitThenContinue());
    }

    private IEnumerator WaitThenContinue()
    {
        yield return new WaitForSeconds(0.5f);

        // show dialogue
        DialogueManager.instance.GetDialogueVet();
        yield return new WaitForSeconds(1.5f);

        if (PlayerPrefs.GetInt("CurrDay") / 5 != 3) // no emotion to play IF HE'S DEAD ;-;
        {
            // iso emotion
            isoEmotion.PlaySad();
        }
        

        // show vet bill
        //vetBill.GetComponent<SmoothLerp>().Move(new Vector2(), 0.25f);
        yield return new WaitForSeconds(0.5f);
        
        // show return button
        returnHomeButton.SetActive(true);
    }

    public void ReturnHome()
    {
        PlayerPrefs.SetInt("CurrDay", PlayerPrefs.GetInt("CurrDay") + 1);
        SceneManager.LoadScene("Home");
    }
}
