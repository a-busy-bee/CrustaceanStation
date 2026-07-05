using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

public class CrabSelector : MonoBehaviour
{
    public static CrabSelector instance { get; protected set; }

    public List<GameObject> prefabs;
    public List<GameObject> prefabsSpecial;
    public List<Sprite> sprites;

    // for tutorial
    private List<int> idxsChosenRecently = new List<int>();

    // for main game (not zen mode)
    private List<int> idxQueue = new List<int>();   // list of characters to be seen
    private Dictionary<int, bool> seenCharacters = new Dictionary<int, bool>(); // <idx of character, whether it was seen or not> (using dictionary for better performance)
    private List<int> seenSpecialCharacters = new List<int>(); // same as dictionary above

    private List<(int, int)> specialCharacterIdxs = new List<(int, int)>(); // (index in prefabsSpecial, index in idxQueue)
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


        // generate list of characters
        int chosenCrabIdx;
        for (int i = 0; i < maxQueueLength; i++)
        {
            do
            {
                chosenCrabIdx = Random.Range(0, prefabs.Count);
            }
            while (seenCharacters.ContainsKey(chosenCrabIdx) || prefabs[chosenCrabIdx].GetComponent<CrabController>().IsSpecial()); // no important characters just yet

            seenCharacters[chosenCrabIdx] = true;
            idxQueue.Add(chosenCrabIdx);
        }

        // add special characters
        int numSpecialCharacters = Random.Range(2, 4);

        for (int i = 0; i < numSpecialCharacters; i++)
        {
            do
            {
                chosenCrabIdx = Random.Range(0, prefabsSpecial.Count);
            }
            while (seenSpecialCharacters.Contains(chosenCrabIdx));

            seenSpecialCharacters.Add(chosenCrabIdx);


            // indices given queue of 50:
            // 2 specials: 16, 33
            // 3 specials: 12, 25, 37
            int idxInQueue = (i + 1) * idxQueue.Count() / (numSpecialCharacters + 1); 
            idxQueue[idxInQueue] = chosenCrabIdx; // add to queue
            specialCharacterIdxs.Add((chosenCrabIdx, idxInQueue)); // add to list of locations of all special characters in queue
        }
    }

    public (GameObject, int) ChooseCrab()
    {
        for (int i = 0; i < specialCharacterIdxs.Count(); i++)
        {
            if (currIdx == specialCharacterIdxs[i].Item2)
            {
                (GameObject, int) special = (prefabsSpecial[specialCharacterIdxs[i].Item1], currIdx);
                currIdx++;

                return special;
            }
        }

        (GameObject, int) pair = (prefabs[idxQueue[currIdx]], currIdx);
        currIdx++;

        return pair;
    }

    public void PushNextSpecial()
    {
        for (int i = 0; i < specialCharacterIdxs.Count(); i++)
        {
            if (currIdx < specialCharacterIdxs[i].Item2) // ignore all idxs we've already seen
            {

                // special character has already been pushed
                if (specialCharacterIdxs[i].Item2 == currIdx) return;

                /* move next special to immediate front of the queue (swap with what's currently there) */
                int genericIdx = idxQueue[currIdx];
                int currSpecialIdx = specialCharacterIdxs[i].Item2;

                // set special to immediate next
                idxQueue[currIdx] = specialCharacterIdxs[i].Item1;

                // update list of where special characters are located
                specialCharacterIdxs[i] = (specialCharacterIdxs[i].Item1, currIdx);

                // shift over everything else by one
                int nextSpecialIdx = currSpecialIdx;
                for (int j = i + 1; j < specialCharacterIdxs.Count(); j++)
                {
                    currSpecialIdx = nextSpecialIdx;
                    idxQueue[currSpecialIdx] = specialCharacterIdxs[j].Item1;
                    nextSpecialIdx = specialCharacterIdxs[j].Item2;

                    specialCharacterIdxs[j] = (specialCharacterIdxs[j].Item1, currSpecialIdx);

                }

                // swap with what's current in immediate next with former index of last special
                idxQueue[nextSpecialIdx] = genericIdx;

                //return;
                break;
            }
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
                || prefabs[chosenCrabIdx].GetComponent<CrabController>().GetCrabInfo().isLarge);

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
        return specialCharacterIdxs.Count();
    }

    public Sprite ChooseSprite()
    {
        return sprites[Random.Range(0, sprites.Count)];
    }
}
