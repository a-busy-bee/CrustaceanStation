using UnityEngine;
using System;

[Serializable]
public class HeadlineData
{
    public string[] headlines;
    public string[] headlinesGood;
    public string[] headlinesBad;
    
}

//TODO: add node for specific events like "headline after seeing itty bitty for the first time"

public class HeadlineManager : MonoBehaviour
{
    public static HeadlineManager instance { get; private set; }
    private HeadlineData headlineData;
    [SerializeField] private HeadlineObject headlineObject;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        LoadJson();
    }

    private void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Headlines");

        if (jsonFile != null)
        {
            string jsonString = jsonFile.text;

            headlineData = JsonUtility.FromJson<HeadlineData>(jsonString);
        }
        else
        {
            Debug.Log("file not found");
        }
    }

    public void GetHeadline()
    {
        int currDay = SaveManager.instance.GetProgression_CurrDay();
        string key;

        if (currDay < 15) key = "headline_" + (currDay - 1);
        else if (PlotManager.instance.IsGoodEnding()) key = "headline_good_" + (currDay - 16);
        else key = "headline_bad_" + (currDay - 16);

        string headline = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Headlines, key);
        SetSpecificText(60, headline);
    }

    public void SetSpecificText(int fontSize, string text)
    {
        headlineObject.SetText(fontSize, text);
    }
}
