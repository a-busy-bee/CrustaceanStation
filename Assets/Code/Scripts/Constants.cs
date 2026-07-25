using UnityEngine;
using System.Collections.Generic;

using Special = CrabInfo.SpecialCharacter;
public class Constants : MonoBehaviour
{
    public static Constants instance { get; private set; }
    public static string GAME_SCENE_NAME = "BaseArea";
    public static int CLOCK_END_TIME = 24;
    public static float CLOCK_SPEED = 7.5f;

    #region LETTERS & FEEDBACK
    // LETTERS MANAGER
    public Dictionary<int, int> LETTER_dayToIdxCrustyCo = new Dictionary<int, int>() // day -> idx in list
    {
        {0, 0},
        {1, 1},
        {6, 2},
        {7, 3},
        {11, 4},
        {16, 5},
    };
    public Dictionary<int, int> LETTER_dayToIdxFamily = new Dictionary<int, int>() // day -> idx in list
    {
        {3, 0},
        {14, 1},
        {17, 2}
    };
    public Dictionary<int, int> LETTER_dayToIdxMailkeeper = new Dictionary<int, int>() // day -> idx in list
    {
        {4, 0},
        {8, 1},
        {12, 2},
        {16, 3}
    };

    // FEEDBACK FORMS
    public Dictionary<Special, Dictionary<int, int>> FEEDBACK_characterToDayToIdx = new Dictionary<Special, Dictionary<int, int>>()
    {
        {Special.itty, new Dictionary<int, int>() {
            {0, 0},
            {0, 1},
            {0, 2},
            {0, 3},
            {0, 4},
            {0, 5},
            {0, 6}
        }},
        {Special.protestorCatfish, new Dictionary<int, int>() {
            {0, 0},
            {0, 1},
            {0, 2},
            {0, 3},
        }},
        {Special.horseshoe, new Dictionary<int, int>() {
            {0, 0},
            {0, 1},
            {0, 2},
            {0, 3},
            {0, 4},
            {0, 5},
            {0, 6},
            {0, 7},
            {0, 8},
            {0, 9},
            {0, 10},
            {0, 11}
        }},
        {Special.isobelle, new Dictionary<int, int>() {
            {0, 0},
            {0, 1},
            {0, 2}
        }},
        {Special.seaStarDad, new Dictionary<int, int>() {
            {0, 0},
            {0, 1},
            {0, 2},
            {0, 3},
            {0, 4}
        }},
        {Special.granny, new Dictionary<int, int>() {
            {0, 0},
            {0, 1},
            {0, 2},
            {0, 3}
        }},
        {Special.gramps, new Dictionary<int, int>() {
            {0, 0},
            {0, 1},
            {0, 2},
            {0, 3}
        }}
    };

    #endregion



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
    }
}
