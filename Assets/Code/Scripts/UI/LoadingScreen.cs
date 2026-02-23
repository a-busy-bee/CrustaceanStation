using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingScreen : MonoBehaviour
{

    [SerializeField] private GameObject imagenewGame;
    [SerializeField] private Animator animator;
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
        "Tip: whales take up two seats",
        "Tip: Don't forget to check the name on the ticket",
        "Tip: Try tapping the shopkeeper",
        "Tapping shopkeepers...",
        "Decorating the kiosk...",
        "Watching sunsets..."
    };

    private void Start()
    {
        imagenewGame.SetActive(false);
        group.alpha = 0;
        group.blocksRaycasts = false;

        animator.enabled = false;
    }
    public void PlayLoad()
    {
        imagenewGame.SetActive(true);
        animator.enabled = true;
        StartCoroutine(WaitForEndOfAnim());

        
    }
    private IEnumerator LoadSceneCoroutine()
    {
        sliderParent.SetActive(true);
        slider.value = 0;
        tooltipText.text = tooltips[Random.Range(0, tooltips.Length)];

        while (slider.value < 0.67f)
        {
            sinFreq = Random.Range(20, 50);
            float sinVal = Mathf.Sin(Time.time * sinFreq);
            if (sinVal < 0) sinVal = Random.Range(0, 1);

            slider.value += fadeSpeed * sinVal * Time.deltaTime;
            yield return null;
        }


        slider.value = 0.67f;

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("Home");

        while (!loadingOperation.isDone)
        {
            slider.value = loadingOperation.progress + 0.67f;
            yield return null;
        }

        slider.value = 1f;

        while (group.alpha > 1)
        {
            group.alpha -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
        group.alpha = 0;
        group.blocksRaycasts = false;
    }

    private IEnumerator WaitForEndOfAnim()
    {

        group.blocksRaycasts = true;

        while (group.alpha < 1)
        {
            group.alpha += fadeSpeed * Time.deltaTime;
            yield return null;
        }
        group.alpha = 1;

        animator.Play("NewGameLoad");
        sliderParent.SetActive(false);

        yield return new WaitForSeconds(4.2f);

        StartCoroutine(LoadSceneCoroutine());
    }
}
