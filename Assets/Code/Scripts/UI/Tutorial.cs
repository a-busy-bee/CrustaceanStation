using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject[] pages;
    private int currentPage = 0;
    [SerializeField] GameObject pauseBlur;
    [SerializeField] GameObject settingsBlur;

    // TODO: Turn off last page if it's not the first time
    private void Awake()
    {
        Debug.Log(currentPage);
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

        currentPage = 0;
        pages[currentPage].SetActive(true);
    }
    public void OnNextPage()
    {
        pages[currentPage].SetActive(false);
        currentPage++;
        pages[currentPage].SetActive(true);
        pages[currentPage].transform.Find("Image").GetComponent<Animator>().Play("page");
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

        pages[currentPage].SetActive(false);
        currentPage = 0;
        pages[currentPage].SetActive(true);
        
        gameObject.SetActive(false);
        Debug.Log(currentPage);
    }

    public void SetSettingsBlur()
    {
        settingsBlur.SetActive(true);
    }
}
