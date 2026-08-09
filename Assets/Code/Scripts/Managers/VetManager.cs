using UnityEngine;
using UnityEngine.UI;

public class VetManager : MonoBehaviour
{
    //[SerializeField] private GameObject returnHomeButton;
    //[SerializeField] private GameObject goodEndingButton;
    //[SerializeField] private GameObject badEndingButton;
    [SerializeField] private GameObject vet;
    [SerializeField] private GameObject medicationBottle;
    private int currDay;

    [Header("Iso")]
    [SerializeField] private Sprite[] rollingSprites;
    [SerializeField] private Sprite[] walkingSprites;
    [SerializeField] private Image isoWalkSprite;
    [SerializeField] private Image isoRollSprite;
    [SerializeField] private Emotion isoEmotion;

    private void Start()
    {
        currDay = SaveManager.instance.GetProgression_CurrDay();
        currDay = 5;
        if (currDay / 5 == 1) CutscenePlayer_Dialogue.instance.SetDialogueType(CutscenePlayer_Dialogue.DialogueType.vet1);
        else if (currDay / 5 == 2) CutscenePlayer_Dialogue.instance.SetDialogueType(CutscenePlayer_Dialogue.DialogueType.vet2);
        else if (currDay / 5 == 3) CutscenePlayer_Dialogue.instance.SetDialogueType(CutscenePlayer_Dialogue.DialogueType.vet3);
        else if (currDay / 5 == 4)
        {
            if (PlotManager.instance.IsGoodEnding())
            {
                CutscenePlayer_Dialogue.instance.SetNextScene(CutscenePlayer_Dialogue.NextScene.EndingGood);
                CutscenePlayer_Dialogue.instance.SetDialogueType(CutscenePlayer_Dialogue.DialogueType.vet4good);
            }
            else
            {
                CutscenePlayer_Dialogue.instance.SetNextScene(CutscenePlayer_Dialogue.NextScene.EndingBad);
                CutscenePlayer_Dialogue.instance.SetDialogueType(CutscenePlayer_Dialogue.DialogueType.vet4bad);
            }
        }

        medicationBottle.SetActive(false);

        int colorIdx = SaveManager.instance.GetIso_Color();
        isoWalkSprite.GetComponent<Image>().sprite = walkingSprites[colorIdx];
        isoRollSprite.GetComponent<Image>().sprite = rollingSprites[colorIdx];
    }

    /*private IEnumerator WaitThenContinue()
    {
        yield return new WaitForSeconds(0.5f);

        // show dialogue
        //DialogueManager.instance.GetDialogueVet();

        /*yield return new WaitForSeconds(1f);

        
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
        
    }*/

    /*public void ReturnHome()
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
    }*/

    public void ClickMedBottle()
    {
        //todo: shake the bottle
        //todo: play a rattle noise
        Debug.Log("drugs go rattle rattle");
    }
}
