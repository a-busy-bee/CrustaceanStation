using UnityEngine;
using System.Collections;

public class CutscenePlayer : MonoBehaviour
{
    public static CutscenePlayer instance { get; private set; }

    private const int normalSceneLength = 2;
    //private const int animatedSceneLength = 2;
    private float sceneLength = 2;

    // scenes
    [SerializeField] private GameObject[] scenes;
    [SerializeField] private GameObject firstBlackBkg;
    [SerializeField] private CanvasGroup fadeToBlack;
    private int currSceneIdx = 0;
    private bool fading;
    private float currVelocity;


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

    public void ProgressScene()
    {
        if (currSceneIdx < 4) 
        {
            scenes[currSceneIdx].SetActive(false);
            currSceneIdx++;
            scenes[currSceneIdx].SetActive(true);

            if (currSceneIdx == 3) // if a scene is animated, but we don't have that rn
            {
                //scenes[currSceneIdx - 1].SetActive(true);
                //sceneLength = animatedSceneLength;
                sceneLength = normalSceneLength;
            }
            else sceneLength = normalSceneLength;

            StartCoroutine(WaitThenContinueNextScene());
        }
        else if (currSceneIdx == 4)
        {
            fading = true;
        }
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
                SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.TitleScreen);
            }
        }
    }
}
