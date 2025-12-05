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
        int chosenCrabIdx = Random.Range(0, prefabs.Count);

        if (idxsChosenRecently.Count == 15)
        {
            idxsChosenRecently.RemoveAt(0);
        }

        if (!idxsChosenRecently.Exists(i => i == chosenCrabIdx))
        {
            return (prefabs[chosenCrabIdx], chosenCrabIdx);
        }
        else
        {
            while (idxsChosenRecently.Exists(i => i == chosenCrabIdx))
            {
                chosenCrabIdx = Random.Range(0, prefabs.Count);
            }

            return (prefabs[chosenCrabIdx], chosenCrabIdx);
        }
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
