using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class WeatherManager : MonoBehaviour
{
    public static WeatherManager instance { get; private set; }

    [SerializeField] private WeatherType[] types; // sunny > rain > fog
    private WeatherType currentType;

    [Header("Foreground")]
    [SerializeField] private Image cloudsTop;
    [SerializeField] private Image backgroundTop;
    [SerializeField] private Material multiply;

    [Header("Background")]
    [SerializeField] private Image cloudsBottom;
    [SerializeField] private Image backgroundBottom;
    [SerializeField] private Image backgroundMultiplyLayer;

    [Header("Rain Images")]
    [SerializeField] private GameObject rainImage;
    [SerializeField] private GameObject rainImageBkg;

    [Header("Fog")]
    [SerializeField] private GameObject fogOverlay;
    [SerializeField] private GameObject groundClouds;

    // transitions
    private float transitionTime = 0f;
    private float duration = 2f;
    private bool isTransitioning;
    private bool wasFoggy;
    private bool isRainy;
    private bool wasRainy;
    private Color goalCloudTop;
    private Color goalBkgTop;
    private Color goalCloudBottom;
    private Color goalBkgBottom;
    private Color startCloudTop;
    private Color startBkgTop;
    private Color startCloudBottom;
    private Color startBkgBottom;
    private float startFogAlpha;
    private float startGroundAlpha;
    private const float maxRainAlpha = 0.04f;


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

        // reset in case something was left on
        startFogAlpha = fogOverlay.GetComponent<Image>().color.a;
        startGroundAlpha = groundClouds.GetComponent<Image>().color.a;
        fogOverlay.SetActive(false);
        groundClouds.SetActive(false);

        // choose random state
        int startingWeather = Random.Range(0, 3);
        WeatherType startingType = types[startingWeather];
        currentType = startingType;

        cloudsTop.color = startingType.cloudsTop;
        backgroundTop.color = startingType.backgroundTop;
        cloudsBottom.color = startingType.cloudsBottom;
        backgroundBottom.color = startingType.backgroundBottom;

        backgroundTop.material = null;

        if (startingType.isFoggy)
        {
            // turn on fog
            fogOverlay.SetActive(true);
            groundClouds.SetActive(true);
            wasFoggy = true;
        }
        else if (startingType.isRainy)
        {
            rainImageBkg.SetActive(true);
            rainImage.SetActive(true);
            isRainy = true;

            backgroundTop.material = multiply;

            Color color = backgroundMultiplyLayer.color;
            color.a = 0.67f;
            backgroundMultiplyLayer.color = color;
        }
    }

    public void ChangeWeather()
    {
        startCloudTop = cloudsTop.color;
        startBkgTop = backgroundTop.color;
        startCloudBottom = cloudsBottom.color;
        startBkgBottom = backgroundBottom.color;
        //backgroundTop.material = multiply;

        int changeIdx = Random.Range(0, 4);

        if (changeIdx == 0 || changeIdx == 2 && !(currentType.type == "sunny"))
        {
            MakeSunny();
        }
        else if (changeIdx == 1 && !(currentType.type == "lightRain" || currentType.type == "darkRain"))
        {
            MakeRainy();
        }
    }

    [ContextMenu("Make sunny")]
    private void MakeSunny()
    {
        isTransitioning = true;


        if (isRainy)
        {
            isRainy = false;
            wasRainy = true;
        }

        int newWeather = 0;

        currentType = types[newWeather];

        goalCloudTop = types[newWeather].cloudsTop;
        goalBkgTop = types[newWeather].backgroundTop;
        goalCloudBottom = types[newWeather].cloudsBottom;
        goalBkgBottom = types[newWeather].backgroundBottom;
    }

    [ContextMenu("Make rainy")]
    private void MakeRainy()
    {
        backgroundTop.material = multiply;
        isTransitioning = true;


        rainImage.SetActive(true);
        rainImageBkg.SetActive(true);

        if (!isRainy)
        {
            isRainy = true;
            wasRainy = false;
        }

        int newWeather = 1;
        currentType = types[newWeather];

        goalCloudTop = types[newWeather].cloudsTop;
        goalBkgTop = types[newWeather].backgroundTop;
        goalCloudBottom = types[newWeather].cloudsBottom;
        goalBkgBottom = types[newWeather].backgroundBottom;
    }

    private void Update()
    {
        if (isTransitioning)
        {
            transitionTime += Time.deltaTime;
            float t = transitionTime / duration;

            cloudsTop.color = Color.Lerp(startCloudTop, goalCloudTop, t);
            backgroundTop.color = Color.Lerp(startBkgTop, goalBkgTop, t);
            cloudsBottom.color = Color.Lerp(startCloudBottom, goalCloudBottom, t);
            backgroundBottom.color = Color.Lerp(startBkgBottom, goalBkgBottom, t);

            if (wasFoggy)
            {
                Color fog = fogOverlay.GetComponent<Image>().color;
                fog.a = Mathf.Lerp(startFogAlpha, 0, t);
                fogOverlay.GetComponent<Image>().color = fog;

                Color groundCloudColor = groundClouds.GetComponent<Image>().color;
                groundCloudColor.a = Mathf.Lerp(startGroundAlpha, 0, t);
                groundClouds.GetComponent<Image>().color = groundCloudColor;
            }

            if (isRainy)
            {
                Color rainAlpha = rainImage.GetComponent<Image>().color;
                rainAlpha.a = Mathf.Lerp(0, maxRainAlpha, t);
                rainImage.GetComponent<Image>().color = rainAlpha;
                rainImageBkg.GetComponent<Image>().color = rainAlpha;

                Color rainMultiplyAlpha = backgroundMultiplyLayer.color;
                rainMultiplyAlpha.a = Mathf.Lerp(0, 0.67f, t);
                backgroundMultiplyLayer.color = rainMultiplyAlpha;
            }
            else if (wasRainy)
            {
                Color rainAlpha = rainImage.GetComponent<Image>().color;
                rainAlpha.a = Mathf.Lerp(maxRainAlpha, 0, t);
                rainImage.GetComponent<Image>().color = rainAlpha;
                rainImageBkg.GetComponent<Image>().color = rainAlpha;

                Color rainMultiplyAlpha = backgroundMultiplyLayer.color;
                rainMultiplyAlpha.a = Mathf.Lerp(0.67f, 0, t);
                backgroundMultiplyLayer.color = rainMultiplyAlpha;
            }

            if (t >= 1f)
            {
                isTransitioning = false;
                transitionTime = 0;

                if (wasFoggy)
                {
                    wasFoggy = false;
                    fogOverlay.SetActive(false);
                    groundClouds.SetActive(false);
                }
                if (wasRainy)
                {
                    wasRainy = false;
                    isRainy = false;

                    rainImage.SetActive(false);
                    rainImageBkg.SetActive(false);
                }
            }
        }
    }

    public WeatherType GetCurrentWeather()
    {
        return currentType;
    }

}
