using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject backgroundPanel;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("coins", 100);
        PlayerPrefs.SetInt("numTracks", 3);
        PlayerPrefs.SetInt("crabDropRate", 3);
        PlayerPrefs.SetInt("cartQuality", 3);
    }

    public void quitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void startGame()
    {
        SceneManager.LoadScene("Temp");
    }

    public void Settings()
    {
        //SceneManager.LoadScene("Settings");
        settingsPanel.SetActive(true);
        backgroundPanel.SetActive(true);
    }

    public void Credits()
    {
        //SceneManager.LoadScene("Credits");
    }
}
