using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject backgroundPanel;

    private string defaultPath;
    private string savePath;
    private PlotData plotData;
    
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

            if (PlayerPrefs.GetInt("IntroMailSeen") == -1)
            {
                LoadFile();
            }
        }

        SceneManager.LoadScene("Home");
        //loadingScreenPanel.SetActive(true);
        //loadingScreenPanel.GetComponent<LoadingScreen>().PlayLoad("Home");
    }

    public void LoadFile()
    {
        defaultPath = Application.dataPath + "/Data/Inbox.json";
        savePath = Application.persistentDataPath + "/Inbox.json";

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        // load default file from resources
        string jsonText = File.ReadAllText(defaultPath);
        plotData = JsonUtility.FromJson<PlotData>(jsonText);

        File.WriteAllText(savePath, JsonUtility.ToJson(plotData, true));

        PlayerPrefs.SetInt("IntroMailSeen", 1);
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

    public void BackToMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
