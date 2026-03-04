using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class Hats : MonoBehaviour
{
    public List<Sprite> hatOptions;
    [SerializeField] private Image hat;

    IEnumerator Start()
    {
        var spriteHandle = Addressables.LoadAssetsAsync<Sprite>("Hats", sprite =>
        {
            hatOptions.Add(sprite);
        });


        yield return spriteHandle;

        if (Random.Range(0, 5) == 3) // only 1 in 5 crabs have hats
        {
            hat.sprite = hatOptions[Random.Range(0, hatOptions.Count)];

            Color color = hat.color;
            color.a = 1;
            hat.color = color;
        }
        else
        {
            hat.sprite = null;

            Color color = hat.color;
            color.a = 0;
            hat.color = color;
        }
    }
}
