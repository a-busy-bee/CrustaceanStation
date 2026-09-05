using UnityEngine;
using TMPro;

using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
public class LocalizationFontSwapper : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    [SerializeField] private TMP_FontAsset mainFont;
    [SerializeField] private TMP_FontAsset chineseFont;
    [SerializeField] private TMP_FontAsset japaneseFont;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        UpdateFont();
    }

    void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    void OnLocaleChanged(Locale newLocale)
    {
        UpdateFont();
    }

    void UpdateFont()
    {
        var localeCode = LocalizationSettings.SelectedLocale.Identifier.Code;
        
        if (localeCode.StartsWith("zh"))
            textComponent.font = chineseFont;
        else if (localeCode.StartsWith("ja"))
            textComponent.font = japaneseFont;
        else
            textComponent.font = mainFont;
    }
}
