using UnityEngine;
using System;

[Serializable]
public class FeedbackData
{
    public FeedbackNodeGeneric[] nodesGeneric;
    public FeedbackNodePlot[] nodesPlot;
}

[Serializable]
public class FeedbackNodeGeneric
{
    public string name;
    public string text;
}


[Serializable]
public class FeedbackNodePlot
{
    public int plotID;
    public string[] name;
    public string[] text;
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

    public (string, string) GetGenericFeedback()
    {
        int rand = UnityEngine.Random.Range(0, feedbackData.nodesGeneric.Length);
        return (feedbackData.nodesGeneric[rand].name, feedbackData.nodesGeneric[rand].text);
    }

    public (string, string) GetPlotFeedback(int stage)
    {
        int rand = UnityEngine.Random.Range(0, feedbackData.nodesPlot[stage].text.Length);

        string name = "";
        if (rand < feedbackData.nodesPlot[stage].name.Length) name = feedbackData.nodesPlot[stage].name[rand];

        return (name, feedbackData.nodesPlot[stage].text[rand]);
    }
}
