using System.Collections;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
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
    private int currSceneIdx;

    // minigame
    private bool certificateShown;
    [SerializeField] private GameObject minigameParent;
    [SerializeField] private GameObject certificateParent;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private GameObject[] debugObjects;


    private void Awake()
    {
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
        currSceneIdx = 0;

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

                    if (prevState == CutsceneState.certificate)
                    {
                        minigameParent.SetActive(false);
                        certificateParent.SetActive(false);
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
            case CutsceneState.certificate:
                {
                    certificateParent.SetActive(true);
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
}
