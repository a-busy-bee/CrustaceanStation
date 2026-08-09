using UnityEngine;

public class Calendar : MonoBehaviour
{
    //crosses
    [SerializeField] private GameObject[] crosses;

    private int currDay = 0;

    private void Start()
    {
        currDay = SaveManager.instance.GetProgression_CurrDay();

        for (int i = 0; i < currDay; i++)
        {
            // turn on cross
            crosses[i].SetActive(true);
        }

        for (int i = currDay; i < crosses.Length; i++)
        {
            // turn off cross
            crosses[i].SetActive(false);
        }

        if (currDay > 20)
        {
            HomeManager.instance.SetIsoRoomButtonActive(false);
            HomeManager.instance.SetGoToWorkButtonActive(false);
            return;
        }
        // enable iso room button on sundays
        bool isIsoDay = (currDay + 1) % 5 == 0;
        HomeManager.instance.SetIsoRoomButtonActive(isIsoDay);
        HomeManager.instance.SetGoToWorkButtonActive(!isIsoDay);

    }
}
