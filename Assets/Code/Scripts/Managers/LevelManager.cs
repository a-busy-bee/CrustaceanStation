using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    [Header("Main")]
    [SerializeField] private Clock clock;
    [SerializeField] private Kiosk kiosk;

    // prefabs
    [Header("Goals & Summary")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject goalRating;
    [SerializeField] private RatingGoal ratingGoalScript;
    [SerializeField] private GameObject goalCrabCount;
    [SerializeField] private CrabCountGoal crabCountGoalScript;
    [SerializeField] private GameObject summaryMenu;

    [Header("Track Upgrade")]
    [SerializeField] private GameObject[] tracks;


    [Header("Decor")]
    [SerializeField] private DecorItems[] items;
    [SerializeField] private DecorItems[] topDecor;
    [SerializeField] private GameObject leftSlot;
    [SerializeField] private GameObject rightSlot;
    [SerializeField] private GameObject topSlot;



    // UI background 
    [Header("Other")]
    [SerializeField] private GameObject transparentOverlay;
    [SerializeField] private AudioManager audioManager;

    // Player goals for the day
    private bool isRating = false;
    private bool dayStarted = false;

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

        goalRating.SetActive(false);
        goalCrabCount.SetActive(false);
        summaryMenu.SetActive(false);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("newGame") == 1)
        {
            PlayerPrefs.SetInt("newGame", -1); // set so that it's not replayed unless progress is reset

            // TODO: Play the tutorial
        }

        ActivateUpgrades();
        ShowDecor();

        StartCoroutine(ShowGoalForTheDay());
    }

    public void OnPause()
    {
        // show overlay background
        transparentOverlay.SetActive(true);

        // stop clock & crabs & trains
        Time.timeScale = 0f;
        audioManager.Play();

    }

    public void OnResume()
    {
        // hide overlay background
        transparentOverlay.SetActive(false);

        // start clock & crabs & trains
        Time.timeScale = 1f;
    }

    public void OnStartLevel()
    {
        // start the clock
        clock.BeginDay();

        dayStarted = true;
    }

    public void ShowStatsForTheDay()
    {
        // show prefab
        transparentOverlay.SetActive(true);
        summaryMenu.SetActive(true);
        summaryMenu.GetComponent<Summary>().SetRating(ratingGoalScript.GetRating());
        summaryMenu.GetComponent<Summary>().SetCrabsProcessed(kiosk.GetTotalCrabs());

        dayStarted = false;
    }

    public void OnShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void OnMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void OnNewDay()
    {
        SceneManager.LoadScene("Temp");
    }


    private IEnumerator ShowGoalForTheDay()
    {
        transparentOverlay.SetActive(true);

        // show prefab
        if (Random.Range(0, 10) > 4)
        {
            isRating = true;
            goalRating.SetActive(true);
            ratingGoalScript.SetGoalActive();
        }
        else
        {
            goalCrabCount.SetActive(true);
            crabCountGoalScript.SetGoalActive();
        }

        yield return new WaitForSeconds(3.5f);

        // hide prefab
        if (isRating)
        {
            goalRating.SetActive(false);
        }
        else
        {
            goalCrabCount.SetActive(false);
        }

        transparentOverlay.SetActive(false);

        OnStartLevel();
    }

    public bool HasStarted()
    {
        return dayStarted;
    }

    private void ActivateUpgrades()
    {
        int trackCount = PlayerPrefs.GetInt("numTracks"); // starts at 0, so need to add one when updating track num in clock

        // activate tracks
        for (int i = 0; i < trackCount; i++)
        {
            tracks[i].SetActive(true);
        }

        // update train controllers
        clock.UpdateNumTracks(trackCount + 1);
    }

    private void ShowDecor()
    {
        int top = PlayerPrefs.GetInt("decor_top");
        int left = PlayerPrefs.GetInt("decor_left");
        int right = PlayerPrefs.GetInt("decor_right");

        if (top > 1)
        {
            topSlot.SetActive(true);
            topSlot.GetComponent<Image>().sprite = topDecor[top].sprite;
        }
        else
        {
            topSlot.SetActive(false);
        }

        if (left > 1)
        {
            leftSlot.SetActive(true);
            leftSlot.GetComponent<Image>().sprite = items[left].sprite;
        }
        else
        {
            leftSlot.SetActive(false);
        }

        if (right > 1)
        {
            rightSlot.SetActive(true);
            rightSlot.GetComponent<Image>().sprite = items[right].sprite;
        }
        else
        {
            rightSlot.SetActive(false);
        }
    }

    public int GetCartQuality()
    {
        return PlayerPrefs.GetInt("cartQuality");
    }

    [ContextMenu("Reset Progress")]
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }

}
