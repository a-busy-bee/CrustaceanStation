using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class HomeManager : MonoBehaviour
{
    public static HomeManager instance { get; protected set; }
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private GameObject crabdexNotif;
    [SerializeField] private GameObject mailroomNotif;
    [SerializeField] private GameObject isoRoomButton;
    [SerializeField] private GameObject goToWorkButton;

    private PlotData plotData;
    private string defaultPath;
    private string savePath;

	private void Awake()
	{
		if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

	}

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
        defaultPath = Path.Combine(Application.streamingAssetsPath, "Data", "Inbox.json");
        savePath = Path.Combine(Application.persistentDataPath, "Inbox.json");

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
        bool isNewGame = SaveManager.instance.GetProgression_NewGame();
        if (isNewGame)
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
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.Mailroom);
        SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.Mailroom);
    }

    public void GoToIsoRoom()
    {
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.Iso);
        SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.IsoRoom);
    }

    public void BackToMenu()
    {
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.TitleScreen);
    }

    public void SetIsoRoomButtonActive(bool isActive)
    {
        isoRoomButton.SetActive(isActive);
    }

    public void SetGoToWorkButtonActive(bool isActive)
    {
        goToWorkButton.SetActive(isActive);
    }
}
