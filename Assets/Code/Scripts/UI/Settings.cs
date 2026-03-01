using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject backgroundDisplay;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject areYouSurePanel;
    public void OnReturn()
    {
        gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name != "BaseArea")
        {
            backgroundDisplay.SetActive(false);
        }

        areYouSurePanel.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);

            if (SceneManager.GetActiveScene().name != "BaseArea")
            {
                backgroundDisplay.SetActive(false);
            }
        }
    }

    public void OnTutorial()
    {
        tutorial.SetActive(true);
        tutorial.GetComponent<Tutorial>().Play(false);
        tutorial.GetComponent<Tutorial>().SetSettingsBlur();
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
        
        PlayerPrefs.SetInt("cartQuality", 0);
        PlayerPrefs.SetInt("numTracks", 0);
        PlayerPrefs.SetInt("crabDropRate", 0);
        PlayerPrefs.SetInt("decor_top", 0);
        PlayerPrefs.SetInt("decor_left", 0);
        PlayerPrefs.SetInt("decor_right", 0);

        PlayerPrefs.SetInt("coins", 0);

        areYouSurePanel.SetActive(false);
    }

    public void OnResetNo()
    {
        areYouSurePanel.SetActive(false);
    }
}
