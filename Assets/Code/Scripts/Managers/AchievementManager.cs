using UnityEngine;
using Steamworks;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance { get; private set; }

    public enum AchievementTypeBool
    {
        gettingHeated,
        dinnerForOne,
        risingTensions,
        amYou,
        evolution,
        soLong,
        whatHaveIDone
    }

    public enum AchievementTypeProgressive
    {
        networking,
        ticketmaster,
        likeTrains,
        heLikesIt

    }

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

    public void UnlockAchievementBool(AchievementTypeBool type)
    {
        if (!SteamManager.Initialized) return;

        bool isUnlocked;
        if (SteamUserStats.GetAchievement(type.ToString(), out isUnlocked))
        {
            if (isUnlocked) return;

            SteamUserStats.SetAchievement(type.ToString());
        }
    }

    public void UnlockAchievementProgressive(AchievementTypeProgressive type, int increase)
    {
        if (!SteamManager.Initialized) return;

        int currCount;
        if (SteamUserStats.GetStat(type.ToString(), out currCount))
        {
            int newCount = currCount + increase;

            SteamUserStats.SetStat(type.ToString(), newCount);
            SteamUserStats.StoreStats();
        }
    }
    
}
