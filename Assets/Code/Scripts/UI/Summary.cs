using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Summary : MonoBehaviour
{

    private void Start()
    {
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

        
    }

    public void Continue()
    {
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneManager.LoadScene("Home");
    }
}
