using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class HomeManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private GameObject crabdexNotif;
    [SerializeField] private GameObject mailroomNotif;
    
    private PlotData plotData;
    private string defaultPath;
    private string savePath;


    private void Start()
    {
        crabdexNotif.SetActive(false);
        mailroomNotif.SetActive(false);

        //TODO: figure out if crabdex has new entries
        //TODO: figure out if mailroom has new entries
        LoadJSON();

        if (plotData.inbox.Count != 0) mailroomNotif.SetActive(true);
    }

    private void LoadJSON()
    {
        defaultPath = Application.dataPath + "/Data/Inbox.json";
        savePath = Application.persistentDataPath + "/Inbox.json";

        if (File.Exists(savePath))
        {
            plotData = JsonUtility.FromJson<PlotData>(File.ReadAllText(savePath));
        }
        else
        {
            // load default file from resources
            string jsonText = File.ReadAllText(defaultPath);
            plotData = JsonUtility.FromJson<PlotData>(jsonText);

            File.WriteAllText(savePath, JsonUtility.ToJson(plotData, true));
        }
    }


    public void startDay(string sceneName)
    {
        if (PlayerPrefs.GetInt("newGame") == 1)
        {
            loadingScreenPanel.SetActive(true);
            loadingScreenPanel.GetComponent<LoadingScreen>().PlayLoad("Tutorial");
            return;
        }

        loadingScreenPanel.SetActive(true);
        loadingScreenPanel.GetComponent<LoadingScreen>().PlayLoad(sceneName);
    }
    public void OpenCrabdex()
    {
        Crabdex.instance.ShowCodex();
    }

    public void GoToMailRoom()
    {
        SceneManager.LoadScene("Mailroom");
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
