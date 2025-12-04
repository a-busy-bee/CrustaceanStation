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
        var prefabHandle = Addressables.LoadAssetsAsync<GameObject>("CharacterPrefabs", prefab =>
        {
            prefabs.Add(prefab);
        });

        yield return prefabHandle;

        var spriteHandle = Addressables.LoadAssetsAsync<Sprite>("CharacterSprites", sprite =>
        {
            sprites.Add(sprite);
        });

        yield return spriteHandle;
    }

    public GameObject ChooseCrab()
    {
        int chosenCrabIdx = Random.Range(0, prefabs.Count);
        //idxsChosenRecently.Add(chosenCrabIdx);

        if (idxsChosenRecently.Count == 15)
        {
            idxsChosenRecently.RemoveAt(0);
        }

        if (!idxsChosenRecently.Exists(i => i == chosenCrabIdx))
        {
            idxsChosenRecently.Add(chosenCrabIdx);
            return prefabs[chosenCrabIdx];
        }
        else
        {
            while (idxsChosenRecently.Exists(i => i == chosenCrabIdx))
            {
                chosenCrabIdx = Random.Range(0, prefabs.Count);
            }

            idxsChosenRecently.Add(chosenCrabIdx);
            return prefabs[chosenCrabIdx];
        }
    }

    public Sprite ChooseSprite()
    {
        return sprites[Random.Range(0, sprites.Count)];
    }
}
