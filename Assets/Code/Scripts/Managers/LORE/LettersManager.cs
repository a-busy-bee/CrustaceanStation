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
 
    public string GetCrustyCoLetterByDay(int day)
    {
        return letterData.letterNodesCrustyCo[Constants.instance.LETTER_dayToIdxCrustyCo[day]];
    }

    public string GetCrustyCoLetterByIdx(int idx)
    {
        return letterData.letterNodesCrustyCoAnyDay[idx];
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
