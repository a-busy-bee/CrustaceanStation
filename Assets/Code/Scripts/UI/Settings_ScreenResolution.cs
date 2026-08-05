using TMPro;
using UnityEngine;

public class Settings_ScreenResolution : MonoBehaviour
{
    private Vector2Int[] resolutions = new Vector2Int[] {
        new Vector2Int(1280, 720),
        new Vector2Int(1920, 1080),
        new Vector2Int(1920, 1200),
        new Vector2Int(2560, 1440),
        new Vector2Int(2560, 1600),
        new Vector2Int(3840, 2160)
    };
    private int currIdx = -1;
    private int currResolutionIdx;
    private bool isFullscreen = true;

    [SerializeField] private TextMeshProUGUI resolutionText;

    private void Start()
    {
        int x = SaveManager.instance.GetSettings_ResolutionX();
        int y = SaveManager.instance.GetSettings_ResolutionY();

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].x == x && resolutions[i].y == y)
            {
                currIdx = i;
                SetResolutionText(currIdx);
            }
        }

        if (currIdx == -1) // no resolution saved/found
        {
            currIdx = 0;
            SetResolutionText(currIdx);
        }
    }

    public void NextResolution()
    {
        currIdx++;
        if (currIdx == resolutions.Length) currIdx = 0;

        SetResolutionText(currIdx);
        ApplyResolution();
    }

    public void PrevResolution()
    {
        currIdx--;
        if (currIdx == -1) currIdx = resolutions.Length - 1;

        SetResolutionText(currIdx);
        ApplyResolution();
    }

    public void ApplyResolution()
    {
        currResolutionIdx = currIdx;
        Screen.SetResolution(resolutions[currIdx].x, resolutions[currIdx].y, isFullscreen);
    }

    public void ToggleFullScreen()
    {
        isFullscreen = !isFullscreen;
        Screen.SetResolution(resolutions[currResolutionIdx].x, resolutions[currResolutionIdx].y, isFullscreen);
    }

    private void SetResolutionText(int idx)
    {
        resolutionText.text = resolutions[idx].x.ToString() + "x" + resolutions[idx].y.ToString();
    }
    

}
