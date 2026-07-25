using UnityEngine;
using System.Collections.Generic;

using Special = CrabInfo.SpecialCharacter;
public class Constants : MonoBehaviour
{
    public static Constants instance { get; private set; }
    public static string GAME_SCENE_NAME = "BaseArea";
    public static int CLOCK_END_TIME = 24;
    public static float CLOCK_SPEED = 1f;//7.5f;

    #region SPECIALS
    public Dictionary<Special, string> specialEnumToStringName = new Dictionary<Special, string>()
    {
        { Special.itty,             "Itty Bitty" },
        { Special.protestorCatfish, "Charlie" },
        { Special.horseshoe,        "The Old One" },
        { Special.isobelle,         "Isobelle" },
        { Special.seaStarDad,       "Hank" },
        { Special.granny,           "Granny" },
        { Special.gramps,           "Gramps" }
    };

    public Dictionary<int, Special[]> SELECTOR_dayToCharacter = new Dictionary<int, Special[]> {
        { 1, new Special[]  {Special.itty, Special.seaStarDad, Special.granny, Special.gramps}},
        { 2, new Special[]  {Special.itty, Special.isobelle, Special.granny, Special.gramps, Special.horseshoe}},
        { 3, new Special[]  {Special.itty, Special.seaStarDad, Special.isobelle, Special.protestorCatfish}},
        { 4, new Special[]  {Special.granny, Special.gramps, Special.horseshoe, Special.protestorCatfish}},

        { 6, new Special[]  {Special.itty, Special.seaStarDad, Special.isobelle, Special.granny, Special.horseshoe}},
        { 7, new Special[]  {Special.itty, Special.isobelle, Special.granny, Special.gramps}},
        { 8, new Special[]  {Special.itty, Special.granny, Special.gramps, Special.protestorCatfish}},
        { 9, new Special[]  {Special.seaStarDad, Special.isobelle, Special.horseshoe, Special.protestorCatfish}},

        { 11, new Special[] {Special.itty, Special.seaStarDad, Special.granny, Special.gramps}},
        { 12, new Special[] {Special.itty, Special.seaStarDad, Special.isobelle, Special.horseshoe, Special.protestorCatfish}},
        { 13, new Special[] {Special.itty, Special.granny, Special.horseshoe, Special.protestorCatfish}},
        { 14, new Special[] {Special.seaStarDad, Special.isobelle, Special.horseshoe, Special.protestorCatfish}},

        { 16, new Special[] {Special.itty, Special.seaStarDad, Special.horseshoe, Special.protestorCatfish}},
        { 17, new Special[] {Special.isobelle, Special.granny, Special.gramps, Special.horseshoe, Special.protestorCatfish}},
        { 18, new Special[] {Special.itty, Special.granny, Special.gramps, Special.horseshoe}},
        { 19, new Special[] {Special.seaStarDad, Special.gramps, Special.horseshoe, Special.protestorCatfish}}
    };
    #endregion

    #region LETTERS & FEEDBACK
    // LETTERS MANAGER
    public Dictionary<int, int> LETTER_dayToIdxCrustyCo = new Dictionary<int, int>() // day -> idx in list
    {
        {1, 0},
        {6, 1},
        {7, 2},
        {11, 3},
        {16, 4},
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
            {1, 1},
            {2, 2},
            {3, 3},
            {4, 4},
            {5, 5},
            {6, 6}
        }},
        {Special.protestorCatfish, new Dictionary<int, int>() {
            {0, 0},
            {1, 1},
            {2, 2},
            {3, 3}
        }},
        {Special.horseshoe, new Dictionary<int, int>() {
            {0, 0},
            {1, 1},
            {2, 2},
            {3, 3},
            {4, 4},
            {5, 5},
            {6, 6},
            {7, 7},
            {8, 8},
            {9, 9},
            {10, 10},
            {11, 11}
        }},
        {Special.isobelle, new Dictionary<int, int>() {
            {0, 0},
            {1, 1},
            {2, 2}
        }},
        {Special.seaStarDad, new Dictionary<int, int>() {
            {0, 0},
            {1, 1},
            {2, 2},
            {3, 3},
            {4, 4}
        }},
        {Special.granny, new Dictionary<int, int>() {
            {0, 0},
            {1, 1},
            {2, 2},
            {3, 3}
        }},
        {Special.gramps, new Dictionary<int, int>() {
            {0, 0},
            {1, 1},
            {2, 2},
            {3, 3}
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
