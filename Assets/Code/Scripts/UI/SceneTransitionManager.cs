using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance { get; private set; }
    public enum SceneType
    {
        BaseArea,
        Credits,
        Cutscene,
        EndingBad,
        EndingGood,
        Fired,
        Home,
        IsoRoom,
        Mailroom,
        TitleScreen,
        Tutorial,
        Vet
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void TransitionToScene(SceneType scene)
    {
        FadeToBlack.instance.FadeIn();

        StartCoroutine(WaitThenScene(scene));
    }

    private IEnumerator WaitThenScene(SceneType scene)
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(scene.ToString());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeToBlack.instance.FadeOut();
    }
}
