using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VetManager : MonoBehaviour
{
    [SerializeField] private GameObject returnHomeButton;
    [SerializeField] private Image isoImage;
    [SerializeField] private Emotion isoEmotion;
    [SerializeField] private GameObject vetBill;
    [SerializeField] private GameObject vet;

    [Header("Iso Colors")]
    [SerializeField] private Color[] isoColors;

    private void Start()
    {
        returnHomeButton.SetActive(false);

        Color isoColor = Color.white;

        string hex = "#" + PlayerPrefs.GetString("IsoColor");
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            isoColor = color;
        }
        isoImage.color = isoColor;

        vet.GetComponent<SmoothLerp>().Move(new Vector2(-421, 370), 0.25f);
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
