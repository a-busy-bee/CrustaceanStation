using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private float sceneLength = 2;

    // scenes
    [SerializeField] private GameObject[] scenes;
    private int currSceneIdx = 0;

    // minigame
    private bool certificateShown;
    [SerializeField] private GameObject minigameParent;
    [SerializeField] private GameObject certificateParent;
    [SerializeField] private Image[] lastCutsceneIsoSprites;

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

        currState = CutsceneState.sceneImage;

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
                    scenes[currSceneIdx].SetActive(false);
                    currSceneIdx++;
                    scenes[currSceneIdx].SetActive(true);

                    if (prevState == CutsceneState.minigame)
                    {
                        minigameParent.SetActive(false);
                        certificateParent.SetActive(false);

                        SetColorForLateCutsceneIsos();
                    }

                    StartCoroutine(WaitThenContinueNextScene());
                }
                break;
            case CutsceneState.minigame:
                {
                    scenes[currSceneIdx].SetActive(false);
                    minigameParent.SetActive(true);
                }
                break;
        }
    }

    public void ProgressScene()
    {
        if (currSceneIdx < 2) // scenes before minigame
        {
            SetState(CutsceneState.sceneImage);
        }
        else if (currSceneIdx < 5 && certificateShown)  // scenes after minigame
        {
            SetState(CutsceneState.sceneImage);
        }
        else if (currSceneIdx == 2) // scene exactly before minigame
        {
            SetState(CutsceneState.minigame);
        }
        else if (currSceneIdx == 5)
        {
            SceneManager.LoadScene("Home");
        }
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
        Debug.Log("what");
        Color isoColor = Color.white;
        
        string hex = "#" + PlayerPrefs.GetString("IsoColor");
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            isoColor = color;
        }


        foreach (Image iso in lastCutsceneIsoSprites)
        {
            iso.color = isoColor;
        }
    }
}
