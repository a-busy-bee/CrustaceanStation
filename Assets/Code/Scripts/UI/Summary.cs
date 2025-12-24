using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Summary : MonoBehaviour
{
    [Header("Rating Goal")]
    [SerializeField] private Slider ratings;
    [SerializeField] private RatingGoal ratingGoal;
    [SerializeField] private GameObject ratingGoalReward;

    [Header("Crabs Seen Goal")]
    [SerializeField] private TextMeshProUGUI crabsProcessed;
    [SerializeField] private CrabCountGoal crabCountGoal;
    [SerializeField] private GameObject crabGoalReward;

    private void Awake()
    {
        ratingGoalReward.SetActive(false);
        crabGoalReward.SetActive(false);
	}
	public void SetCrabsProcessed(int crabs)
    {
        Debug.Log("num crabs " + crabs);
        crabsProcessed.text = crabs.ToString();

        if (crabCountGoal.IsActive() && crabCountGoal.WasGoalAchieved())
        {
            Debug.Log("acheived crab");
            crabGoalReward.SetActive(true);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 20);
        }
    }

    public void SetRating(float rating)
    {
        Debug.Log("rating " + rating);
        ratings.value = rating;

        if (ratingGoal.IsGoalActive() && ratingGoal.WasGoalAchieved())
        {
            Debug.Log("acheived rating");
            ratingGoalReward.SetActive(true);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 20);
        }
    }
}
