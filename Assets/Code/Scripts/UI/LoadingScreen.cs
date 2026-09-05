using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class LoadingScreen : MonoBehaviour
{

    //[SerializeField] private GameObject imagenewGame;
    //[SerializeField] private Animator animator;
    [SerializeField] Slider slider;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private GameObject sliderParent;
    [SerializeField] private TextMeshProUGUI tooltipText;
    private float sinFreq = 20f;
    private string currIdx;
    private bool isSubscribed;
    private string[] tooltips = {
        "Summoning crabs...",
        "Fighting off seagulls...",
        "Distracting whales...",
        "Decorating the kiosk...",
        "Watching sunsets..."
    };

    private void Start()
    {
        group.alpha = 0;
        group.blocksRaycasts = false;

        LocalizationSettings.SelectedLocaleChanged += RefreshCurrDialogue;
        isSubscribed = true;
        currIdx = "";
    }

    private void OnDisable()
    {
        if (!isSubscribed) return;
        LocalizationSettings.SelectedLocaleChanged -= RefreshCurrDialogue;
        isSubscribed = false;
    }

    private void OnDestroy()
    {
        if (!isSubscribed) return;
        LocalizationSettings.SelectedLocaleChanged -= RefreshCurrDialogue;
        isSubscribed = false;
    }

    public void PlayLoad(string sceneName)
    {
        StartCoroutine(FadeThenLoadThenFade(sceneName));
    }

    private IEnumerator FadeThenLoadThenFade(string sceneName)
    {
        FadeToBlack.instance.FadeIn();
        yield return new WaitForSeconds(0.5f);
        group.blocksRaycasts = true;
        group.alpha = 1;

        FadeToBlack.instance.FadeOut();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        sliderParent.SetActive(true);
        slider.value = 0;
        currIdx = "misc_loading_" + Random.Range(0, tooltips.Length);
        tooltipText.text = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Misc, currIdx);

        while (slider.value < 0.67f) // fake some loading because we're evil and want people to look at the fire splash art
        {
            sinFreq = Random.Range(20, 50);
            float sinVal = Mathf.Sin(Time.time * sinFreq);
            if (sinVal < 0) sinVal = Random.Range(0, 1);

            slider.value += fadeSpeed * sinVal * Time.deltaTime;
            yield return null;
        }

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        FadeToBlack.instance.FadeIn();

        while (!loadingOperation.isDone)
        {
            slider.value = loadingOperation.progress - 0.67f;
            yield return null;
        }

        slider.value = 1f;

    }
    
    public void RefreshCurrDialogue(Locale l)
    {
        if (currIdx == "") return;
        string text = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Misc, currIdx);
        tooltipText.text = text;
    }
}