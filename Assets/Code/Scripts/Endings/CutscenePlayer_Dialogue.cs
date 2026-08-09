using UnityEngine;
using System.Collections;

public class CutscenePlayer_Dialogue : MonoBehaviour
{
    public static CutscenePlayer_Dialogue instance { get; private set; }

    private const int normalSceneLength = 2;
    private float sceneLength = 2;

    // scenes
    [SerializeField] private GameObject[] scenes;
    [SerializeField] private GameObject firstBlackBkg;
    [SerializeField] private CanvasGroup fadeToBlack;
    [SerializeField] private CanvasGroup clickToContinueText;
    [SerializeField] private GameObject clickToContinue;
    [SerializeField] private GameObject character;
    private int currSceneIdx = 0;
    private bool fading;
    private float currVelocity;

    // dialogue
    private int currDialogue = 0;
    private IEnumerator WaitClickToContinueTimer;
    private bool fadingClickToContinue;
    private float currVelocityClickToContinue;

    // variables across diff instances
    public enum NextScene
    {
        TitleScreen,
        Home,
        EndingGood,
        EndingBad
    }
    public enum DialogueType
    {
        grampsEnding,
        vet1,
        vet2,
        vet3,
        vet4good,
        vet4bad
    }
    [Header("Instance-Specific")]
    [SerializeField] private NextScene nextScene;
    [SerializeField] private DialogueType dialogueType;
    [SerializeField] private int numScenesBeforeDialogue = 0; // 3
    [SerializeField] private int animatedScene = -1; // 2
    [SerializeField] private float charTargetX;
    [SerializeField] private float charTargetY;
    private int numDialogues = 0; // 39

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
        if (scenes.Length != 0) scenes[currSceneIdx].SetActive(true);
        fadeToBlack.alpha = 0;

        if (dialogueType == DialogueType.grampsEnding)
        {
            StartCoroutine(WaitThenContinueNextScene());
        }
    }

    public void SetDialogueType(DialogueType newType)
    {
        dialogueType = newType;
        StartCoroutine(WaitThenContinueNextScene());
    }

    public void SetNextScene(NextScene newScene)
    {
        nextScene = newScene;
    }

    public void ProgressScene()
    {
        if (currSceneIdx < numScenesBeforeDialogue) // scenes before dialogue
        {
            StartCoroutine(SwitchScenes());
        }
        else if (currSceneIdx == numScenesBeforeDialogue)
        {
            numDialogues = DialogueManager.instance.GetNumDialoguesForCharacterLongDialogue(dialogueType);
            AppearCharacter();
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

        if (currSceneIdx == animatedScene) // if a scene is animated, but we don't have that rn
        {
            //scenes[currSceneIdx - 1].SetActive(true);
            //sceneLength = animatedSceneLength;
            sceneLength = normalSceneLength;
        }
        else sceneLength = normalSceneLength;

        StartCoroutine(WaitThenContinueNextScene());
    }

    private void AppearCharacter()
    {
        // appear character
        character.GetComponent<SmoothLerp>().Move(new Vector2(charTargetX, charTargetY), 0.5f);

        StartCoroutine(WaitForCharacterToAppear());
    }

    private void Dialogue(int idx)
    {
        if (currDialogue == numDialogues)
        {
            StartCoroutine(WaitThenFade());
            return;
        }

        // show dialogue at idx
        DialogueManager.instance.ShowCharacterLongDialogue(dialogueType, idx);

        // if vet
        if (dialogueType == DialogueType.vet1 && currDialogue == 18)
        {
            VetManager.instance.ShowMeds();
        }
        else if (dialogueType > DialogueType.grampsEnding && dialogueType < DialogueType.vet4bad && currDialogue == numDialogues - 1)
        {
            VetManager.instance.PlayIsoSad();
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

    private IEnumerator WaitForCharacterToAppear()
    {
        yield return new WaitForSeconds(0.75f);

        // show first dialogue
        Dialogue(currDialogue);
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
        DialogueObject.DialogueState state = DialogueManager.instance.GetDialogueState();
        if (state == DialogueObject.DialogueState.Typing)
        {
            DialogueManager.instance.ClearDialogue();
            return;
        }

        StopCoroutine(WaitClickToContinueTimer);
        clickToContinueText.alpha = 0;
        currVelocityClickToContinue = 0f;
        fadingClickToContinue = false;
        clickToContinue.SetActive(false);

        DialogueManager.instance.ClearDialogue();

        currDialogue++;
        StartCoroutine(WaitForHideThenShowNext());
        
    }

    private IEnumerator WaitForHideThenShowNext()
    {
        while (DialogueManager.instance.GetDialogueState() != DialogueObject.DialogueState.NotAppeared) yield return null;
        Dialogue(currDialogue);
    }

    private IEnumerator WaitThenContinueNextScene()
    {
        if (scenes.Length == 0) yield return new WaitForSeconds(0);
        else yield return new WaitForSeconds(sceneLength);

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

                SceneTransitionManager.SceneType scene = SceneTransitionManager.SceneType.TitleScreen;
                switch (nextScene)
                {
                    case NextScene.TitleScreen:
                        scene = SceneTransitionManager.SceneType.TitleScreen;
                        break;

                    case NextScene.Home:
                        SaveManager.instance.SetProgression_IncrementCurrDay();
                        scene = SceneTransitionManager.SceneType.Home;
                        break;

                    case NextScene.EndingGood:
                        AchievementManager.instance.UnlockAchievementBool(AchievementManager.AchievementTypeBool.soLong);
                        scene = SceneTransitionManager.SceneType.EndingGood;
                        break;

                    case NextScene.EndingBad:
                        AchievementManager.instance.UnlockAchievementBool(AchievementManager.AchievementTypeBool.whatHaveIDone);
                        scene = SceneTransitionManager.SceneType.EndingBad;
                        break;
                }
                SceneTransitionManager.instance.TransitionToScene(scene);
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
