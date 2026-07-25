using UnityEngine;
using System;
using System.Collections.Generic;

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

    private void Start()
    {
        LoadJson();
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
        return feedbackData.nodesGeneric[rand].text;
    }

    public string GetCharacterFeedback(Special special, int day)
    {
        Constants constants = Constants.instance;
        switch (special)
        {
            case Special.itty:
                return feedbackData.ittyBitty[constants.FEEDBACK_characterToDayToIdx[special][day]];

            case Special.protestorCatfish:
                return feedbackData.protestorCatfish[constants.FEEDBACK_characterToDayToIdx[special][day]];

            case Special.horseshoe:
                return feedbackData.horseshoe[constants.FEEDBACK_characterToDayToIdx[special][day]];

            case Special.isobelle:
                return feedbackData.isobelle[constants.FEEDBACK_characterToDayToIdx[special][day]];

            case Special.seaStarDad:
                return feedbackData.seaStarDad[constants.FEEDBACK_characterToDayToIdx[special][day]];

            case Special.granny:
                return feedbackData.granny[constants.FEEDBACK_characterToDayToIdx[special][day]];

            case Special.gramps:
                return feedbackData.gramps[constants.FEEDBACK_characterToDayToIdx[special][day]];
        }

        return "Get Character Feedback";
    }
        
}
