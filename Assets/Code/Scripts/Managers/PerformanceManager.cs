using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerformanceManager : MonoBehaviour
{
    public static PerformanceManager instance { get; protected set; }
    private float barPercent;
    private int numWrong;
    private bool isChanging;
    private float currentVelocity;
    private float stepSize = 0.1f;

    [SerializeField] private Slider sliderKiosk;
    [SerializeField] private Slider sliderSummary;
    [SerializeField] private Image sliderKioskColor;
    [SerializeField] private Image sliderSummaryColor;
    [SerializeField] private Color[] sliderColors;
    [SerializeField] private TextMeshProUGUI numWrongText;

    // (numSeen - numCorrect)/numCorrect

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


        if (PlayerPrefs.GetInt("PerformanceBarSavedData") == 1)
        {
            PlayerPrefs.SetFloat("PerformanceBarPercent", 0.67f);
        }

        barPercent = PlayerPrefs.GetFloat("PerformanceBarPercent");
    }

    private void Start()
    {
        sliderKiosk.value = barPercent;
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

    [ContextMenu("Correct")]
    public void Correct()
    {
        Debug.Log("correct");
        barPercent += stepSize;
        if (barPercent >= 1) barPercent = 1;

        Save();
        UpdateSlider();
    }

    [ContextMenu("Incorrect")]
    public void Incorrect()
    {
        Debug.Log("incorrect");
        numWrong++;
        barPercent -= stepSize;
        if (barPercent <= 0) barPercent = 0;

        Save();
        UpdateSlider();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("PerformanceBarPercent", barPercent);
    }

    private void UpdateSlider()
    {
        isChanging = true;
        //sliderSummary.value = barPercent;

        // 0.7 for green/yellow boundary
        // 0.2 for yellow/red boundary
    }

    private void Update()
    {
        if (isChanging)
        {
            sliderKiosk.value = Mathf.SmoothDamp(sliderKiosk.value, barPercent, ref currentVelocity, 0.75f);

            if (Mathf.Abs(barPercent - sliderKiosk.value) < 0.01f)
            {
                isChanging = false;

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

        }
    }

    public float GetBarPercent()
    {
        return barPercent;
    }

    public int GetNumWrong()
    {
        return numWrong;
    }

    public void InitSummary()
    {
        numWrongText.text = numWrong.ToString();
        sliderSummary.value = barPercent;
        if (barPercent >= 0.7f)
        {
            sliderSummaryColor.color = sliderColors[2];
        }
        else if (barPercent >= 0.2)
        {
            sliderSummaryColor.color = sliderColors[1];
        }
        else
        {
            sliderSummaryColor.color = sliderColors[0];

            PlotManager.instance.AddMail("letter", "crustyCo", 8);
        }
    }

}
