using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingScreen : MonoBehaviour
{

    //[SerializeField] private GameObject imagenewGame;
    //[SerializeField] private Animator animator;
    [SerializeField] Slider slider;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private GameObject sliderParent;
    [SerializeField] private TextMeshProUGUI tooltipText;
    private float sinFreq = 20f;

    private string[] tooltips = {
        "Summoning crabs...",
        "Fighting off seagulls...",
        "Distracting whales...",
        "Finding Itty Bitty's lollipop",
        "Tip: families take up two seats",
        "Decorating the kiosk...",
        "Watching sunsets..."
    };

    private void Start()
    {
        //imagenewGame.SetActive(false);
        group.alpha = 0;
        group.blocksRaycasts = false;

        //animator.enabled = false;
    }

    public void PlayLoad(string sceneName)
    {
        //imagenewGame.SetActive(true);
        // TODO: convert animation to smooth damp 
        //animator.enabled = true;
        group.blocksRaycasts = true;
        group.alpha = 1;
        StartCoroutine(LoadSceneCoroutine(sceneName));
        //StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        Debug.Log("f");
        sliderParent.SetActive(true);
        slider.value = 0;
        tooltipText.text = tooltips[Random.Range(0, tooltips.Length)];

        while (slider.value < 0.67f) // fake some loading because we're evil and want people to look at the fire splash art
        {
            sinFreq = Random.Range(20, 50);
            float sinVal = Mathf.Sin(Time.time * sinFreq);
            if (sinVal < 0) sinVal = Random.Range(0, 1);

            slider.value += fadeSpeed * sinVal * Time.deltaTime;
            yield return null;
        }


        slider.value = 0.67f;

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!loadingOperation.isDone)
        {
            slider.value = loadingOperation.progress + 0.67f;
            yield return null;
        }

        slider.value = 1f;

        /*while (group.alpha > 0)
        {
            group.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
        group.alpha = 0;
        group.blocksRaycasts = false;
        gameObject.SetActive(false);*/
        Debug.Log("g");
    }

    /*private IEnumerator WaitForEndOfAnim(string sceneName)
    {
        Debug.Log("d");
        sliderParent.SetActive(false);
        group.blocksRaycasts = true;

        /*while (group.alpha < 1)
        {
            group.alpha += fadeSpeed * Time.deltaTime;
            yield return null;
        }
        group.alpha = 1;
        //animator.Play("NewGameLoad");

        //yield return new WaitForSeconds(1f);
        StartCoroutine(LoadSceneCoroutine(sceneName));
        Debug.Log("e");
    }*/
}
