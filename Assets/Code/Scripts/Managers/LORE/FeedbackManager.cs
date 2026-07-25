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
public class CharacterFeedback
{
    public string[] text;
}

[Serializable]
public class FeedbackData
{
    public FeedbackNodeGeneric[] nodesGeneric;
    public CharacterFeedback ittyBitty;
    public CharacterFeedback protestorCatfish;
    public CharacterFeedback horseshoe;
    public CharacterFeedback isobelle;
    public CharacterFeedback seaStarDad;
    public CharacterFeedback granny;
    public CharacterFeedback gramps;
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
        if (constants.FEEDBACK_characterToDayToIdx[special].ContainsKey(day))
        {
            switch (special)
            {
                case Special.itty:
                    return feedbackData.ittyBitty.text[constants.FEEDBACK_characterToDayToIdx[special][day]];

                case Special.protestorCatfish:
                    return feedbackData.protestorCatfish.text[constants.FEEDBACK_characterToDayToIdx[special][day]];

                case Special.horseshoe:
                    return feedbackData.horseshoe.text[constants.FEEDBACK_characterToDayToIdx[special][day]];

                case Special.isobelle:
                    return feedbackData.isobelle.text[constants.FEEDBACK_characterToDayToIdx[special][day]];

                case Special.seaStarDad:
                    return feedbackData.seaStarDad.text[constants.FEEDBACK_characterToDayToIdx[special][day]];

                case Special.granny:
                    return feedbackData.granny.text[constants.FEEDBACK_characterToDayToIdx[special][day]];

                case Special.gramps:
                    return feedbackData.gramps.text[constants.FEEDBACK_characterToDayToIdx[special][day]];
            }
        }
        return "";
    }
}
