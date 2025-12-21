using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public static Clock instance { get; private set; }

    // CLOCK INFO
    private int startTime = 0;
    private int endTime = 24;
    private int currentTime = 0;
    private float rotateAmount = 15.0f;
    [SerializeField] private float rotDuration = 1.0f;
    [SerializeField] private Image fill;

    [SerializeField] private GameObject clockHand;

    // CRABS
    [SerializeField] private Kiosk kiosk;

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

        fill.fillAmount = 0;
    }

    public void BeginDay()
    {
        currentTime = startTime;
        StartCoroutine(TimeItself());
    }

    // CLOCK ANIMATION
    private IEnumerator TimeItself()
    {
        while (currentTime < endTime)
        {

            if (currentTime == startTime)
            {
                LevelManager.instance.CheckTrains(startTime);
                yield return WaitThenSummonCrabs();
            }

            yield return new WaitForSeconds(Constants.CLOCK_SPEED);

            // rotate clock hand
            yield return RotateHand();
            currentTime++;

            LevelManager.instance.CheckTrains(currentTime);

            if (currentTime % 2 == 0) // chance to change weather every 2 hours
            {
                WeatherManager.instance.ChangeWeather();
            }

            if (currentTime == endTime)
            {
                LevelManager.instance.SetState(LevelManager.LMState.Summary);
            }
        }
    }

    private IEnumerator RotateHand()
    {
        Quaternion startRot = clockHand.transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 0, -rotateAmount);

        float startFill = fill.fillAmount;
        float endFill = startFill + (rotateAmount / 360.0f);

        float elapsed = 0f;
        while (elapsed < rotDuration)
        {
            clockHand.transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / rotDuration);
            fill.fillAmount = Mathf.Lerp(startFill, endFill, elapsed / rotDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        clockHand.transform.rotation = endRot;
        fill.fillAmount = endFill;

    }

    private IEnumerator WaitThenSummonCrabs()
    {
        yield return new WaitForSeconds(0.5f);
        kiosk.SetState(Kiosk.KioskState.Empty);
    }

}
