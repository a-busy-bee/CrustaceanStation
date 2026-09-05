using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Localization.Settings;
using Unity.VisualScripting;

using UnityEngine.Localization;

public class Settings_Languages : MonoBehaviour
{
    private string[] languages = new string[] {
        "English",
        "Español",
        "Français",
        "Deutsch",
        "Italiano",
        "日本語",
        "中国人"
    };

    private int currIdx = -1;
    private int currLanguageIdx;
    private bool isChanging;

    [SerializeField] private TextMeshProUGUI languageText;

    private void Start()
    {
        int language = SaveManager.instance.GetSettings_Language();

        for (int i = 0; i < languages.Length; i++)
        {
            if (languages[i] == languages[language])
            {
                currIdx = i;
                SetLanguageText(currIdx);
                ApplyLanguage();
            }
        }

        if (currIdx == -1) // no resolution saved/found
        {
            currIdx = 0;
            SetLanguageText(currIdx);
        }
    }

    public void NextLanguage()
    {
        currIdx++;
        if (currIdx == languages.Length) currIdx = 0;

        SetLanguageText(currIdx);
        ApplyLanguage();
    }

    public void PrevLanguage()
    {
        currIdx--;
        if (currIdx == -1) currIdx = languages.Length - 1;

        SetLanguageText(currIdx);
        ApplyLanguage();
    }

    public void ApplyLanguage()
    {
        currLanguageIdx = currIdx;

        // langauge logic stuff
        StartCoroutine(LoadGlobalLocale());
    }

    private IEnumerator LoadGlobalLocale()
    {
        isChanging = true;
        yield return LocalizationSettings.InitializationOperation;

        if (currIdx >= 0 && currIdx < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            Locale targetLocale = LocalizationSettings.AvailableLocales.Locales[currIdx];
            LocalizationSettings.SelectedLocale = targetLocale;

            SaveManager.instance.SaveSettings(SaveManager.SettingsType.language, currIdx.ToString());
        }
    }

    private void SetLanguageText(int idx)
    {
        languageText.text = languages[idx];
    }
    


}
