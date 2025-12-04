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

        if (SceneManager.GetActiveScene().name != "Temp")
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

            if (SceneManager.GetActiveScene().name != "Temp")
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
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("ResetCrabdex", 1);
        PlayerPrefs.SetInt("ResetDecor", 1);
        areYouSurePanel.SetActive(false);
    }

    public void OnResetNo()
    {
        areYouSurePanel.SetActive(false);
    }
}
