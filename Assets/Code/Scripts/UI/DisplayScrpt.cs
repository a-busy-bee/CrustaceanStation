using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class DisplayScrpt : MonoBehaviour
{
    [SerializeField] private float speed = 16.0f;

    private Vector3 offPosBoard = new Vector3(0, 1110, -2);
    private Vector3 onPosBoard = new Vector3(0, 27.7f, -2);
    private Vector3 offPosCrab = new Vector3(-217, -776f, -2);
    private Vector3 onPosCrab = new Vector3(-217, -434, -2);
    [SerializeField] private RectTransform rectTransformBoard;
    [SerializeField] private RectTransform rectTransformCrab;
    private Vector3 currVelocityBoard;
    private Vector3 currVelocityCrab;

    private bool moving = false;
    private bool displayed; // displayed is for animation purposes
    private bool paused; // pause the game when paused == true
    private bool crabMoving;
    private bool crabDisplayed;

    
    [SerializeField] private GameObject settings;
    void Start()
    {
        displayed = false;
        paused = false;
        rectTransformBoard.anchoredPosition = offPosBoard;
        rectTransformCrab.anchoredPosition = offPosCrab;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && LevelManager.instance.HasStarted() && !settings.activeSelf)
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
                /*var step = speed * Time.deltaTime;
                rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, offPos, step);
                if (rectTransform.anchoredPosition.y <= offPos.y || Mathf.Abs(rectTransform.anchoredPosition.y - offPos.y) < 0.0001f)
                {
                    moving = false;
                    displayed = false;
                }*/

                rectTransformBoard.anchoredPosition = Vector3.SmoothDamp(rectTransformBoard.anchoredPosition, offPosBoard, ref currVelocityBoard, 0.25f);
                if (Vector3.Distance(rectTransformBoard.anchoredPosition, offPosBoard) < 1f)
                {
                    moving = false;
                    displayed = false;

                    // resume the clock/everything else when it's done moving
                }


            }

            // bring the screen up
            else
            {
                /*var step = speed * Time.deltaTime;
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
                }*/

                rectTransformBoard.anchoredPosition = Vector3.SmoothDamp(rectTransformBoard.anchoredPosition, onPosBoard, ref currVelocityBoard, 0.25f);
                if (Vector3.Distance(rectTransformBoard.anchoredPosition, onPosBoard) < 100f)
                {
                    crabMoving = true;
                }


                if (Vector3.Distance(rectTransformBoard.anchoredPosition, onPosBoard) < 1f)
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

        if (crabMoving)
        {
            if (crabDisplayed)
            {
                rectTransformCrab.anchoredPosition = Vector3.SmoothDamp(rectTransformCrab.anchoredPosition, offPosCrab, ref currVelocityCrab, 0.25f);
                if (Vector3.Distance(rectTransformCrab.anchoredPosition, offPosBoard) < 1f)
                {
                    crabMoving = false;
                    displayed = false;
                }
            }
            else
            {
                rectTransformCrab.anchoredPosition = Vector3.SmoothDamp(rectTransformCrab.anchoredPosition, onPosCrab, ref currVelocityCrab, 0.25f);
                if (Vector3.Distance(rectTransformCrab.anchoredPosition, onPosCrab) < 1f)
                {
                    crabMoving = false;
                    crabDisplayed = true;
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
}
