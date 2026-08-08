using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerformanceManager : MonoBehaviour
{
    public static PerformanceManager instance { get; protected set; }
    private float barPercent;
    private int numWrong;
    private Dictionary<MistakeType, int> mistakes = new Dictionary<MistakeType, int>
    {
        {MistakeType.idTicket, 0},
        {MistakeType.transport, 0},
        {MistakeType.seating, 0},
        {MistakeType.plot, 0}
    };
    private int numHappy;
    private int correctCounter; // must get all three stages to be correct (idTicket, transport, and seating)
    private bool isChanging;
    private float currentVelocity;
    private float stepSize = 0.1f;

    private Animator animator;
    private bool sparkled = false;

    [SerializeField] private Slider sliderKiosk;
    [SerializeField] private Slider sliderSummary;
    [SerializeField] private Image sliderKioskColor;
    [SerializeField] private Image sliderSummaryColor;
    [SerializeField] private Color[] sliderColors;
    [SerializeField] private Animator sparkleAnimator;

    [Header("Summary Numbers")]
    [SerializeField] private TextMeshProUGUI numWrongText;
    [SerializeField] private TextMeshProUGUI numIDTicketText;
    [SerializeField] private TextMeshProUGUI numTransportText;
    [SerializeField] private TextMeshProUGUI numSeatingText;
    [SerializeField] private TextMeshProUGUI numHappyText;


    public enum MistakeType
    {
        idTicket,
        transport,
        seating,
        plot
    }

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
    }

    private void Start()
    {
        SaveManager saveManager = SaveManager.instance;
        animator = GetComponent<Animator>();

        bool isPerformanceBarSaved = saveManager.GetProgression_PerfBarSaved();
        if (!isPerformanceBarSaved)
        {
            saveManager.SaveProgressionData(SaveManager.ProgressionType.performanceBarPercent, 0.67f.ToString());
            saveManager.SaveProgressionData(SaveManager.ProgressionType.performanceBarSaved, true.ToString());
        }

        barPercent = saveManager.GetProgression_PerfBarPercent();

        
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
        //Debug.Log("correct");
        barPercent += stepSize;
        if (barPercent >= 1) barPercent = 1;

        correctCounter++;

        Save();
        UpdateSlider();
    }

    [ContextMenu("Correct Half")]
    public void CorrectHalf() // for smaller increases
    {
        //Debug.Log("correct");
        barPercent += stepSize / 2.0f;
        if (barPercent >= 1) barPercent = 1;

        correctCounter++;
        if (correctCounter == 3)
        {
            numHappy++;
            correctCounter = 0;
        }

        Save();
        UpdateSlider();
    }

    [ContextMenu("Incorrect")]
    public void Incorrect(MistakeType mistake)
    {
        animator.SetTrigger("Rumble");
        numWrong++;
        barPercent -= stepSize * 3.5f;
        if (barPercent <= 0)
        {
            barPercent = 0;
        }

        mistakes[mistake]++;
        ResetCorrect();
        
        Save();
        UpdateSlider();
    }

    [ContextMenu("Incorrect Half")]
    public void IncorrectHalf(MistakeType mistake)
    {
        animator.SetTrigger("Rumble");
        numWrong++;
        barPercent -= stepSize;
        if (barPercent <= 0)
        {
            barPercent = 0;
        }

        mistakes[mistake]++;
        ResetCorrect();

        Save();
        UpdateSlider();
    }

    public void ResetCorrect()
    {
        correctCounter = 0;
    }

    private void Save()
    {
        SaveManager.instance.SaveProgressionData(SaveManager.ProgressionType.performanceBarPercent, barPercent.ToString());
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
            // todo: if moving down, do some kind of particle system or effect

            sliderKiosk.value = Mathf.SmoothDamp(sliderKiosk.value, barPercent, ref currentVelocity, 0.75f);

            if (barPercent < sliderKiosk.value)
            {
                sparkled = false;
            }

            // upon reaching full bar
            if (!sparkled && (sliderKiosk.value >= 0.95f) && (barPercent > sliderKiosk.value))
            {
                sparkleAnimator.SetTrigger("Sparkle");
                sparkled = true;
            }

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

            if (Mathf.Abs(barPercent - sliderKiosk.value) < 0.01f)
            {
                isChanging = false;
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
        numIDTicketText.text = mistakes[MistakeType.idTicket].ToString();
        numTransportText.text = mistakes[MistakeType.transport].ToString();
        numSeatingText.text = mistakes[MistakeType.seating].ToString();
        numHappyText.text = numHappy.ToString();

        sliderSummary.value = barPercent;

        bool alreadyRedBefore = SaveManager.instance.GetProgression_RedOne();
        if (barPercent >= 0.7f)
        {
            sliderSummaryColor.color = sliderColors[2];

            if (alreadyRedBefore) SaveManager.instance.SaveProgressionData(SaveManager.ProgressionType.red1, "false");
        }
        else if (barPercent >= 0.2)
        {
            sliderSummaryColor.color = sliderColors[1];
            if (alreadyRedBefore) SaveManager.instance.SaveProgressionData(SaveManager.ProgressionType.red1, "false");
        }
        else
        {
            sliderSummaryColor.color = sliderColors[0];

            if (alreadyRedBefore)
            {
                //second day having a red rating
                // cue fired cutscene
                SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.Fired);
                return;
            }

            int currDay = SaveManager.instance.GetProgression_CurrDay();
            if (currDay < 5)
            {
                PlotManager.instance.AddMail("letter", "crustyCoID", 0);
            }
            else
            {
                PlotManager.instance.AddMail("letter", "crustyCoID", 1);
            }

            SaveManager.instance.SaveProgressionData(SaveManager.ProgressionType.red1, "true");
        }
    }

}
