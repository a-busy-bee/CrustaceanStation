using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutscenePlayer : MonoBehaviour
{
    public static CutscenePlayer instance { get; private set; }

    private const int normalSceneLength = 2;
    //private const int animatedSceneLength = 2;
    private float sceneLength = 2;

    // scenes
    [Header("Scenes")]
    [SerializeField] private GameObject[] scenes;
    [SerializeField] private GameObject firstBlackBkg;
    [SerializeField] private CanvasGroup fadeToBlack;
    private int currSceneIdx = 0;
    private bool fading;
    private float currVelocity;

    [Header("Ending-specific")]
    [SerializeField] private Image isoCurledUp;
    [SerializeField] private Image isoHealed;
    [SerializeField] private Sprite[] curledUpSprites;
    [SerializeField] private Sprite[] healedSprites;

    [Header("Misc")]
    [SerializeField] private int maxSceneIdx = 4;
    [SerializeField] private int animatedSceneIdx = 3;
    [SerializeField] private SceneTransitionManager.SceneType nextScene = SceneTransitionManager.SceneType.TitleScreen;

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

        firstBlackBkg.SetActive(false);

        firstBlackBkg.SetActive(true);
        scenes[currSceneIdx].SetActive(true);
        fadeToBlack.alpha = 0;

        StartCoroutine(WaitThenContinueNextScene());
    }

    private void Start()
    {
        int colorIdx = SaveManager.instance.GetIso_Color();
        if (isoCurledUp != null) isoCurledUp.sprite = curledUpSprites[colorIdx];
        if (isoHealed != null) isoHealed.sprite = healedSprites[colorIdx];
    }

    public void ProgressScene()
    {
        if (currSceneIdx < maxSceneIdx)
        {
            StartCoroutine(SwitchScenes());
        }
        else if (currSceneIdx == maxSceneIdx)
        {
            fading = true;
        }
    }

    private IEnumerator SwitchScenes()
    {
        FadeToBlack.instance.FadeIn();
        yield return new WaitForSeconds(0.5f);
        scenes[currSceneIdx].SetActive(false);
        currSceneIdx++;
        scenes[currSceneIdx].SetActive(true);
        FadeToBlack.instance.FadeOut();
        yield return new WaitForSeconds(0.5f);

        if (currSceneIdx == animatedSceneIdx) // if a scene is animated, but we don't have that rn
        {
            //scenes[currSceneIdx - 1].SetActive(true);
            //sceneLength = animatedSceneLength;
            sceneLength = normalSceneLength;
        }
        else sceneLength = normalSceneLength;

        StartCoroutine(WaitThenContinueNextScene());
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
                SceneTransitionManager.instance.TransitionToScene(nextScene);
            }
        }
    }
}
