using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VetManager : MonoBehaviour
{
    public static VetManager instance { get; private set; }
    [SerializeField] private GameObject vet;
    [SerializeField] private GameObject medicationBottle;
    private int currDay;

    [Header("Iso")]
    [SerializeField] private Sprite[] rollingSprites;
    [SerializeField] private Sprite[] walkingSprites;
    [SerializeField] private Image isoWalkSprite;
    [SerializeField] private Image isoRollSprite;
    [SerializeField] private Emotion isoEmotion;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
    }
	private void Start()
    {
        currDay = SaveManager.instance.GetProgression_CurrDay();
        Debug.Log(currDay);
        if (currDay / 5 == 0) CutscenePlayer_Dialogue.instance.SetDialogueType(CutscenePlayer_Dialogue.DialogueType.vet1);
        else if (currDay / 5 == 1) CutscenePlayer_Dialogue.instance.SetDialogueType(CutscenePlayer_Dialogue.DialogueType.vet2);
        else if (currDay / 5 == 2) CutscenePlayer_Dialogue.instance.SetDialogueType(CutscenePlayer_Dialogue.DialogueType.vet3);
        else if (currDay / 5 == 3)
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

    public void ShowMeds()
    {
        medicationBottle.SetActive(true);
        SaveManager.instance.SaveProgressionData(SaveManager.ProgressionType.medsAvailable, "true");
    }

    public void PlayIsoSad()
    {
        isoEmotion.PlaySad();
    }

   
    public void ClickMedBottle()
    {
        //todo: shake the bottle
        //todo: play a rattle noise
        Debug.Log("drugs go rattle rattle");
    }
}
