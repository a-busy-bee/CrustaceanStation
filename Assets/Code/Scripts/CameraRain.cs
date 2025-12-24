using UnityEngine;

public class CameraRain : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    void Awake()
    {
        if (!renderTexture)
        {
            renderTexture = new RenderTexture(1024, 1024, 16);
            renderTexture.Create();
        }
    }
}
