using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class FullStackLetter
{
    public string top;
    public string body;
    public string bottom;
}

[Serializable]
public class FullStackLetterByDay
{
    public int day;
    public string top;
    public string body;
    public string bottom;
}

[Serializable]
public class FullStackLetterByID
{
    public string id;
    public string top;
    public string body;
    public string bottom;
}

[Serializable]
public class LetterData
{
    public FullStackLetterByDay[] letterNodesCrustyCoByDay;
    public FullStackLetterByDay[] letterNodesCrustyCoByDay_GoodEnding;
    public FullStackLetterByDay[] letterNodesCrustyCoByDay_BadEnding;
    public FullStackLetterByID[] letterNodesCrustyCoByID;

    public FullStackLetter[] letterNodesFamily;
    public FullStackLetter[] letterNodesMailkeeper;

}

public class LettersManager : MonoBehaviour
{
    private LetterData letterData;


    private void Start()
    {
        LoadJson();
    }

    private void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Letters");

        if (jsonFile != null)
        {
            string jsonString = jsonFile.text;

            letterData = JsonUtility.FromJson<LetterData>(jsonString);
        }
        else
        {
            Debug.Log("file not found");
        }
    }

    public FullStackLetterByDay GetCrustyCoLetterByDay(int day, bool isEndingDependent = false, bool isGoodEnding = false)
    {
        if (isEndingDependent)
        {
            int idx = Constants.instance.LETTER_dayToIdxCrustyCoEndings[day];

            if (isGoodEnding)
            {
                return letterData.letterNodesCrustyCoByDay_GoodEnding[idx];
            }
            else
            {
                return letterData.letterNodesCrustyCoByDay_BadEnding[idx];
            }
        }

        int regIdx = Constants.instance.LETTER_dayToIdxCrustyCo[day];
        return letterData.letterNodesCrustyCoByDay[regIdx];
    }

    public FullStackLetterByID GetCrustyCoLetterByID(int id)
    {
        return letterData.letterNodesCrustyCoByID[id];
    }

    public FullStackLetter GetFamilyLetter(int day)
    {
        return letterData.letterNodesFamily[Constants.instance.LETTER_dayToIdxFamily[day]];
    }

    public FullStackLetter GetMailkeeperLetter(int day)
    {
        return letterData.letterNodesMailkeeper[Constants.instance.LETTER_dayToIdxMailkeeper[day]];
    }
}
