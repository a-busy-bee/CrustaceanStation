using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VetManager : MonoBehaviour
{
    [SerializeField] private GameObject returnHomeButton;
    [SerializeField] private GameObject goodEndingButton;
    [SerializeField] private GameObject badEndingButton;
    [SerializeField] private GameObject vet;
    [SerializeField] private GameObject medicationBottle;

    [Header("Iso")]
    [SerializeField] private Sprite[] rollingSprites;
    [SerializeField] private Sprite[] walkingSprites;
    [SerializeField] private Image isoWalkSprite;
    [SerializeField] private Image isoRollSprite;
    [SerializeField] private Emotion isoEmotion;

    private void Start()
    {
        medicationBottle.SetActive(false);
        returnHomeButton.SetActive(false);
        goodEndingButton.SetActive(false);
        badEndingButton.SetActive(false);

        int colorIdx = SaveManager.instance.GetIso_Color();
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

        yield return new WaitForSeconds(1f);

        int currDay = SaveManager.instance.GetProgression_CurrDay();
        if (currDay / 5 == 1)
        {
            medicationBottle.SetActive(true);
            SaveManager.instance.SaveProgressionData(SaveManager.ProgressionType.medsAvailable, "true");
        }

        yield return new WaitForSeconds(0.5f);


        if (currDay / 5 != 3) // no emotion to play IF HE'S DEAD ;-;
        {
            // iso emotion
            isoEmotion.PlaySad();
        }

        // show vet bill
        //vetBill.GetComponent<SmoothLerp>().Move(new Vector2(), 0.25f);
        yield return new WaitForSeconds(0.5f);

        // show return button

        if (currDay < 20)
        {
            returnHomeButton.SetActive(true);
        }
        else if (PlotManager.instance.IsGoodEnding())
        {
            goodEndingButton.SetActive(true);
        }
        else
        {
            badEndingButton.SetActive(true);
        }
        
    }

    public void ReturnHome()
    {
        SaveManager.instance.SetProgression_IncrementCurrDay();
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneManager.LoadScene("Home");
    }

    public void GoodEnding()
    {
        AchievementManager.instance.UnlockAchievementBool(AchievementManager.AchievementTypeBool.soLong);
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneManager.LoadScene("EndingGood");
    }

    public void BadEnding()
    {
        AchievementManager.instance.UnlockAchievementBool(AchievementManager.AchievementTypeBool.whatHaveIDone);
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneManager.LoadScene("EndingBad");
    }

    public void ClickMedBottle()
    {
        //todo: shake the bottle
        //todo: play a rattle noise
        Debug.Log("drugs go rattle rattle");
    }
}
