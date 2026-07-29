using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutscenePlayer_Good : MonoBehaviour
{
    public static CutscenePlayer_Good instance { get; private set; }

    private const int normalSceneLength = 2;
    //private const int animatedSceneLength = 2;
    private float sceneLength = 2;

    // scenes
    [SerializeField] private GameObject[] scenes;
    [SerializeField] private GameObject firstBlackBkg;
    [SerializeField] private CanvasGroup fadeToBlack;
    [SerializeField] private CanvasGroup clickToContinueText;
    [SerializeField] private GameObject clickToContinue;
    [SerializeField] private GameObject gramps;
    private int currSceneIdx = 0;
    private bool fading;
    private float currVelocity;

    // gramps dialogue
    private int currGrampsDialogue = 0;
    private IEnumerator WaitClickToContinueTimer;
    private bool fadingClickToContinue;
    private float currVelocityClickToContinue;

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

        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].SetActive(false);
        }

        clickToContinue.SetActive(false);

        firstBlackBkg.SetActive(true);
        scenes[currSceneIdx].SetActive(true);
        fadeToBlack.alpha = 0;

        StartCoroutine(WaitThenContinueNextScene());
    }

    public void ProgressScene()
    {
        if (currSceneIdx < 3) // scenes before dialogue
        {
            scenes[currSceneIdx].SetActive(false);
            currSceneIdx++;
            scenes[currSceneIdx].SetActive(true);

            if (currSceneIdx == 2) // if a scene is animated, but we don't have that rn
            {
                //scenes[currSceneIdx - 1].SetActive(true);
                //sceneLength = animatedSceneLength;
                sceneLength = normalSceneLength;
            }
            else sceneLength = normalSceneLength;

            StartCoroutine(WaitThenContinueNextScene());
        }
        else if (currSceneIdx == 3)
        {

            AppearGramps();
        }

    }

    private void AppearGramps()
    {
        // appear gramps
        gramps.GetComponent<SmoothLerp>().Move(new Vector2(0, 0), 0.5f);

        StartCoroutine(WaitForGrampsToAppear());
    }

    private void Dialogue(int idx)
    {
        // show dialogue at idx
        DialogueManager.instance.ShowGrampsGoodEndingDialogue(idx);

        if (currGrampsDialogue == 39)
        {
            StartCoroutine(WaitThenFade());
            return;
        }

        // enable click to continue
        clickToContinue.SetActive(true);

        // start timer
        WaitClickToContinueTimer = ClickToContinueTimer();
        StartCoroutine(WaitClickToContinueTimer);
    }

    private IEnumerator WaitThenFade()
    {
        yield return new WaitForSeconds(1.5f);
        fading = true;
    }

    private IEnumerator WaitForGrampsToAppear()
    {
        yield return new WaitForSeconds(0.75f);

        // show first dialogue
        Dialogue(currGrampsDialogue);
    }

    private IEnumerator ClickToContinueTimer()
    {
        yield return new WaitForSeconds(2);

        // fade in click to continue
        clickToContinue.SetActive(true);
        clickToContinueText.alpha = 0;
        currVelocityClickToContinue = 0f;
        fadingClickToContinue = true;
    }

    public void ClickToContinue() // button
    {
        StopCoroutine(WaitClickToContinueTimer);
        clickToContinueText.alpha = 0;
        currVelocityClickToContinue = 0f;
        fadingClickToContinue = false;
        clickToContinue.SetActive(false);

        DialogueManager.instance.ClearDialogue();

        currGrampsDialogue++;
        Dialogue(currGrampsDialogue);
    }

    private IEnumerator WaitThenContinueNextScene()
    {
        yield return new WaitForSeconds(sceneLength);
        ProgressScene();
    }

    private void Update()
    {
        if (fading)
        {
            fadeToBlack.alpha = Mathf.SmoothDamp(fadeToBlack.alpha, 1, ref currVelocity, 0.75f);

            if ((1 - fadeToBlack.alpha) < 0.001f)
            {
                fading = false;
                AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
                SceneManager.LoadScene("TitleScreen");
            }
        }

        if (fadingClickToContinue)
        {
            clickToContinueText.alpha = Mathf.SmoothDamp(clickToContinueText.alpha, 1, ref currVelocityClickToContinue, 0.75f);

            if ((1 - clickToContinueText.alpha) < 0.001f)
            {
                fadingClickToContinue = false;
            }
        }
    }
}
