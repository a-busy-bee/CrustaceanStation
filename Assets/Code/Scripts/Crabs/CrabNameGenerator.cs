using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrabNameGenerator : MonoBehaviour
{
    public static CrabNameGenerator instance { get; private set; }

    private Dictionary<CrabInfo.CrabType, List<string>> nameDictionary; // species-speicifc names (ie Crabstopher)
    private List<string> general = new List<string>();  // general names (ie Max)

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        nameDictionary = new Dictionary<CrabInfo.CrabType, List<string>>();

        LoadNameFile();
    }

    public string GetNameByType(CrabInfo.CrabType type)
    {
        if (nameDictionary.ContainsKey(type) && Random.Range(0, 3) <= 2)
        {
            int idx = Random.Range(0, nameDictionary[type].Count);
            return nameDictionary[type][idx];
        }

        // fallback
        return general[Random.Range(0, general.Count)];
    }

    public string GetAnyName()
    {
        return general[Random.Range(0, general.Count)];
    }


    private void LoadNameFile()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("names");

        if (csvFile == null)
        {
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) { continue; }

            string line = lines[i].Trim();

            // Split only on first comma (name can be quoted)
            int firstComma = line.IndexOf(',');
            if (firstComma < 0) continue;

            string name = line.Substring(0, firstComma).Trim().Trim('"');
            string speciesField = line.Substring(firstComma + 1).Trim().Trim('"');

            string[] speciesList = speciesField.Split(',');

            foreach (string rawSpecies in speciesList)
            {
                string species = rawSpecies.Trim();

                // Generic → fallback list
                if (species.Equals("Generic", System.StringComparison.OrdinalIgnoreCase))
                {
                    general.Add(name);
                    continue;
                }

                // Try parsing enum
                if (System.Enum.TryParse(species, true, out CrabInfo.CrabType crabType))
                {
                    if (!nameDictionary.ContainsKey(crabType))
                    {
                        nameDictionary[crabType] = new List<string>();
                    }

                    nameDictionary[crabType].Add(name);
                }
                else
                {
                    Debug.LogWarning($"Unknown CrabType '{species}' for name '{name}'");
                }
            }
        }

    }

}
