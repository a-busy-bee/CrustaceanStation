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
        Application.Quit();
    }

    public void startGame()
    {
        SaveManager saveManager = SaveManager.instance;

        bool isNewGame = saveManager.GetProgression_NewGame();
        if (isNewGame)
        {
            saveManager.SaveProgressionData(SaveManager.ProgressionType.newGame, true.ToString());

            bool isIntroMailSeen = saveManager.GetProgression_IntroMailSeen();
            if (!isIntroMailSeen)  // reset
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
        SaveManager.instance.SaveProgressionData(SaveManager.ProgressionType.introMailSeen, true.ToString());
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

    public void VisitVet()
    {
        SceneManager.LoadScene("Vet");
    }

    public void Settings()
    {
        //SceneManager.LoadScene("Settings");
        settingsPanel.SetActive(true);
        settingsPanel.GetComponent<Settings>().Show();
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
