using UnityEngine;
using System;
using System.Collections.Generic;

using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

using Special = CrabInfo.SpecialCharacter;

[Serializable]
public class FeedbackNodeGeneric
{
    public string text;
}

[Serializable]
public class FeedbackData
{
    public FeedbackNodeGeneric[] nodesGeneric;
    public string[] ittyBitty;
    public string[] protestorCatfish;
    public string[] horseshoe;
    public string[] isobelle;
    public string[] seaStarDad;
    public string[] granny;
    public string[] gramps;
}

public class FeedbackManager : MonoBehaviour
{
    private FeedbackData feedbackData;
    private string currIdx;
    private string currText;
    private bool isSubscribed;

    private void Start()
    {
        LoadJson();
        LocalizationSettings.SelectedLocaleChanged += RefreshCurrDialogue;
        isSubscribed = true;
        currIdx = "";
    }

    private void OnDisable()
    {
        if (!isSubscribed) return;
        LocalizationSettings.SelectedLocaleChanged -= RefreshCurrDialogue;
        isSubscribed = false;
    }

    private void OnDestroy()
    {
        if (!isSubscribed) return;
        LocalizationSettings.SelectedLocaleChanged -= RefreshCurrDialogue;
        isSubscribed = false;
    }

    private void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Feedback");

        if (jsonFile != null)
        {
            string jsonString = jsonFile.text;

            feedbackData = JsonUtility.FromJson<FeedbackData>(jsonString);
        }
        else
        {
            Debug.Log("file not found");
        }
    }

    public string GetGenericFeedback()
    {
        int rand = UnityEngine.Random.Range(0, feedbackData.nodesGeneric.Length);
        currIdx = "feedback_nodesGeneric_" + rand;
        return LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.FeedbackForms, currIdx);
    }

    public string GetCharacterFeedback(Special special, int day)
    {
        Constants constants = Constants.instance;
        int idx = constants.FEEDBACK_characterToDayToIdx[special][day];
        string prefix = "feedback_nodesGeneric_";
        switch (special)
        {
            case Special.itty:
                prefix = "feedback_ittyBitty_";
                break;
            //return feedbackData.ittyBitty[constants.FEEDBACK_characterToDayToIdx[special][day]];

            case Special.protestorCatfish:
                prefix = "feedback_protestorCatfish_";
                break;
            //return feedbackData.protestorCatfish[constants.FEEDBACK_characterToDayToIdx[special][day]];

            case Special.horseshoe:
                prefix = "feedback_horseshoe_";
                break;
            //return feedbackData.horseshoe[constants.FEEDBACK_characterToDayToIdx[special][day]];

            case Special.isobelle:
                prefix = "feedback_isobelle_";
                break;
            //return feedbackData.isobelle[constants.FEEDBACK_characterToDayToIdx[special][day]];

            case Special.seaStarDad:
                prefix = "feedback_seaStarDad_";
                break;
            //return feedbackData.seaStarDad[constants.FEEDBACK_characterToDayToIdx[special][day]];

            case Special.granny:
                prefix = "feedback_granny_";
                break;
            //return feedbackData.granny[constants.FEEDBACK_characterToDayToIdx[special][day]];

            case Special.gramps:
                prefix = "feedback_gramps_";
                break;
                //return feedbackData.gramps[constants.FEEDBACK_characterToDayToIdx[special][day]];
        }

        currIdx = prefix + idx;
        return LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.FeedbackForms, currIdx);

    }

    public void ResetIdx()
    {
        currIdx = "";
    }

    public void RefreshCurrDialogue(Locale l)
    {
        if (currIdx == "") return;
        currText = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.FeedbackForms, currIdx);
    }

    public string GetFeedbackFormLocalized()
    {
        return currText;
    }
}
