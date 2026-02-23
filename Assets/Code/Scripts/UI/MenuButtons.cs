using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject backgroundPanel;
    [SerializeField] private GameObject loadingScreenPanel;
    public void quitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void startGame()
    {
        if (PlayerPrefs.GetInt("newGame") != -1)
        {
            Debug.Log("newGame");
            PlayerPrefs.SetInt("newGame", 1);
            PlayerPrefs.SetInt("kioskStyle", 0);
        }

        loadingScreenPanel.SetActive(true);
        loadingScreenPanel.GetComponent<LoadingScreen>().PlayLoad();
    }

    public void Settings()
    {
        //SceneManager.LoadScene("Settings");
        settingsPanel.SetActive(true);
        backgroundPanel.SetActive(true);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
