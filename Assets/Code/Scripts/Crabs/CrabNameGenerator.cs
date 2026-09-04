using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CrabNameGenerator : MonoBehaviour
{
    public static CrabNameGenerator instance { get; private set; }
    private Dictionary<CrabInfo.CrabType, List<int>> nameDictionary; // species-specific names (ie Crabstopher)
    private List<int> general = new List<int>(); // general names (ie Max)
    private int nameIdxCounter = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        nameDictionary = new Dictionary<CrabInfo.CrabType, List<int>>();

        LoadNameFile();
    }

    public string GetNameByType(CrabInfo.CrabType type)
    {
        // 2/3 chance to use species name if available
        if (nameDictionary.ContainsKey(type) && Random.Range(0, 3) <= 1)
        {
            var list = nameDictionary[type];
            int idx = list[Random.Range(0, list.Count)];
            return LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Names, "names_" + idx);
        }

        // fallback
        return GetAnyName();
    }

    public string GetAnyName()
    {
        int idx = general[Random.Range(0, general.Count)];
        return LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Names, "names_" + idx);
    }

    private void LoadNameFile()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("names");

        if (csvFile == null)
        {
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        // skip header
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string line = lines[i].Trim();

            // Parse CSV row
            List<string> fields = ParseCsvLine(line);

            if (fields.Count < 2) continue;

            List<string> speciesList = ParseCsvLine(fields[1]);

            foreach (string rawSpecies in speciesList)
            {
                string species = rawSpecies.Trim();

                // fallback list
                if (species.Equals("Generic", System.StringComparison.OrdinalIgnoreCase))
                {
                    general.Add(nameIdxCounter);
                    continue;
                }

                if (System.Enum.TryParse(species, true,
                    out CrabInfo.CrabType crabType))
                {
                    if (!nameDictionary.ContainsKey(crabType))
                    {
                        nameDictionary[crabType] = new List<int>();
                    }

                    nameDictionary[crabType].Add(nameIdxCounter);
                }
                else
                {
                    Debug.LogWarning(
                        $"Unknown CrabType '{species}' for name '{name}'");
                }
            }

            nameIdxCounter++;
        }
    }
    private List<string> ParseCsvLine(string line)
    {
        List<string> fields = new List<string>();
        StringBuilder current = new StringBuilder();

        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                // Handle escaped quotes ("")
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(current.ToString().Trim());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        fields.Add(current.ToString().Trim());

        return fields;
    }
}