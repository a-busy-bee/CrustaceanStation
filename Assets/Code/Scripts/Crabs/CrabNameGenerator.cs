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
        "Sir Pin",
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
        "Lilik"

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
        "Clay"
    };
    private List<string> horseshoe = new List<string>
    {
        "Iso",
        "Isobelle",
        "Sheldon",
        "Limul",
        "Ancient Being",
        "Clay"
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
        "Luna"
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
        "Crabstopher"
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
        "Gramps"
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
        "Lobber"
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
        "Crabstopher"
    };
    private List<string> seamonkeys = new List<string>
    {
        "Cray",
        "Bubbles",
        "Sallies",
        "Mimimimimi"
    };

    private List<string> isopod = new List<string>
    {
        "Iso",
        "Isobelle",
        "Sheldon",
        "Limul",
        "Molt",
        "Clay"
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
        "Crabstopher"
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
        "Crabstopher"
    };

    private List<string> allNames;
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

        allNames = general.Concat(catfish)
                          .Concat(horseshoe)
                          .Concat(horseshoe)
                          .Concat(coquette)
                          .Concat(crab)
                          .Concat(shrimp)
                          .Concat(lobster)
                          .Concat(hermit)
                          .Concat(isopod)
                          .Concat(tall)
                          .Concat(ghost).ToList();
    }

    public string GetNameByType(CrabInfo.CrabType type)
    {

        if (type == CrabInfo.CrabType.catfish && Random.Range(0, 3) <= 2)
        {
            return catfish[Random.Range(0, catfish.Count)];
        }
        else if (type == CrabInfo.CrabType.horseshoe && Random.Range(0, 3) <= 2)
        {
            return horseshoe[Random.Range(0, horseshoe.Count)];
        }
        else if (type == CrabInfo.CrabType.scopeCreep && Random.Range(0, 3) <= 2)
        {
            return scopeCreep[Random.Range(0, scopeCreep.Count)];
        }
        else if (type == CrabInfo.CrabType.coquette && Random.Range(0, 3) <= 2)
        {
            return coquette[Random.Range(0, coquette.Count)];
        }
        else if (type == CrabInfo.CrabType.crab && Random.Range(0, 3) <= 2)
        {
            return crab[Random.Range(0, crab.Count)];
        }
        else if (type == CrabInfo.CrabType.shrimp && Random.Range(0, 3) <= 2)
        {
            return shrimp[Random.Range(0, shrimp.Count)];
        }
        else if (type == CrabInfo.CrabType.lobster && Random.Range(0, 3) <= 2)
        {
            return lobster[Random.Range(0, lobster.Count)];
        }
        else if (type == CrabInfo.CrabType.hermit && Random.Range(0, 3) <= 2)
        {
            return hermit[Random.Range(0, hermit.Count)];
        }
        else if (type == CrabInfo.CrabType.seamonkeys && Random.Range(0, 3) <= 2)
        {
            return seamonkeys[Random.Range(0, seamonkeys.Count)];
        }
        else if (type == CrabInfo.CrabType.isopod && Random.Range(0, 3) <= 2)
        {
            return isopod[Random.Range(0, isopod.Count)];
        }
        else if (type == CrabInfo.CrabType.ghost && Random.Range(0, 3) <= 2)
        {
            return ghost[Random.Range(0, ghost.Count)];
        }
        else if (type == CrabInfo.CrabType.tall && Random.Range(0, 3) <= 2)
        {
            return tall[Random.Range(0, tall.Count)];
        }
        else
        {
            return general[Random.Range(0, general.Count)];
        }
    }

    public string GetAnyName()
    {
        return allNames[Random.Range(0, allNames.Count)];
    }
}
