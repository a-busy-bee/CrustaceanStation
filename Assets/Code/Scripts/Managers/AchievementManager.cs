using UnityEngine;
using Steamworks;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance { get; private set; }

    public enum AchievementTypeBool
    {
        networking,
        gettingHeated,
        dinnerForOne,
        soLong,
        whatHaveIDone
    }

    public enum AchievementTypeProgressive
    {
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
        if (SteamUserStats.GetStat(type.ToString() + "_stat", out currCount))
        {
            int newCount = currCount + increase;

            SteamUserStats.SetStat(type.ToString(), newCount);
            SteamUserStats.StoreStats();
        }
    }

    public bool IsBoolAchievementUnlocked(AchievementTypeBool type)
    {
        if (!SteamManager.Initialized) return false;

        bool isUnlocked;
        if (SteamUserStats.GetAchievement(type.ToString(), out isUnlocked))
        {
            if (isUnlocked) return true;
        }

        return false;
    }

    public bool IsProgressiveAchievementUnlocked(AchievementTypeProgressive type)
    {
        if (!SteamManager.Initialized) return false;

        bool isUnlocked;
        if (SteamUserStats.GetAchievement(type.ToString() + "_stat", out isUnlocked))
        {
            if (isUnlocked) return true;
        }

        return false;
    }
    
}
