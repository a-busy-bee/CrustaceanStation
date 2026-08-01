using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Linq;
using System.IO;

[Serializable]
public class InboxItem
{
    public string type;         // letter, research note, feedback form
    public string subType;      // crustyCo, bioDiv, etc
    public int id;              // index in array in respective json (ie letter.crustyCo[id])
    public int timestamp;       // when it was summoned in-game
    public string templateID;   // what type of letter is it? TODO: could prolly be removed
    public bool isRead;         // whether player read this in the mail room already
}

[Serializable]
public class PlotData
{
    public List<InboxItem> inbox;
    public int nextTimeStamp;
}

public class PlotManager : MonoBehaviour
{
    public static PlotManager instance { get; private set; }
    public enum Stage
    {
        week1,
        week2,
        week3,
        week4
    }

    private Stage currStage;
    private float crust = 1.0f;
    private float endingThreshold = 0.75f;

    //JSON STUFF
    private PlotData plotData;
    private string defaultPath;
    private string savePath;

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
        LoadJSON();
    }

    public int GetCurrStageInt()
    {
        return (int)currStage;
    }
    public Stage GetCurrStage()
    {
        return currStage;
    }

    private void LoadJSON()
    {
        defaultPath = Application.dataPath + "/Data/Inbox.json";
        savePath = Application.persistentDataPath + "/Inbox.json";

        if (File.Exists(savePath))
        {
            plotData = JsonUtility.FromJson<PlotData>(File.ReadAllText(savePath));
        }
        else
        {
            string jsonText = File.ReadAllText(defaultPath);
            plotData = JsonUtility.FromJson<PlotData>(jsonText);
            File.WriteAllText(savePath, JsonUtility.ToJson(plotData, true));
        }

        // Ensure inbox list is never null after loading
        if (plotData.inbox == null)
            plotData.inbox = new List<InboxItem>();
    }

    public void AddMail(string newType, string newSubtype = "", int newId = 0)
    {
        InboxItem newItem = new InboxItem
        {
            type = newType,
            subType = newSubtype,
            id = newId,
            timestamp = plotData.nextTimeStamp,
            templateID = newType,
            isRead = false
        };

        plotData.nextTimeStamp++;
        plotData.inbox.Add(newItem);
        SaveData();
    }

    private void SaveData()
    {
        File.WriteAllText(savePath, JsonUtility.ToJson(plotData, true));
    }

    public bool IsGoodEnding()
    {
        return crust > endingThreshold; 
    }
}
