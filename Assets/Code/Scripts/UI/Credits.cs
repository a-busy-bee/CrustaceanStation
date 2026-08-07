using UnityEngine;
public class Credits : MonoBehaviour
{
    public void OnReturn()
    {
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.Home);
    }
}
