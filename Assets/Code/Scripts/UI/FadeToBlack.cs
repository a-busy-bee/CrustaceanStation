using UnityEngine;

public class FadeToBlack : MonoBehaviour
{
    public static FadeToBlack instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject blackBkg;
    [SerializeField] private GameObject whiteBkg;
    private float speed = 0.25f;
    private float currVelocity;

    public enum FadeType
    {
        off,
        fadingIn,
        on,
        fadingOut
    }

    private FadeType fadeState;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    public void FadeIn(bool isWhite = false)
    {
        if (isWhite)
        {
            if (whiteBkg != null) whiteBkg.SetActive(true);
            blackBkg.SetActive(false);
        }
        else
        {
            if (whiteBkg != null) whiteBkg.SetActive(false);
            blackBkg.SetActive(true);
        }
        
        canvasGroup.blocksRaycasts = true;
        fadeState = FadeType.fadingIn;
    }

    public void FadeOut(bool isWhite = false)
    {
        if (isWhite)
        {
            if (whiteBkg != null) whiteBkg.SetActive(true);
            blackBkg.SetActive(false);
        }
        else
        {
            if (whiteBkg != null) whiteBkg.SetActive(false);
            blackBkg.SetActive(true);
        }

        fadeState = FadeType.fadingOut;
    }

    private void Update()
    {
        switch (fadeState)
        {
            case FadeType.fadingIn:

                canvasGroup.alpha = Mathf.SmoothDamp(canvasGroup.alpha, 1.0f, ref currVelocity, speed);

                if (1.0f - canvasGroup.alpha < 0.0001f)
                {
                    fadeState = FadeType.on;
                    currVelocity = 0.0f;
                }

                break;

            case FadeType.fadingOut:

                canvasGroup.alpha = Mathf.SmoothDamp(canvasGroup.alpha, 0.0f, ref currVelocity, speed);

                if (canvasGroup.alpha < 0.0001)
                {
                    fadeState = FadeType.off;
                    canvasGroup.blocksRaycasts = false;
                    currVelocity = 0.0f;
                }

                break;
        }
    }
}
