using UnityEngine;
using System;

[Serializable]
public class HeadlineData
{
    public string[] nodesGeneric;
    public HeadlineNodePlot[] nodesPlot;
}

[Serializable]
public class HeadlineNodePlot
{
    public int id;
    public string[] text;
}

//TODO: add node for specific events like "headline after seeing itty bitty for the first time"

public class HeadlineManager : MonoBehaviour
{
    private HeadlineData headlineData;
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

    public string GetGenericHeadline()
    {
        return headlineData.nodesGeneric[UnityEngine.Random.Range(0, headlineData.nodesGeneric.Length)];
    }

    public string GetPlotHeadline(PlotManager.Stage stage)
    {
        return headlineData.nodesPlot[(int)stage].text[UnityEngine.Random.Range(0, headlineData.nodesPlot[(int)stage].text.Length)];
    }
}
