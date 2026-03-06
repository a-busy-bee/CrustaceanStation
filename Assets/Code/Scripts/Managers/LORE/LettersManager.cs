using UnityEngine;
using System;

[Serializable]
public class LetterData
{
    public string[] letterNodesCrustyCo;
    public string[] letterNodesBioCo;
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

    public string GetCrustyCoLetter(int id)
    {
        return letterData.letterNodesCrustyCo[id];
    }

    public string GetBioCoLetter(int id)
    {
        return letterData.letterNodesBioCo[id];
    }
}
