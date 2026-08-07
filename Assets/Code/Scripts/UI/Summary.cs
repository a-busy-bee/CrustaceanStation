using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Summary : MonoBehaviour
{
    [SerializeField] private GameObject screendim;
    private void Start()
    {
        screendim.SetActive(false);

        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1120);
        GetComponent<SmoothLerp>().Move(new Vector2(0, 0), 0.75f);

        SaveManager saveManager = SaveManager.instance;

        bool isFirstDayHeadline = saveManager.GetProgression_FirstDayHeadlineSeen();
        if (HeadlineManager.instance != null && !isFirstDayHeadline)
        {
            saveManager.SaveProgressionData(SaveManager.ProgressionType.firstDayHeadlineSeen, true.ToString());

            HeadlineManager.instance.SetSpecificText(100, "Crustacean Station Grand Opening!");
            return;
        }

        if (Random.Range(0, 10) < 3) return;

        if (Random.Range(0, 10) < 4)
        {
            HeadlineManager.instance.GetPlotHeadline(PlotManager.instance.GetCurrStage());
        }
        else
        {
            HeadlineManager.instance.GetGenericHeadline();
        }
        
        StartCoroutine(WaitThenDim());
    }

    private IEnumerator WaitThenDim()
    {
        yield return new WaitForSeconds(0.75f);

        screendim.SetActive(true);
    }

    public void Continue()
    {
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.Home);
    }
}
