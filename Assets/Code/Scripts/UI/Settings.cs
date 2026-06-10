using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using NUnit.Framework;
public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private GameObject backgroundDisplay;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject areYouSurePanel;

    [SerializeField] private RectTransform rectTransform;
    private Vector2 onPos = new Vector2(16, -81);
    private Vector2 offPos = new Vector2(16, 1198);
    private Vector2 currVelocity;
    private bool moving;
    private bool displayed;

    private void Start()
    {
        displayed = false;
        moving = false;
        rectTransform.anchoredPosition = offPos;
    }
    public void OnReturn()
    {
        //Assert.IsTrue(displayed);
        //gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name != "BaseArea") backgroundDisplay.SetActive(false);

        if (areYouSurePanel != null) areYouSurePanel.SetActive(false);

        moving = true;
    }

    public void Show()
    {
        moving = true;
        displayed = false;
    }

    public bool IsDisplayed()
    {
        return displayed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && displayed) OnReturn();

        if (moving)
        {
            if (displayed)
            {
                rectTransform.anchoredPosition = Vector2.SmoothDamp(rectTransform.anchoredPosition, offPos, ref currVelocity, 0.25f, Mathf.Infinity, Time.unscaledDeltaTime);
                if (Vector2.Distance(rectTransform.anchoredPosition, offPos) < 10f)
                {
                    moving = false;
                    displayed = false;

                }
            }
            // bring the screen up
            else
            {
                rectTransform.anchoredPosition = Vector2.SmoothDamp(rectTransform.anchoredPosition, onPos, ref currVelocity, 0.25f, Mathf.Infinity, Time.unscaledDeltaTime);
                if (Vector2.Distance(rectTransform.anchoredPosition, onPos) < 10f)
                {
                    moving = false;
                    displayed = true;
                }

            }
        }
    }

    public void OnTutorial()
    {
        //tutorial.SetActive(true);
        //tutorial.GetComponent<Tutorial>().Play(false);
        //tutorial.GetComponent<Tutorial>().SetSettingsBlur();
        loadingScreenPanel.SetActive(true);
        loadingScreenPanel.GetComponent<LoadingScreen>().PlayLoad("Tutorial");
    }

    public void Reset()
    {
        areYouSurePanel.SetActive(true);
    }

    public void OnResetYes()
    {
        //PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("ResetCrabdex", 1);
        PlayerPrefs.SetInt("ResetDecor", 1);
        PlayerPrefs.SetInt("kioskStyle", 0);
        PlayerPrefs.SetInt("newGame", 1);
        PlayerPrefs.SetInt("IntroMailSeen", -1);
        
        PlayerPrefs.SetInt("cartQuality", 0);
        PlayerPrefs.SetInt("numTracks", 0);
        PlayerPrefs.SetInt("crabDropRate", 0);
        PlayerPrefs.SetInt("decor_top", 0);
        PlayerPrefs.SetInt("decor_left", 0);
        PlayerPrefs.SetInt("decor_right", 0);

        PlayerPrefs.SetInt("first day", 0);

        PlayerPrefs.SetInt("coins", 0);

        areYouSurePanel.SetActive(false);
    }


    public void OnResetNo()
    {
        areYouSurePanel.SetActive(false);
    }
}
