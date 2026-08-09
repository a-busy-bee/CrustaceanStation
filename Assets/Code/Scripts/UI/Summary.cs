using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Summary : MonoBehaviour
{
    [SerializeField] private GameObject screendim;
    private void Start()
    {
        screendim.SetActive(false);

        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1120);
        GetComponent<SmoothLerp>().Move(new Vector2(0, 0), 0.75f);

        HeadlineManager.instance.GetHeadline();
        
        StartCoroutine(WaitThenDim());
    }

    private IEnumerator WaitThenDim()
    {
        yield return new WaitForSeconds(0.75f);

        screendim.SetActive(true);
    }

    public void Continue()
    {
        AudioManager.instance.SwitchTheme(AudioManager.ThemeNames.CheckingIntoStation);
        SceneTransitionManager.instance.TransitionToScene(SceneTransitionManager.SceneType.Home);
    }
}
