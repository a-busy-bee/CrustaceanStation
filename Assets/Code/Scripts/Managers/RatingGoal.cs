using UnityEngine;
using UnityEngine.UI;

public class RatingGoal : MonoBehaviour
{
    /*
    TODO: 
        highlight background green when valid rating
        function to give player extra coins if they beat the goal
    */

    private bool isGoalActive;
    private float rating;
    private float goal;
    [SerializeField] private Slider ratingsSlider;
    [SerializeField] private GameObject ratingsSliderObject;

    [SerializeField] private Image background;

    [SerializeField] private Slider goalScreenSlider;

    // SLIDER MOVEMENT
    private float ratingValue;
    private float movementSpeed = 5f;
    private bool updating = false;

    private void Awake()
    {
        ratingsSlider.value = 1;
        isGoalActive = false;
        ratingsSliderObject.SetActive(false);
    }

    public void UpdateRating(float newRating)
    {
        updating = true;
        rating = newRating;
    }

    public float GetRating()
    {
        return rating;
    }

    public bool IsGoalActive()
    {
        return isGoalActive;
    }

    public void SetGoalActive()
    {
        isGoalActive = true;
        ratingsSliderObject.SetActive(true);
        ratingsSlider.value = 1f;

        goal = Random.Range(5, 11);
        goalScreenSlider.value = goal / 10f;
    }

    public bool WasGoalAchieved()
    {
        return rating >= goalScreenSlider.value;
    }

    void Update()
    {
        if (updating)
        {
            ratingsSlider.value = Mathf.Lerp(ratingsSlider.value, rating, movementSpeed * Time.deltaTime);

            if (Mathf.Abs(ratingsSlider.value - rating) < 0.01f)
            {
                updating = false;
                ratingsSlider.value = rating;
            }
        }
        
    }
}
