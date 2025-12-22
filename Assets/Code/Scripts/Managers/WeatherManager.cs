using UnityEngine;
using UnityEngine.UI;
public class WeatherManager : MonoBehaviour
{
    public static WeatherManager instance { get; private set; }

    [SerializeField] private WeatherType[] types; // sunny > lightRain > darkRain > fog
    private WeatherType currentType;

    [Header("Foreground")]
    [SerializeField] private Image cloudsTop;
    [SerializeField] private Image backgroundTop;

    [Header("Background")]
    [SerializeField] private Image cloudsBottom;
    [SerializeField] private Image backgroundBottom;

    [Header("Rain Particle Systems")]
    [SerializeField] private ParticleSystem darkRainParticles;
    [SerializeField] private ParticleSystem lightRainParticles;

    [Header("Fog")]
    [SerializeField] private GameObject fogOverlay;
    [SerializeField] private GameObject groundClouds;

    // transitions
    private float transitionTime = 0f;
    private float duration = 2f;
    private bool isTransitioning;
    private bool wasFoggy;
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
        int startingWeather = Random.Range(0, 4);
        startingWeather = 2;
        WeatherType startingType = types[startingWeather];
        currentType = startingType;

        cloudsTop.color = startingType.cloudsTop;
        backgroundTop.color = startingType.backgroundTop;
        cloudsBottom.color = startingType.cloudsBottom;
        backgroundBottom.color = startingType.backgroundBottom;

        if (startingType.isFoggy)
        {
            // turn on fog
            fogOverlay.SetActive(true);
            groundClouds.SetActive(true);
            wasFoggy = true;
        }
        else if (startingType.isRainy)
        {
            if (startingType.rainType == WeatherType.RainType.light)
            {
                lightRainParticles.Play();
            }
            else if (startingType.rainType == WeatherType.RainType.dark)
            {
                darkRainParticles.Play();
            }
        }
    }

    public void ChangeWeather()
    {
        startCloudTop = cloudsTop.color;
        startBkgTop = backgroundTop.color;
        startCloudBottom = cloudsBottom.color;
        startBkgBottom = backgroundBottom.color;

        int changeIdx = Random.Range(0, 4);

        if (changeIdx == 0 && !(currentType.type == "sunny"))
        {
            MakeSunny();
        }
        else if (changeIdx == 1 && !(currentType.type == "lightRain" || currentType.type == "darkRain"))
        {
            MakeRainy();
        }

        MakeRainy();
    }

    private void MakeSunny()
    {
        isTransitioning = true;

        int newWeather = 0; // change weather to light rain if it's currently dark rain

        if (currentType.rainType == WeatherType.RainType.light)
        {
            lightRainParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        else
        {
            darkRainParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            lightRainParticles.Play();
            newWeather = 1;
        }

        currentType = types[newWeather];

        goalCloudTop = types[newWeather].cloudsTop;
        goalBkgTop = types[newWeather].backgroundTop;
        goalCloudBottom = types[newWeather].cloudsBottom;
        goalBkgBottom = types[newWeather].backgroundBottom;
    }

    private void MakeRainy()
    {
        isTransitioning = true;

        int typeOfRain = Random.Range(0, 2);

        if (typeOfRain == 0)
        {
            lightRainParticles.Play();
            typeOfRain = 1;
        }
        else
        {
            darkRainParticles.Play();
            typeOfRain = 2;
        }
        currentType = types[typeOfRain];

        goalCloudTop = types[typeOfRain].cloudsTop;
        goalBkgTop = types[typeOfRain].backgroundTop;
        goalCloudBottom = types[typeOfRain].cloudsBottom;
        goalBkgBottom = types[typeOfRain].backgroundBottom;
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

            if (t >= 1f)
            {
                isTransitioning = false;
                transitionTime = 0;
                fogOverlay.SetActive(false);
                groundClouds.SetActive(false);

                if (wasFoggy)
                {
                    wasFoggy = false;
                }
            }
        }
    }

    public WeatherType GetCurrentWeather()
    {
        return currentType;
    }

}
