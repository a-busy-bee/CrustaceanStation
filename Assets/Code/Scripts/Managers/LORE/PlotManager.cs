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
        day1,           // crusty corp letter on desk, tutorial
        headlines,      // introduce headlines & performance monitor
        mailroom,       // introduce feedback box & mailroom, add causal dialogue
        onlySPrey,      // crusty corp letter -> send all S prey
        allNonCrust,    // crusty corp letter -> send all non-crustaceans, see first mutation same day (first customer?)
        involuntary,    // after mutation, letter from crusty co -> involuntary shuttle
        shuttleActive,  // letter from biodiv co -> freedom van
        alarmActive,    // next day, letter from crusty co -> freedom van alarm, start tracking crusty vs bio floating pt variables
        bigEventCrust,  // send crustaceans that don't look crab-enough
        bigEventBio     // quit job to work for biodiv corporation
    }

    private Stage currStage;
    private int crabsSinceLastLevelUp = 0;
    private Dictionary<Stage, int> crabsNeededToAdvance;
    private float crust = 1.0f;
    private float bio = 1.0f;

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

    /*private void LoadJSON()
    {
        defaultPath = Application.dataPath + "/Data/Inbox.json";
        savePath = Application.persistentDataPath + "/Inbox.json";

        if (File.Exists(savePath))
        {
            plotData = JsonUtility.FromJson<PlotData>(File.ReadAllText(savePath));
        }
        else
        {
            // load default file from resources
            string jsonText = File.ReadAllText(defaultPath);
            plotData = JsonUtility.FromJson<PlotData>(jsonText);

            File.WriteAllText(savePath, JsonUtility.ToJson(plotData, true));
        }
    }*/

    public int GetCurrStageInt()
    {
        return (int)currStage;
    }
    public Stage GetCurrStage()
    {
        return currStage;
    }

    //PlayerPrefs.GetInt("currStage")
    //PlayerPrefs.GetInt("crabsSinceLevelUp")
    //PlayerPrefs.GetInt("crustCo")
    //PlayerPrefs.GetInt("bioCo)

    /*public void AddMail(string newType, string newSubtype = "", int newId = 0)
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

        plotData.nextTimeStamp = plotData.nextTimeStamp + 1;
        plotData.inbox.Append(newItem);

        SaveData();
    }

    private void SaveData()
    {
        File.WriteAllText(savePath, JsonUtility.ToJson(plotData, true));
    }*/

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
}
