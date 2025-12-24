using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;
    private int currentPage = 0;
    [SerializeField] private GameObject pauseBlur;
    [SerializeField] private GameObject settingsBlur;
    [SerializeField] private GameObject finalPageNotNewGame; // final page is different if played through the settings/pause menu

    private bool isNewGame = false;

    public void Play(bool isFromNewGame)
    {
        isNewGame = isFromNewGame;
        if (!isFromNewGame)
        {
            pauseBlur.SetActive(false);
            settingsBlur.SetActive(false);

            // blur settings page?
            if (SceneManager.GetActiveScene().name == "Temp")
            {
                pauseBlur.SetActive(true);
            }
            else
            {
                settingsBlur.SetActive(true);
            }

            currentPage = 1;
            pages[currentPage].SetActive(true);
        }
        else
        {
            Time.timeScale = 0;
            currentPage = 0;
            pages[currentPage].SetActive(true);
        }
    }

    public void OnNextPage()
    {
        if (!isNewGame && currentPage == 5)  // last page in new game is different than the last page in settings/pause
        {
            pages[currentPage].SetActive(false);
            finalPageNotNewGame.SetActive(true);
            finalPageNotNewGame.transform.Find("Image").GetComponent<Animator>().Play("page");
        }
        else
        {
            pages[currentPage].SetActive(false);
            currentPage++;
            pages[currentPage].SetActive(true);
            pages[currentPage].transform.Find("Image").GetComponent<Animator>().Play("page");
        }

    }

    public void OnFinish()
    {
        if (SceneManager.GetActiveScene().name == "Temp")
        {
            pauseBlur.SetActive(false);
        }
        else
        {
            settingsBlur.SetActive(false);
        }

        if (!isNewGame)
        {
            finalPageNotNewGame.SetActive(false);
            currentPage = 1;
            pages[currentPage].SetActive(true);
        }
        else
        {
            pages[currentPage].SetActive(false);
            currentPage = 1; // if played from settings or pause, start from page 1, not 0
            pages[currentPage].SetActive(true);
            Time.timeScale = 1;
        }

        gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Temp" && LevelManager.instance.lmState == LevelManager.LMState.Setup)
        {
            LevelManager.instance.SetState(LevelManager.LMState.Game);
        }
    }

    public void SetSettingsBlur()
    {
        settingsBlur.SetActive(true);
    }
}
