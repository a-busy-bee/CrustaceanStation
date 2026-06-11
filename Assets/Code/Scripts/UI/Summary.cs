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
        if (HeadlineManager.instance != null && PlayerPrefs.GetInt("first day") != 1)
        {
            PlayerPrefs.SetInt("first day", 1);
            HeadlineManager.instance.SetSpecificText(100, "Crustacean Station Grand Opening!");
            return;
        }
        if (Random.Range(0, 10) < 5) return;

        if (Random.Range(0, 10) < 4)
        {
            HeadlineManager.instance.GetPlotHeadline(PlotManager.instance.GetCurrStage());
        }
        else
        {
            HeadlineManager.instance.GetGenericHeadline();
        }
    }

    public void SetCrabsProcessed(int crabs)
    {
        /*Debug.Log("num crabs " + crabs);
        crabsProcessed.text = crabs.ToString();

        if (crabCountGoal.IsActive() && crabCountGoal.WasGoalAchieved())
        {
            Debug.Log("acheived crab");
            crabGoalReward.SetActive(true);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 20);
        }*/
    }

    public void SetRating(float rating)
    {
        /*Debug.Log("rating " + rating);
        ratings.value = rating;

        if (ratingGoal.IsGoalActive() && ratingGoal.WasGoalAchieved())
        {
            Debug.Log("acheived rating");
            ratingGoalReward.SetActive(true);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 20);
        }*/
    }

    public void Continue()
    {
        SceneManager.LoadScene("Home");
    }
}
