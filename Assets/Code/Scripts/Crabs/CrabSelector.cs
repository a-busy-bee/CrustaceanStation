using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;

using Special = CrabInfo.SpecialCharacter;
public class CrabSelector : MonoBehaviour
{
    public static CrabSelector instance { get; protected set; }

    public List<GameObject> prefabs;
    public List<GameObject> prefabsSpecial;
    public List<Sprite> sprites;

    // for tutorial
    private List<int> idxsChosenRecently = new List<int>();

    // for main game (not zen mode)
    private Queue<int> idxQueue = new Queue<int>();   // list of characters to be seen
    private Queue<int> specialQueue = new Queue<int>();   // list of specials to be seen
                                                        // if not empty, next character is the first index of specialQueue
                                                        // if empty, ignore
                                                        // when adding special to queue (based on clock), push to back of specialQueue
                                                        // ex. [itty, horseshoe, granny] [everyone else], so next character would be itty, 
                                                        // and if another special gets added then they would appear after granny
    private Queue<int> specialsForToday = new Queue<int>(); // list of specials based on dayToCharacter, but remove specials as they're added to queue
    private Dictionary<int, bool> seenCharacters = new Dictionary<int, bool>(); // <idx of character, whether it was seen or not> (using dictionary for better performance)
    private int currIdx;
    private int maxQueueLength = 45;

    private Dictionary<int, Special[]> dayToCharacter = new Dictionary<int, Special[]> {
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

    IEnumerator Start()
    {
        // LOAD ALL ASSETS
        prefabs.Clear();
        var prefabHandle = Addressables.LoadAssetsAsync<GameObject>("CharacterPrefabs", null);
        yield return prefabHandle;
        prefabs = new List<GameObject>(prefabHandle.Result);

        prefabsSpecial.Clear();
        var prefabSpecialHandle = Addressables.LoadAssetsAsync<GameObject>("CharacterPrefabsSpecial", null);
        yield return prefabSpecialHandle;
        prefabsSpecial = new List<GameObject>(prefabSpecialHandle.Result);

        sprites.Clear();
        var spriteHandle = Addressables.LoadAssetsAsync<Sprite>("CharacterSprites", null);
        yield return spriteHandle;
        sprites = new List<Sprite>(spriteHandle.Result);

        // ADD TODAY'S SPECIALS
        int currDay = SaveManager.instance.GetProgression_CurrDay();
        foreach (Special special in dayToCharacter[currDay + 1])
        {
            // find idx in special prefabs
            int specialObj = prefabsSpecial.FindIndex(s => s.GetComponent<CrabController>().GetSpecialType() == special);
            specialsForToday.Enqueue(specialObj);
        }

        // GENERATE GENERIC CHARACTERS
        int chosenCrabIdx;
        for (int i = 0; i < maxQueueLength; i++)
        {
            do
            {
                chosenCrabIdx = Random.Range(0, prefabs.Count);
            }
            while (seenCharacters.ContainsKey(chosenCrabIdx) || prefabs[chosenCrabIdx].GetComponent<CrabController>().IsSpecial()); // no important characters just yet

            seenCharacters[chosenCrabIdx] = true;
            idxQueue.Enqueue(chosenCrabIdx);
        }
        
    }

    public (GameObject, int) ChooseCrab()
    {
        // if special queue is not empty, summon special character next
        if (specialQueue.Count != 0)
        {
            int specialIdx = specialQueue.Dequeue();
            GameObject specialObj = prefabsSpecial[specialIdx];
            return (specialObj, specialIdx);
        }
        
        // otherwise choose generic character
        int idx = idxQueue.Dequeue();
        GameObject obj = prefabs[idx];
        return (obj, idx);
    }

    public void PushNextSpecial()
    {
        if (specialsForToday.Count != 0)
        {
            int specialIdx = specialsForToday.Dequeue();
            specialQueue.Enqueue(specialIdx);
        }
    }

    public (GameObject, int) ChooseCrabTutorial()
    {
        if (idxsChosenRecently.Count >= prefabs.Count)
        {
            idxsChosenRecently.Clear();
        }

        int chosenCrabIdx;

        do
        {
            chosenCrabIdx = Random.Range(0, prefabs.Count);
        }
        while (idxsChosenRecently.Contains(chosenCrabIdx)
                || prefabs[chosenCrabIdx].GetComponent<CrabController>().GetCrabInfo().isLarge
                || prefabs[chosenCrabIdx].GetComponent<CrabController>().GetCrabInfo().isMultiple);

        idxsChosenRecently.Add(chosenCrabIdx);

        if (idxsChosenRecently.Count > 15)
        {
            idxsChosenRecently.RemoveAt(0);
        }

        return (prefabs[chosenCrabIdx], chosenCrabIdx);
    }

    public void AddToQueue(int idx)
    {
        idxsChosenRecently.Add(idx);
    }

    public int GetNumSpecialCharacters()
    {
        int currDay = SaveManager.instance.GetProgression_CurrDay();
        return dayToCharacter[currDay + 1].Length;
    }

    public Sprite ChooseSprite()
    {
        return sprites[Random.Range(0, sprites.Count)];
    }
}
