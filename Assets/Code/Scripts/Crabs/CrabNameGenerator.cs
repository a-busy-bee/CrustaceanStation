using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrabNameGenerator : MonoBehaviour
{
    public static CrabNameGenerator instance { get; private set; }

    private List<string> general = new List<string>
    {
        "Shelly",
        "Molt",
        "Fiddley",
        "Iso",
        "Isobelle",
        "Podrick",
        "Brac",
        "Rab",
        "Cara",
        "Sean",
        "Shawn",
        "Magister Cinus",
        "Braxius",
        "Portia",
        "Depo",
        "Honshoo",
        "Cragglin",
        "Clay",
        "Dringle",
        "Lilik",
        "Finn",
        "Axl",
        "Amphie",
        "Newton",
        "Pinky",
        "Smolo",
        "Avogadro",
        "Socrates",
        "Plato",
        "Stoy",
        "Kale",
        "Callum",
        "Bait",
        "Spabloo",
        "Gobo",
        "Loto",
        "Rin",
        "Dewy",
        "Bran",
        "Echo",
        "Mona",
        "Clyde"
    };
    private List<string> catfish = new List<string>
    {
        "Ray",
        "Leo",
        "Oliver",
        "Luna",
        "Charlie",
        "Smudge",
        "Calypso",
        "Chester",
        "Clay",
        "Finn",
        "Axl",
        "Newton",
        "Kale",
        "Callum",
        "Echo"
    };
    private List<string> horseshoe = new List<string>
    {
        "Iso",
        "Isobelle",
        "Sheldon",
        "Limul",
        "Ancient Being",
        "Clay",
        "Newton",
        "Avogadro",
        "Socrates",
        "Plato",
        "Echo",
        "The Time Keeper"
    };
    private List<string> scopeCreep = new List<string>
    {
        "sc0p3cr33p",
        "0101001101000011",
        "sc0p3cr33p0101001101000011",
        "s010100110p301010011r33p"
    };
    private List<string> coquette = new List<string>
    {
        "Portia",
        "Cara",
        "Isobelle",
        "Fiddley",
        "Eloise",
        "Calliope",
        "Margot",
        "Clawdette",
        "Odette",
        "Luna",
        "Amphie",
        "Pinky",
        "Loto",
        "Rin",
        "Dewy",
        "Echo",
        "Mona"
    };
    private List<string> crab = new List<string>
    {
        "Shelly",
        "Molt",
        "Sheldon",
        "Crabster",
        "Taxi Crab",
        "Clawdette",
        "Crusty Shawn",
        "Crabstopher",
        "Newton",
        "Kale",
        "Callum",
        "Mona",
        "Clyde"
    };
    private List<string> shrimp = new List<string>
    {
        "Podrick",
        "Cray",
        "Prawnston",
        "Amano",
        "Ryan",
        "Crusty Shawn",
        "Granny",
        "Gramps",
        "Amphie",
        "Newton",
        "Pinky",
        "Smolo",
        "Avogadro",
        "Callum",
        "Bait",
        "Spabloo",
        "Gobo",
        "Loto",
        "Rin"
    };
    private List<string> lobster = new List<string>
    {
        "Shelly",
        "Molt",
        "Sir Lob",
        "Nephyr",
        "Loppester",
        "Lord Clawington III",
        "Clawdette",
        "Crusty Shawn",
        "Lobber",
        "Newton",
        "Kale",
        "Callum",
        "Mona",
        "Clyde"
    };
    private List<string> hermit = new List<string>
    {
        "Shelly",
        "Molt",
        "Podrick",
        "Sheldon",
        "Crabster",
        "Coco",
        "Taxi Crab",
        "Clawdette",
        "Crusty Shawn",
        "Hermie",
        "Rocky",
        "Crabstopher",
        "Newton",
        "Kale",
        "Callum",
        "Mona",
        "Clyde",
        "Avogadro",
        "Socrates",
        "Plato"
    };
    private List<string> seamonkeys = new List<string>
    {
        "Cray",
        "Bubbles",
        "Sallies",
        "Mimimimimi",
        "Bait",
        "Spabloo",
        "Gobo",
        "Loto",
        "Pinky",
        "Amphie",
        "The Time Keepers",
    };

    private List<string> isopod = new List<string>
    {
        "Iso",
        "Isobelle",
        "Sheldon",
        "Limul",
        "Molt",
        "Clay",
        "Newton",
        "Avogadro",
        "The Depth's Judge",
        "Socrates",
        "Plato",
        "Echo",
        "The Time Keeper",
        "Mona",
        "Clyde"
    };

    private List<string> tall = new List<string>
    {
        "Shelly",
        "Molt",
        "Sheldon",
        "Crabster",
        "Taxi Crab",
        "Clawdette",
        "Crusty Shawn",
        "Crabstopher",
        "Loto",
        "Rin",
        "Dewy",
        "Bran",
        "Echo",
        "Mona",
        "Clyde"
    };

    private List<string> ghost = new List<string>
    {
        "Shelly",
        "Molt",
        "Sheldon",
        "Crabster",
        "Taxi Crab",
        "Clawdette",
        "Crusty Shawn",
        "Crabstopher",
        "Echo"
    };

    private List<string> ittybitty = new List<string>
    {
        "Fiddley",
        "Podrick",
        "Kale",
        "Callum",
        "Itty Bitty",
        "Henry",
        "Benjamin",
        "Maxie",
        "Oliver",
        "Nikki"
    };
    private List<string> amphipod = new List<string>
    {
        "Molt",
        "Fiddley",
        "Brac",
        "Rab",
        "Cara",
        "Braxius",
        "Depo",
        "Cragglin",
        "Clay",
        "Dringle",
        "Lilik",
        "Axl",
        "Avogadro",
        "Plato",
        "Stoy",
        "Callum",
        "Loto",
        "Rin",
        "Bran",
        "Echo",
        "Clyde"
    };
    private List<string> axolotl = new List<string>
    {
        "Podrick",
        "Brac",
        "Rab",
        "Cara",
        "Portia",
        "Clay",
        "Lilik",
        "Finn",
        "Axl",
        "Axie",
        "Amphie",
        "Newton",
        "Pinky",
        "Smolo",
        "Avogadro",
        "Socrates",
        "Plato",
        "Stoy",
        "Callum",
        "Spabloo",
        "Gobo",
        "Rin",
        "Dewy",
        "Bran",
        "Echo",
        "Mona",
        "Clyde",
        "Jeemo"
    };
    private List<string> conch = new List<string>
    {
        "Shelly",
        "Molt",
        "Depo",
        "Honshoo",
        "Cragglin",
        "Dringle",
        "Socrates",
        "Plato",
        "Sheldon"
    };
    private List<string> seaSheep = new List<string>
    {
        "Bethy",
        "Fleece",
        "Hector",
        "Rosie",
        "Fluffy",
        "Millie",
        "Dolly",
    };

    private List<string> fish = new List<string>
    {
        "Podrick",
        "Brac",
        "Rab",
        "Cara",
        "Finn",
        "Kale",
        "Callum",
        "Bait",
        "Spabloo",
        "Gobo",
        "Loto",
        "Rin",
        "Dewy",
        "Bran",
        "Mona"
    };
    private Dictionary<CrabInfo.CrabType, List<string>> nameDictionary;

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

        nameDictionary = new Dictionary<CrabInfo.CrabType, List<string>>()
        {
            { CrabInfo.CrabType.catfish, catfish },
            { CrabInfo.CrabType.horseshoe, horseshoe },
            { CrabInfo.CrabType.scopeCreep, scopeCreep },
            { CrabInfo.CrabType.coquette, coquette },
            { CrabInfo.CrabType.crab, crab },
            { CrabInfo.CrabType.shrimp, shrimp },
            { CrabInfo.CrabType.lobster, lobster },
            { CrabInfo.CrabType.hermit, hermit },
            { CrabInfo.CrabType.seamonkeys, seamonkeys },
            { CrabInfo.CrabType.isopod, isopod },
            { CrabInfo.CrabType.ghost, ghost },
            { CrabInfo.CrabType.isopodTiny, isopod },
            { CrabInfo.CrabType.ittybitty, ittybitty },
            { CrabInfo.CrabType.amphipod, amphipod },
            { CrabInfo.CrabType.axolotl, axolotl },
            { CrabInfo.CrabType.conch, conch },
            { CrabInfo.CrabType.seaSheep, seaSheep },
            { CrabInfo.CrabType.pillbug, isopod },
            { CrabInfo.CrabType.tall, tall },
            { CrabInfo.CrabType.fish, fish }
        };
    }

    public string GetNameByType(CrabInfo.CrabType type)
    {
        if (Random.Range(0, 3) <= 2)
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
}
