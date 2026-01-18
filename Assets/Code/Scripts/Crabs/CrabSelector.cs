using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CrabSelector : MonoBehaviour
{
    public List<GameObject> prefabs;
    public List<Sprite> sprites;

    private List<int> idxsChosenRecently = new List<int>();

    IEnumerator Start()
    {
        prefabs.Clear();
        var prefabHandle = Addressables.LoadAssetsAsync<GameObject>("CharacterPrefabs", null);
        yield return prefabHandle;

        prefabs = new List<GameObject>(prefabHandle.Result);

        sprites.Clear();
        var spriteHandle = Addressables.LoadAssetsAsync<Sprite>("CharacterSprites", null);
        yield return spriteHandle;

        sprites = new List<Sprite>(spriteHandle.Result);
    }

    public (GameObject, int) ChooseCrab()
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
        while (idxsChosenRecently.Contains(chosenCrabIdx));

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
