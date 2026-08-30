using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance { get; private set; }

    public enum Table
    {
        TitleScreen,
        Credits,
        Cutscene,
        Dialogue,
        FeedbackForms,
        Headlines,
        Letters,
        Misc,
        Names,
        Summary,
        Tutorial,
        UI
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
    }

    public string GetTextByStringKey(Table table, string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(table.ToString(), key);
    } 
}

