using UnityEngine;
using System.Collections.Generic;

using Special = CrabInfo.SpecialCharacter;
using Mutant = CrabInfo.MutantType;
public class Constants : MonoBehaviour
{
    public static Constants instance { get; private set; }
    public static string GAME_SCENE_NAME = "BaseArea";
    public static int CLOCK_END_TIME = 24;
    public static float CLOCK_SPEED = 0.0001f;//10f;//100f;//0.01f;//2f;//0.1f;//10f;//7.5f;

    #region CHARACTERS
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

    public Dictionary<int, Mutant[]> SELECTOR_dayToMutant = new Dictionary<int, Mutant[]>
    {
        { 7, new Mutant[] {Mutant.fishChoke}},
        { 8, new Mutant[] {Mutant.axolotlLong}},
        { 9, new Mutant[] {Mutant.nautilusPurple}},
        {11, new Mutant[] {Mutant.horseshoeLegs, Mutant.axolotlGoldLong}},
        {12, new Mutant[] {Mutant.sealemonBlackYellow, Mutant.sealemonYellowOrange}},
        {13, new Mutant[] {Mutant.sealClaw, Mutant.sealemonBlackOrange}},
        {14, new Mutant[] {Mutant.seasheepHorns, Mutant.nautilusYellow}},
        {16, new Mutant[] {Mutant.sealemonYellowOrange, Mutant.sealemonBlackOrange, Mutant.fishChoke}},
        {17, new Mutant[] {Mutant.fishGills, Mutant.axolotlLong, Mutant.nautilusPurple}},
        {18, new Mutant[] {Mutant.fishChoke, Mutant.seasheepHorns, Mutant.horseshoeLegs}},
        {19, new Mutant[] {Mutant.fishGills, Mutant.seagullLegs, Mutant.sealClaw}}
    };

    #endregion

    #region LETTERS & FEEDBACK
    // LETTERS MANAGER
    public Dictionary<int, int> LETTER_dayToIdxCrustyCo = new Dictionary<int, int>() // day -> idx in list
    {
        {0, 0},
        {5, 1},
        {6, 2},
        {11, 3}
    };
    public Dictionary<int, int> LETTER_dayToIdxCrustyCoEndings = new Dictionary<int, int>() // day -> idx in list
    {
        {15, 0},
        {20, 1}
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

    public Dictionary<int, int> DIALOGUE_dayToIdxMailkeeper = new Dictionary<int, int>()
    {
        {2, 0},
        {6, 1},
        {7, 2},
        {8, 3},
        {11, 4},
        {12, 5},
        {14, 6},
        {16, 7},
        {17, 0},
        {19, 1},
        {20, 9}
    };


    // FEEDBACK FORMS
    public Dictionary<Special, Dictionary<int, int>> FEEDBACK_characterToDayToIdx = new Dictionary<Special, Dictionary<int, int>>()
    {
        {Special.itty, new Dictionary<int, int>() {
            {1, 0},
            {2, 1},
            {3, 2},
            {6, 3},
            {8, 4},
            {11, 5},
            {16, 6}
        }},
        {Special.protestorCatfish, new Dictionary<int, int>() {
            {4, 0},
            {8, 1},
            {9, 2},
            {14, 3}
        }},
        {Special.horseshoe, new Dictionary<int, int>() {
            {2, 0},
            {4, 1},
            {6, 2},
            {9, 3},
            {12, 4},
            {13, 5},
            {14, 6},
            {16, 7},
            {17, 8},
            {18, 9},
            {19, 10}
        }},
        {Special.isobelle, new Dictionary<int, int>() {
            {6, 0},
            {7, 1},
            {13, 2}
        }},
        {Special.seaStarDad, new Dictionary<int, int>() {
            {1, 0},
            {3, 1},
            {9, 2},
            {12, 3},
            {14, 4}
        }},
        {Special.granny, new Dictionary<int, int>() {
            {2, 0},
            {4, 1},
            {8, 2},
            {18, 3}
        }},
        {Special.gramps, new Dictionary<int, int>() {
            {7, 0},
            {11, 1},
            {17, 2},
            {19, 3}
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
