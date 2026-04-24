using UnityEngine;
using UnityEngine.UI;

public class PerformanceManager : MonoBehaviour
{
    private float barPercent;
    private int numSeen;
    private int numCorrect;

    [SerializeField] private Slider sliderKiosk;
    [SerializeField] private Slider sliderSummary;
    [SerializeField] private Image sliderKioskColor;
    [SerializeField] private Image sliderSummaryColor;
    [SerializeField] private Color[] sliderColors;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("PerformanceBarSavedData") == 1)
        {
            PlayerPrefs.SetFloat("PerformanceBarPercent", 0.67f);
            PlayerPrefs.SetInt("PerformanceBarNumSeen", 7);
        }
        numSeen = PlayerPrefs.GetInt("PerformanceBarNumSeen");
        barPercent = PlayerPrefs.GetFloat("PerformanceBarPercent");
        numCorrect = (int)(numSeen * barPercent);
    }
    public void Correct()
    {
        numCorrect++;
        numSeen++;
        Save();
    }

    public void Incorrect()
    {
        numSeen++;
        Save();
    }

    private void Save()
    {
        barPercent = (float)numCorrect / (float)numSeen;
        PlayerPrefs.SetFloat("PerformanceBarPercent", barPercent);
        PlayerPrefs.SetInt("PerformanceBarNumSeen", numSeen);
    }

    private void UpdateSlider()
    {
        sliderKiosk.value = barPercent;
        sliderSummary.value = barPercent;

        //)
    }
}
