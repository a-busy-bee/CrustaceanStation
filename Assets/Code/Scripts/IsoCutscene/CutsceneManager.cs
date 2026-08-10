using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance { get; private set; }

    public enum CutsceneState
    {
        sceneImage,
        minigame,
        certificate
    }
    private CutsceneState currState;
    private CutsceneState prevState;
    private const int normalSceneLength = 2;
    private const int animatedSceneLength = 2;
    private float sceneLength = 2;
    private int animatedSceneIdx = 3;

    // scenes
    [SerializeField] private GameObject[] scenes;
    [SerializeField] private GameObject firstBlackBkg;
    [SerializeField] private GameObject secondBlackBkg;
    private int currSceneIdx = 0;

    // minigame
    private bool certificateShown;
    [SerializeField] private GameObject minigameParent;
    [SerializeField] private GameObject certificateParent;
    [SerializeField] private Image[] lastCutsceneIsoSprites;
    [SerializeField] private Image lastCutsceneIsoSpriteRolled;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private GameObject[] debugObjects;



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

        if (debug) sceneLength = 0.01f;
        else
        {
            for (int i = 0; i < debugObjects.Length; i++)
            {
                debugObjects[i].SetActive(false);
            }
        }

        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].SetActive(false);
        }
        minigameParent.SetActive(false);
        certificateParent.SetActive(false);
        firstBlackBkg.SetActive(false);
        secondBlackBkg.SetActive(false);

        currState = CutsceneState.sceneImage;

        firstBlackBkg.SetActive(true);
        scenes[currSceneIdx].SetActive(true);
        StartCoroutine(WaitThenContinueNextScene());
    }

    public void SetState(CutsceneState newState)
    {
        prevState = currState;
        currState = newState;
        switch (newState)
        {
            case CutsceneState.sceneImage:
                {
                    StartCoroutine(SwitchScenes());
                }
                break;
            case CutsceneState.minigame:
                {
                    StartCoroutine(StartMinigame());
                }
                break;
        }
    }

    public void ProgressScene()
    {
        if (currSceneIdx < animatedSceneIdx) // scenes before minigame
        {
            SetState(CutsceneState.sceneImage);
        }
        else if (currSceneIdx < 6 && certificateShown)  // scenes after minigame
        {
            SetState(CutsceneState.sceneImage);
        }
        else if (currSceneIdx == animatedSceneIdx) // scene exactly before minigame
        {
            SetState(CutsceneState.minigame);
        }
        else if (currSceneIdx == 6)
        {
            AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
            SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.Home);
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


        if (prevState == CutsceneState.minigame)
        {
            SetColorForLateCutsceneIsos();
            yield return new WaitForSeconds(0.5f);

            minigameParent.SetActive(false);
            certificateParent.SetActive(false);
            secondBlackBkg.SetActive(true);

        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        

        sceneLength = normalSceneLength;

        StartCoroutine(WaitThenContinueNextScene());
    }

    private IEnumerator StartMinigame()
    {
        FadeToBlack.instance.FadeIn();
        yield return new WaitForSeconds(0.5f);
        scenes[currSceneIdx].SetActive(false);
        minigameParent.SetActive(true);
        FadeToBlack.instance.FadeOut();
    }


    public void DebugNext()
    {
        if (currState == CutsceneState.sceneImage && currSceneIdx == 2)
        {
            SetState(CutsceneState.minigame);
        }
        else
        {
            ProgressScene();
        }
    }

    private IEnumerator WaitThenContinueNextScene()
    {
        yield return new WaitForSeconds(sceneLength);
        ProgressScene();
    }

    public void SetCertificateShown()
    {
        certificateShown = true;
    }

    private void SetColorForLateCutsceneIsos()
    {
        int color = SaveManager.instance.GetIso_Color();
        IsoMinigameManager.IsoColors isoColor = (IsoMinigameManager.IsoColors)color;
        Sprite walkSprite = IsoMinigameManager.instance.ConvertColorToWalkSprite(isoColor);
        Sprite rollSprite = IsoMinigameManager.instance.ConvertColorToRolledSprite(isoColor);

        foreach (Image iso in lastCutsceneIsoSprites)
        {
            iso.sprite = walkSprite;
        }

        lastCutsceneIsoSpriteRolled.sprite = rollSprite;
    }
}
