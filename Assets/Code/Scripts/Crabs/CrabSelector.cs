using System.Collections.Generic;
using UnityEngine;

public class CrabSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Sprite[] sprites;

    private List<int> idxsChosenRecently = new List<int>();
    public GameObject ChooseCrab()
    {
        int chosenCrabIdx = Random.Range(0, prefabs.Length);
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
                chosenCrabIdx = Random.Range(0, prefabs.Length);
            }

            idxsChosenRecently.Add(chosenCrabIdx);
            return prefabs[chosenCrabIdx];
        }
    }

    public Sprite ChooseSprite()
    {
        return sprites[Random.Range(0, sprites.Length)];
    }
}
