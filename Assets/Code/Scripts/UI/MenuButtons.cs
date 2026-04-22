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

            if (PlayerPrefs.GetInt("IntroMailSeen") == -1)  // reset
            {
                LoadFile();
            }
        }

        SceneManager.LoadScene("Home");
    }

    public void LoadFile()
    {
        savePath = Application.persistentDataPath + "/Inbox.json";

        // Always reset to default — wipes any existing save
        TextAsset defaultFile = Resources.Load<TextAsset>("Inbox");
        plotData = JsonUtility.FromJson<PlotData>(defaultFile.text);

        SaveFile();
        PlayerPrefs.SetInt("IntroMailSeen", 1);
    }

    public void SaveFile()
    {
        File.WriteAllText(savePath, JsonUtility.ToJson(plotData, true));
    }

    public void ReadFile()
    {
        savePath = Application.persistentDataPath + "/Inbox.json";

        if (File.Exists(savePath))
        {
            string savedJson = File.ReadAllText(savePath);
            plotData = JsonUtility.FromJson<PlotData>(savedJson);
        }
        else
        {
            LoadFile();
        }
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
