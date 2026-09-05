using UnityEngine;
public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject translationPage;
    public void OnReturn()
    {
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.TitleScreen);
    }

    public void CredNextPage()
    {
        translationPage.SetActive(true);
    }

    public void CredPrevPage()
    {
        translationPage.SetActive(false);
    }
}
