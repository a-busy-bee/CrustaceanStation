using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;

using Special = CrabInfo.SpecialCharacter;
using Mutant = CrabInfo.MutantType;
public class CrabSelector : MonoBehaviour
{
    public static CrabSelector instance { get; protected set; }

    public List<GameObject> prefabs;
    public List<GameObject> prefabsSpecial;
    public List<GameObject> prefabsMutated;
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
    private Queue<int> mutantQueue = new Queue<int>();
    private Queue<int> specialsForToday = new Queue<int>(); // list of specials based on dayToCharacter, but remove specials as they're added to queue
    private Queue<int> mutantsForToday = new Queue<int>();
    private Dictionary<int, bool> seenCharacters = new Dictionary<int, bool>(); // <idx of character, whether it was seen or not> (using dictionary for better performance)
    private int currIdx;
    private int maxQueueLength = 45;

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

        prefabsMutated.Clear();
        var prefabMutatedHandle = Addressables.LoadAssetsAsync<GameObject>("CharacterPrefabsMutated", null);
        yield return prefabMutatedHandle;
        prefabsMutated = new List<GameObject>(prefabMutatedHandle.Result);

        sprites.Clear();
        var spriteHandle = Addressables.LoadAssetsAsync<Sprite>("CharacterSprites", null);
        yield return spriteHandle;
        sprites = new List<Sprite>(spriteHandle.Result);

        // ADD TODAY'S SPECIALS
        int currDay = SaveManager.instance.GetProgression_CurrDay();
        foreach (Special special in Constants.instance.SELECTOR_dayToCharacter[currDay + 1])
        {
            // find idx in special prefabs
            int specialObj = prefabsSpecial.FindIndex(s => s.GetComponent<CrabController>().GetSpecialType() == special);
            specialsForToday.Enqueue(specialObj);
        }

        // ADD TODAY'S MUTANTS
        if (Constants.instance.SELECTOR_dayToMutant.ContainsKey(currDay + 1))
        {
            foreach (Mutant mutant in Constants.instance.SELECTOR_dayToMutant[currDay + 1])
            {
                int mutantObj = prefabsMutated.FindIndex(m => m.GetComponent<CrabController>().GetMutantType() == mutant);
                mutantsForToday.Enqueue(mutantObj);
            }
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

        // if no special characters left, now prioritize mutants
        if (mutantQueue.Count != 0)
        {
            int mutantIdx = mutantQueue.Dequeue();
            GameObject mutantObj = prefabsMutated[mutantIdx];
            return (mutantObj, mutantIdx);
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

    public void PushNextMutant()
    {
        if (mutantsForToday.Count != 0)
        {
            int mutantIdx = mutantsForToday.Dequeue();
            mutantQueue.Enqueue(mutantIdx);
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

    public Sprite ChooseSprite()
    {
        return sprites[Random.Range(0, sprites.Count)];
    }
}
