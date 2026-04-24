using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class PerformanceManager : MonoBehaviour
{
    private float barPercent;
    private int numSeen;
    private int numCorrect;
    private bool isChanging;
    private float currentVelocity; 

    [SerializeField] private Slider sliderKiosk;
    //[SerializeField] private Slider sliderSummary;
    [SerializeField] private Image sliderKioskColor;
    //[SerializeField] private Image sliderSummaryColor;
    [SerializeField] private Color[] sliderColors;

    // (numSeen - numCorrect)/numCorrect

    private void Awake()
    {
        if (PlayerPrefs.GetInt("PerformanceBarSavedData") == 1)
        {
            PlayerPrefs.SetFloat("PerformanceBarPercent", 0.67f);
            PlayerPrefs.SetInt("PerformanceBarNumSeen", 7);
        }
        numSeen = PlayerPrefs.GetInt("PerformanceBarNumSeen");
        barPercent = PlayerPrefs.GetFloat("PerformanceBarPercent");
        numCorrect = (int)((float)numSeen / (float)(barPercent + 1));
    }

    private void Start()
    {
        UpdateSlider();
    }

    [ContextMenu("Correct")]
    public void Correct()
    {
        
        numSeen++;
        Save();
        UpdateSlider();
    }

    [ContextMenu("Incorrect")]
    public void Incorrect()
    {
        numCorrect++;
        numSeen++;

        Save();
        UpdateSlider();
    }

    private void Save()
    {
        barPercent = (float)(numSeen - numCorrect) / (float)numCorrect;
        PlayerPrefs.SetFloat("PerformanceBarPercent", barPercent);
        PlayerPrefs.SetInt("PerformanceBarNumSeen", numSeen);
    }

    private void UpdateSlider()
    {
        isChanging = true;
        //sliderSummary.value = barPercent;

        // 0.7 for green/yellow boundary
        // 0.2 for yellow/red boundary
        if (barPercent >= 0.7f)
        {
            sliderKioskColor.color = sliderColors[2];
        }
        else if (barPercent >= 0.2)
        {
            sliderKioskColor.color = sliderColors[1];
        }
        else
        {
            sliderKioskColor.color = sliderColors[0];
        }
    }

	private void Update()
	{
        if (isChanging)
        {
            sliderKiosk.value = Mathf.SmoothDamp(sliderKiosk.value, barPercent, ref currentVelocity, 0.25f);

            if (Mathf.Abs(barPercent - sliderKiosk.value) < 0.01f)
            {
                isChanging = false;
            }

        }
	} 

}
