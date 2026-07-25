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
public class LetterData
{
    public string[] letterNodesCrustyCo;
    public string[] letterNodesCrustyCoAnyDay;

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
        if (Constants.instance.LETTER_dayToIdxCrustyCo.ContainsKey(day))
            return letterData.letterNodesCrustyCo[Constants.instance.LETTER_dayToIdxCrustyCo[day]];
        return "";
    }

    public string GetCrustyCoLetterByIdx(int idx)
    {
        return letterData.letterNodesCrustyCoAnyDay[idx];
    }

    public FullStackLetter GetFamilyLetter(int day)
    {
        if (Constants.instance.LETTER_dayToIdxFamily.ContainsKey(day))
            return letterData.letterNodesFamily[Constants.instance.LETTER_dayToIdxFamily[day]];
        return null;
    }

    public FullStackLetter GetMailkeeperLetter(int day)
    {
        if (Constants.instance.LETTER_dayToIdxMailkeeper.ContainsKey(day))
            return letterData.letterNodesMailkeeper[Constants.instance.LETTER_dayToIdxMailkeeper[day]];
        return null;
    }
}
