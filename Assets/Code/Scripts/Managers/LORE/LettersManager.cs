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

    public enum IDToKey
    {
        firstRed,
        secondRed,
        day3NoEaten,
        day3Eaten
    }

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

    public FullStackLetter GetCrustyCoLetterByDay(int day, bool isEndingDependent = false, bool isGoodEnding = false)
    {
        // if (isEndingDependent)
        // {
        //     int idx = Constants.instance.LETTER_dayToIdxCrustyCoEndings[day];

        //     if (isGoodEnding)
        //     {
        //         return letterData.letterNodesCrustyCoByDay_GoodEnding[idx];
        //     }
        //     else
        //     {
        //         return letterData.letterNodesCrustyCoByDay_BadEnding[idx];
        //     }
        // }

        // int regIdx = Constants.instance.LETTER_dayToIdxCrustyCo[day];
        // return letterData.letterNodesCrustyCoByDay[regIdx];

        string prefix = "letter_crustyco";

        if (isEndingDependent)
        {
            if (isGoodEnding)
            {
                prefix += "Good_day_" + day;
                return GetFullStackLetter(prefix);
            }
            else
            {
                prefix += "Bad_day_" + day;
                return GetFullStackLetter(prefix);
            }
        }

        prefix += "_day_" + day;
        return GetFullStackLetter(prefix);
    }

    public FullStackLetter GetCrustyCoLetterByID(int id)
    {
        string key = "letter_crustyco_id_" + (IDToKey)id;
        return GetFullStackLetter(key);
    }

    public FullStackLetter GetFamilyLetter(int day)
    {
        int idx = Constants.instance.LETTER_dayToIdxFamily[day];
        string prefix = "letter_family_" + idx;

        return GetFullStackLetter(prefix);
    }

    public FullStackLetter GetMailkeeperLetter(int day)
    {
        int idx = Constants.instance.LETTER_dayToIdxMailkeeper[day];
        string prefix = "letter_mailkeeper_" + idx;

        return GetFullStackLetter(prefix);
    }

    private FullStackLetter GetFullStackLetter(string prefix)
    {
        FullStackLetter fullStackLetter = new FullStackLetter();
        fullStackLetter.top = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Letter, prefix + "_top");
        fullStackLetter.body = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Letter, prefix + "_body");
        fullStackLetter.bottom = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Letter, prefix + "_bottom");

        return fullStackLetter;
    }
}
