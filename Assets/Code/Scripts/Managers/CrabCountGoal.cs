using TMPro;
using UnityEngine;

public class CrabCountGoal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private TextMeshProUGUI kioskText;
    [SerializeField] private TextMeshProUGUI kioskTextOther;
    [SerializeField] private GameObject accomplishedText;
    [SerializeField] private Kiosk kiosk;
    private int goalCount;
    private bool isActive = false;
    private void Awake()
    {
        goalText.text = "";
        kioskTextOther.gameObject.SetActive(false);
        kioskText.gameObject.SetActive(false);
        accomplishedText.SetActive(false);
    }

    public void SetGoalActive()
    {
        goalCount = Random.Range(20, 40);
        goalText.text = goalCount.ToString();
        kioskText.text = goalCount.ToString();

        kioskTextOther.gameObject.SetActive(true);
        kioskText.gameObject.SetActive(true);

        isActive = true;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public bool WasGoalAchieved()
    {
        return kiosk.GetTotalCrabs() >= goalCount;
    }

    public void IncrementGoal(int totalCrabs)
    {
        if (!isActive) return;

        if (totalCrabs < goalCount)
        {
            kioskText.text = (goalCount - totalCrabs).ToString();
            if (goalCount - totalCrabs == 1)
            {
                kioskTextOther.text = "See        crab";
            }
        }
        else
        {
            kioskTextOther.gameObject.SetActive(false);
            kioskText.gameObject.SetActive(false);

            accomplishedText.SetActive(true);
        }
    }

}
