using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    public enum Stage
    {
        day1,           // crusty corp letter on desk, tutorial
        headlines,      // introduce headlines & performance monitor
        mailroom,       // introduce feedback box & mailroom, add causal dialogue
        onlySPrey,      // crusty corp letter -> send all S prey
        allNonCrust,    // crusty corp letter -> send all non-crustaceans, see first mutation same day (first customer?)
        involuntary,    // after mutation, letter from crusty co -> involuntary shuttle
        shuttleActive,  // letter from biodiv co -> freedom van
        alarmActive,    // next day, letter from crusty co -> freedom van alarm, start tracking crusty vs bio floating pt variables
        bigEventCrust,  // send crustaceans that don't look crab-enough
        bigEventBio     // quit job to work for biodiv corporation
    }

    private Stage currStage;
    private int crabsSinceLastLevelUp = 0;
    private Dictionary<Stage, int> crabsNeededToAdvance;
    private float crust = 1.0f;
    private float bio = 1.0f;

    
}
