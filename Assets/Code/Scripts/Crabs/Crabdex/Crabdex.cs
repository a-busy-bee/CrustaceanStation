using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Crabdex : MonoBehaviour
{
    // TODO
    // increase how often crabs show up as the crabdex is filled out 
    // ie at first you only get 1-2 types of crabs, then more

    public static Crabdex instance { get; private set; }

    [SerializeField] private List<CrabdexEntry> crabdexEntries;     // classification of crab
    [SerializeField] private GameObject[] contentsPageEntries;  // buttons
    [SerializeField] private GameObject crabdexContents;
    [SerializeField] private GameObject crabdexPage;
    [SerializeField] private GameObject screenDim;

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

        // Set all hasBeenDiscovered to false
        if (PlayerPrefs.GetInt("ResetCrabdex") == 1)
        {
            PlayerPrefs.SetInt("ResetCrabdex", 0);

            foreach (CrabdexEntry entry in crabdexEntries)
            {
                entry.generalVariantDiscovered = false;

                for (int i = 0; i < entry.variants.Length; i++)
                {
                    entry.variants[i].hasBeenDiscovered = false;
                }
            }
        }
    }

    IEnumerator Start()
    {
        // set up ID for each button
        for (int i = 0; i < contentsPageEntries.Length; i++)
        {
            contentsPageEntries[i].GetComponent<ContentsButton>().SetID(i);
        }

        crabdexEntries.Clear();
        var entryHandle = Addressables.LoadAssetsAsync<CrabdexEntry>("CrabdexEntries", null);
        yield return entryHandle;

        crabdexEntries = new List<CrabdexEntry>(entryHandle.Result);

    }

    public void OnCrabEntryPress(int id)
    {
        crabdexPage.SetActive(true);
        crabdexPage.GetComponent<CrabdexPageBuilder>().Build(crabdexEntries[id]);
    }

    public void HasBeenDiscovered(CrabInfo crabInfo) // called when a crab approaches the kiosk
    {
        // loop through crabdex entries
        foreach (CrabdexEntry entry in crabdexEntries)
        {
            // if general name matches, check if variant
            if (entry.crabName == crabInfo.crabdexName)
            {
                // if variant, loop through variant types
                if (crabInfo.isVariant)
                {
                    for (int i = 0; i < entry.variants.Length; i++)
                    {
                        // check if this variant type has been discovered
                        if (entry.variants[i].variantName == crabInfo.variantName && !entry.variants[i].hasBeenDiscovered)
                        {
                            //crabInfo.hasBeenDiscovered = true;
                            entry.variants[i].hasBeenDiscovered = true;

                            break;
                        }
                    }
                }
                else // otherwise, only need to check if general type has been discovered
                {
                    if (!entry.generalVariantDiscovered)
                    {
                        //crabInfo.hasBeenDiscovered = true;
                        entry.generalVariantDiscovered = true;
                    }
                }

                break; // no need to loop through the rest since we've found our crab
            }
        }
    }

    public void ShowCodex()
    {
        screenDim.SetActive(true);
        crabdexPage.SetActive(false);

        // show codex starting from contents page
        crabdexContents.SetActive(true);

        for (int i = 0; i < contentsPageEntries.Length; i++)
        {
            if (crabdexEntries[i].generalVariantDiscovered)
            {
                contentsPageEntries[i].transform.Find("CrabPhoto").GetComponent<Image>().sprite = crabdexEntries[i].typicalSprite;
            }
        }
    }

    public void HideCodex()
    {
        // hide codex
        crabdexPage.SetActive(false);
        crabdexContents.SetActive(false);
        screenDim.SetActive(false);
    }

    public bool IsCrustacean(string id)
    {
        foreach (CrabdexEntry entry in crabdexEntries)
        {
            if (entry.crabName == id)
            {
                return entry.isCrustacean;
            }
        }
        return false; // failsafe
    }
}
