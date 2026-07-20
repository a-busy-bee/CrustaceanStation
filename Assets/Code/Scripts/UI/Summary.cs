using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Summary : MonoBehaviour
{
    //[Header("Rating Goal")]
    //[SerializeField] private Slider ratings;
    //[SerializeField] private RatingGoal ratingGoal;
    //[SerializeField] private GameObject ratingGoalReward;

    //[Header("Crabs Seen Goal")]
    //[SerializeField] private TextMeshProUGUI crabsProcessed;
    //[SerializeField] private CrabCountGoal crabCountGoal;
    //[SerializeField] private GameObject crabGoalReward;

    private void Awake()
    {
        /*ratingGoalReward.SetActive(false);
        crabGoalReward.SetActive(false);*/
    }

    private void Start()
    {
        SaveManager saveManager = SaveManager.instance;


        saveManager.SetProgression_IncrementCurrDay();

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
        SceneManager.LoadScene("Home");
    }
}
