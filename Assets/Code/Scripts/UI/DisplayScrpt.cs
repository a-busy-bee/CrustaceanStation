using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class DisplayScrpt : MonoBehaviour
{
    [SerializeField] private float speed = 16.0f;
    private Vector3 offPos = new Vector3(0, -1208.0f, -2);
    private Vector3 onPos = new Vector3(0, -0.91f, -2);
    private bool moving = false;
    // displayed is for animation purposes
    [SerializeField] private bool displayed;
    // pause the game when paused == true
    [SerializeField] private bool paused;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject tutorial;

    void Start()
    {
        displayed = false;
        paused = false;
        rectTransform.anchoredPosition = offPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && LevelManager.instance.HasStarted() && !settings.activeSelf && !tutorial.activeSelf)
        {
            // pause
            if (!displayed)
            {
                paused = true;
                moving = true;
            }

            // unpause
            if (displayed)
            {
                paused = false;
                moving = true;

                if (LevelManager.instance != null)
                {
                    LevelManager.instance.SetState(LevelManager.LMState.Game);
                }
            }
            Debug.Log(paused);
        }

        if (moving)
        {
            // bring the screen down
            if (displayed)
            {
                var step = speed * Time.deltaTime;
                rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, offPos, step);
                if (rectTransform.anchoredPosition.y <= offPos.y || Mathf.Abs(rectTransform.anchoredPosition.y - offPos.y) < 0.0001f)
                {
                    moving = false;
                    displayed = false;

                    // resume the clock/everything else when it's done moving
                }
            }

            // bring the screen up
            else if (!displayed)
            {
                var step = speed * Time.deltaTime;
                rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, onPos, step);
                if (rectTransform.anchoredPosition.y >= onPos.y || Mathf.Abs(rectTransform.anchoredPosition.y - onPos.y) < 0.0001f)
                {
                    moving = false;
                    displayed = true;

                    // stop the clock! 
                    if (LevelManager.instance != null)
                    {
                        LevelManager.instance.SetState(LevelManager.LMState.Paused);
                    }
                }
            }
        }
    }

    // clicking continue
    public void DisplayOff()
    {
        if (displayed)
        {
            moving = true;
            paused = false;

            if (LevelManager.instance != null)
            {
                LevelManager.instance.SetState(LevelManager.LMState.Game);
            }
        }
        Debug.Log(paused);
    }


    public void Back2menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Home");
    }

    public void OnSettings()
    {
        settings.SetActive(true);
    }

    public void OnTutorial()
    {
        tutorial.SetActive(true);
        tutorial.GetComponent<Tutorial>().Play(false);
    }
}
