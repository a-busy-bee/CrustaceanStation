using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class SettingsData
{
    public string language;     // TODO: to be implemented

    // SCREEN SIZE
    public int resolutionX;
    public int resolutionY;

    // OTHER
    public float volume_Master;
    public float volume_SFX;
    public float volume_Music;
    public float brightness;    // TODO: to be implemented
    public bool reduceMotion;   // TODO: to be implemented


}

[Serializable]
public class IsoData
{
    public string isoName;
    public string isoBirthdayMonth;
    public int isoBirthdayDay;
    public int isoColor;
}

[Serializable]
public class Character
{
    public string characterName;
    public int dialogueIdx;
    public bool isDone;
}

[Serializable]
public class CharactersData
{
    public Character[] characters;
}

[Serializable]
public class ProgressionData
{
    public int currDay;
    public int tutorialState;

    // shuttling
    public int numNonCrustiesShuttled;
    public int numNonCrustiesSeen;


    public float performanceBarPercent;
    public bool performanceBarSaved;
    public bool introMailSeen;
    public bool firstDayHeadlineSeen;
    public bool newGame;
    public bool medsAvailable;
    public bool eatenBeforeDayThree;
    public bool redOne;
}

[Serializable]
public class Data
{
    public SettingsData settings;
    public IsoData isoData;
    public CharactersData charactersData;
    public ProgressionData progressionData;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }

    public enum SettingsType
    {
        language,
        volumeMaster,
        volumeSFX,
        volumeMusic,
        brightness,
        reduceMotion,
        screenResX,
        screenResY
    }

    public enum ProgressionType
    {
        currDay,
        performanceBarSaved,
        tutorialState,
        performanceBarPercent,
        introMailSeen,
        firstDayHeadlineSeen,
        newGame,
        medsAvailable,
        eatenBeforeDay3,
        red1,
        nonCrustySeen,
        nonCrustyShuttled
    }

    private Dictionary<string, int> characterNameToIndex = new Dictionary<string, int>
    {
        { "protestorCatfish", 0 },
        { "horseshoe", 1 },
        { "isobelle", 2 },
        { "itty", 3 },
        { "seaStarDad", 4 },
        { "gramps", 5 },
        { "granny", 6 }
    };

    //JSON STUFF 
    private Data data;
    private string defaultPath;
    private string savePath;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        LoadJSON();
    }
    private void LoadJSON()
    {
        defaultPath = Path.Combine(Application.streamingAssetsPath, "Data", "Data.json");
        savePath = Path.Combine(Application.persistentDataPath, "Data.json");

        if (File.Exists(savePath))
        {
            data = JsonUtility.FromJson<Data>(File.ReadAllText(savePath));
        }
        else
        {
            string jsonText = File.ReadAllText(defaultPath);
            data = JsonUtility.FromJson<Data>(jsonText);
            File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
        }
    }

    #region Settings
    public void SaveSettings(SettingsType settingsType, string value)
    {
        switch (settingsType)
        {
            case SettingsType.language:
                data.settings.language = value;
                break;

            case SettingsType.screenResX:
                data.settings.resolutionX = int.Parse(value);
                break;

            case SettingsType.screenResY:
                data.settings.resolutionY = int.Parse(value);
                break;    

            case SettingsType.volumeMaster:
                data.settings.volume_Master = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                break;

            case SettingsType.volumeSFX:
                data.settings.volume_SFX = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                break;

            case SettingsType.volumeMusic:
                data.settings.volume_Music = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                break;

            case SettingsType.brightness:
                data.settings.brightness = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                break;

            case SettingsType.reduceMotion:
                data.settings.reduceMotion = bool.Parse(value);
                break;
        }

        SaveData();
    }
    public string GetSettings_Language()
    {
        return data.settings.language;
    }
    public int GetSettings_ResolutionX()
    {
        return data.settings.resolutionX;
    }
    public int GetSettings_ResolutionY()
    {
        return data.settings.resolutionY;
    }
    public float GetSettings_VolumeMaster()
    {
        return data.settings.volume_Master;
    }
    public float GetSettings_VolumeSFX()
    {
        return data.settings.volume_SFX;
    }
    public float GetSettings_VolumeMusic()
    {
        return data.settings.volume_Music;
    }
    public float GetSettings_Brightness()
    {
        return data.settings.brightness;
    }
    public bool GetSettings_ReduceMotion()
    {
        return data.settings.reduceMotion;
    }
    #endregion

    #region Iso

    public void SaveIsoData(string name, string month, int day, int color)
    {
        data.isoData.isoName = name;
        data.isoData.isoBirthdayMonth = month;
        data.isoData.isoBirthdayDay = day;
        data.isoData.isoColor = color;

        SaveData();
    }
    public string GetIso_Name()
    {
        return data.isoData.isoName;
    }
    public string GetIso_BirthdayMonth()
    {
        return data.isoData.isoBirthdayMonth;
    }
    public int GetIso_Birthday()
    {
        return data.isoData.isoBirthdayDay;
    }
    public int GetIso_Color()
    {
        return data.isoData.isoColor;
    }
    #endregion

    #region Characters
    public void SaveCharacterData(string name, int dialogue, bool isDone = false)
    {
        int idx = characterNameToIndex[name];

        data.charactersData.characters[idx].dialogueIdx = dialogue;
        data.charactersData.characters[idx].isDone = isDone;

        if (!AchievementManager.instance.IsBoolAchievementUnlocked(AchievementManager.AchievementTypeBool.networking))
        {
            int numSpecials = data.charactersData.characters.Length;
            bool seenAll = true;
            for (int i = 0; i < numSpecials; i++)
            {
                if (data.charactersData.characters[i].dialogueIdx == 0) seenAll = false;
            }

            if (seenAll)
            {
                AchievementManager.instance.UnlockAchievementBool(AchievementManager.AchievementTypeBool.networking);
            }
        }
 
        SaveData();
    }
    public int GetCharacter_DialogueIdx(string name)
    {
        int idx = characterNameToIndex[name];

        return data.charactersData.characters[idx].dialogueIdx;
    }
    public bool GetCharacter_Done(string name)
    {
        int idx = characterNameToIndex[name];

        return data.charactersData.characters[idx].isDone;
    }
    #endregion

    #region Progression
    public void SaveProgressionData(ProgressionType progressionType, string value)
    {
        switch (progressionType)
        {
            case ProgressionType.currDay:
                data.progressionData.currDay = int.Parse(value);

                break;

            case ProgressionType.tutorialState:
                data.progressionData.tutorialState = int.Parse(value);
                break;

            case ProgressionType.performanceBarPercent:
                data.progressionData.performanceBarPercent = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                break;

            case ProgressionType.performanceBarSaved:
                data.progressionData.performanceBarSaved = bool.Parse(value);
                break;

            case ProgressionType.introMailSeen:
                data.progressionData.introMailSeen = bool.Parse(value);
                break;

            case ProgressionType.firstDayHeadlineSeen:
                data.progressionData.firstDayHeadlineSeen = bool.Parse(value);
                break;

            case ProgressionType.newGame:
                data.progressionData.newGame = bool.Parse(value);
                break;

            case ProgressionType.medsAvailable:
                data.progressionData.medsAvailable = bool.Parse(value);
                break;

            case ProgressionType.eatenBeforeDay3:
                data.progressionData.eatenBeforeDayThree = bool.Parse(value);
                break;

            case ProgressionType.red1:
                data.progressionData.redOne = bool.Parse(value);
                break;

            case ProgressionType.nonCrustySeen:
                data.progressionData.numNonCrustiesSeen = int.Parse(value);
                break;

            case ProgressionType.nonCrustyShuttled:
                data.progressionData.numNonCrustiesShuttled = int.Parse(value);
                break;
        }

        SaveData();
    }
    public int GetProgression_CurrDay()
    {
        return data.progressionData.currDay;
    }
    public void SetProgression_IncrementCurrDay()
    {
        SaveProgressionData(ProgressionType.currDay, (data.progressionData.currDay + 1).ToString());
    }
    public int GetProgression_NonCrustiesSeen()
    {
        return data.progressionData.numNonCrustiesSeen;
    }
    public void SetProgression_IncrementNonCrustiesSeen()
    {
        SaveProgressionData(ProgressionType.currDay, (data.progressionData.numNonCrustiesSeen + 1).ToString());
    }
    public int GetProgression_NonCrustiesShuttled()
    {
        return data.progressionData.numNonCrustiesShuttled;
    }
    public void SetProgression_IncrementNonCrustiesShuttled()
    {
        SaveProgressionData(ProgressionType.currDay, (data.progressionData.numNonCrustiesShuttled + 1).ToString());
    }
    public int GetProgression_TutorialState()
    {
        return data.progressionData.tutorialState;
    }
    public float GetProgression_PerfBarPercent()
    {
        return data.progressionData.performanceBarPercent;
    }
    public bool GetProgression_PerfBarSaved()
    {
        return data.progressionData.performanceBarSaved;
    }
    public bool GetProgression_IntroMailSeen()
    {
        return data.progressionData.introMailSeen;
    }
    public bool GetProgression_FirstDayHeadlineSeen()
    {
        return data.progressionData.firstDayHeadlineSeen;
    }
    public bool GetProgression_NewGame()
    {
        return data.progressionData.newGame;
    }
    public bool GetProgression_MedsAvailable()
    {
        return data.progressionData.medsAvailable;
    }
    public bool GetProgression_EatenBeforeDay3()
    {
        return data.progressionData.eatenBeforeDayThree;
    }
    public bool GetProgression_RedOne()
    {
        return data.progressionData.redOne;
    }

    #endregion

    public void ResetData()
    {
        // save settings
        string language = data.settings.language;
        float volumeMaster = data.settings.volume_Master;
        float volumeSFX = data.settings.volume_SFX;
        float volumeMusic = data.settings.volume_Music;
        float brightness = data.settings.brightness;
        bool reduceMotion = data.settings.reduceMotion;

        // reset file
        defaultPath = Path.Combine(Application.streamingAssetsPath, "Data", "Data.json");
        savePath = Path.Combine(Application.persistentDataPath, "Data.json");

        try
        {
            string jsonText = File.ReadAllText(defaultPath);
            data = JsonUtility.FromJson<Data>(jsonText);
            File.WriteAllText(savePath, JsonUtility.ToJson(data, true));

            // restore settings
            data.settings.language = language;
            data.settings.volume_Master = volumeMaster;
            data.settings.volume_SFX = volumeSFX;
            data.settings.volume_Music = volumeMusic;

            data.settings.brightness = brightness;
            data.settings.reduceMotion = reduceMotion;
            SaveData();

        }
        catch
        {
            Debug.Log("oops no file");
        }


        // reset inbox too
        string defaultPathInbox = Path.Combine(Application.streamingAssetsPath, "Data", "Inbox.json");
        string savePathInbox = Path.Combine(Application.persistentDataPath, "Inbox.json");

        try
        {
            string jsonText = File.ReadAllText(defaultPathInbox);
            PlotData dataPlot = JsonUtility.FromJson<PlotData>(jsonText);
            File.WriteAllText(savePathInbox, JsonUtility.ToJson(dataPlot, true));
        }
        catch
        {
            Debug.Log("whoops no file");
        }
        
    }
    private void SaveData()
    {
        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
    }
}
