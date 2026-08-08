using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScrpt : MonoBehaviour
{
    [SerializeField] private float speed = 16.0f;

    private Vector2 offPosBoard = new Vector2(0, 1929);
    private Vector2 onPosBoard = new Vector2(0, 0);
    private Vector2 offPosCrab = new Vector2(-292, -460);
    private Vector2 onPosCrab = new Vector2(-292, 0);
    [SerializeField] private RectTransform rectTransformBoard;
    [SerializeField] private RectTransform rectTransformCrab;
    [SerializeField] private GameObject backgroundOverlay;

    [SerializeField] private Button[] buttons;
    private Vector2 currVelocityBoard;
    private Vector2 currVelocityCrab;

    private bool moving = false;
    private bool displayed; // displayed is for animation purposes
    private bool paused; // pause the game when paused == true
    private bool crabMoving;
    private bool crabDisplayed;
    private bool hideAnimRunning = false;

    [SerializeField] private GameObject settings;

    void Start()
    {
        displayed = false;
        paused = false;
        rectTransformBoard.anchoredPosition = offPosBoard;
        rectTransformCrab.anchoredPosition = offPosCrab;

        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !settings.GetComponent<Settings>().IsDisplayed() && !settings.GetComponent<Settings>().IsMoving())
        {
            // pause
            if (!displayed && !paused)
            {
                paused = true;
                moving = true;
            }

            // unpause
            else
            {
                DisplayOff();
            }
        }

        if (moving)
        {
            // bring the screen down
            if (displayed)
            {
                rectTransformBoard.anchoredPosition = Vector2.SmoothDamp(rectTransformBoard.anchoredPosition, offPosBoard, ref currVelocityBoard, 0.25f);
                if (Vector2.Distance(rectTransformBoard.anchoredPosition, offPosBoard) < 100f)
                {
                    moving = false;
                    displayed = false;
                    // resume the clock/everything else when it's done moving
                }
            }
            // bring the screen up
            else
            {
                rectTransformBoard.anchoredPosition = Vector2.SmoothDamp(rectTransformBoard.anchoredPosition, onPosBoard, ref currVelocityBoard, 0.25f);
                if (Vector2.Distance(rectTransformBoard.anchoredPosition, onPosBoard) < 100f)
                {
                    crabMoving = true;
                }

            }
        }

        if (crabMoving)
        {
            if (crabDisplayed)
            {
                rectTransformCrab.anchoredPosition = Vector2.SmoothDamp(rectTransformCrab.anchoredPosition, offPosCrab, ref currVelocityCrab, 0.2f);
                if (Vector2.Distance(rectTransformCrab.anchoredPosition, offPosCrab) < 50f)
                {
                    moving = true;
                }

                if (Vector2.Distance(rectTransformCrab.anchoredPosition, offPosCrab) < 1f)
                {
                    crabMoving = false;
                    crabDisplayed = false;
                }
            }
            else
            {
                rectTransformCrab.anchoredPosition = Vector2.SmoothDamp(rectTransformCrab.anchoredPosition, onPosCrab, ref currVelocityCrab, 0.2f);

                if (Vector2.Distance(rectTransformCrab.anchoredPosition, onPosCrab) < 10f)
                {
                    foreach (Button button in buttons)
                    {
                        button.interactable = true;
                    }
                }
                if (Vector2.Distance(rectTransformCrab.anchoredPosition, onPosCrab) < 1f)
                {
                    crabMoving = false;
                    crabDisplayed = true;

                    moving = false;
                    displayed = true;

                    backgroundOverlay.SetActive(true);
                    Time.timeScale = 0f;

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
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }

            HideThing(backgroundOverlay);

            //moving = true;
            crabMoving = true;

            paused = false;
            //backgroundOverlay.SetActive(false);
            Time.timeScale = 1f;

            if (LevelManager.instance != null)
            {
                LevelManager.instance.SetState(LevelManager.LMState.Game);
            }
        }
    }

    private void HideThing(GameObject thing)
    {
        //print((thing.activeInHierarchy));
        if ((thing != null) && thing.activeInHierarchy)
        {
            hideAnimRunning = false;
            Animator animator = thing.GetComponent<Animator>();
            animator.enabled = true;
            StartCoroutine(WaitForAnim(thing, animator));
        }
    }

    private IEnumerator WaitForAnim(GameObject thing, Animator animator)
    {
        if (!hideAnimRunning) animator.SetTrigger("Hide");
        hideAnimRunning = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        thing.SetActive(false);
    }


    public void Back2menu()
    {
        Time.timeScale = 1f;
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.Home);
    }

    public void BackToTitle()
    {
        Time.timeScale = 1f;
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.TitleScreen);
    }

    public void OnSettings()
    {
        settings.SetActive(true);
        settings.GetComponent<Settings>().Show();
    }



}
