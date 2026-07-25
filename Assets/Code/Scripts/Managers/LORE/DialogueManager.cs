using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;

using Special = CrabInfo.SpecialCharacter;


[Serializable]
public class DialogueData
{
    public DialogueNodeGeneric[] nodesGeneric;
    public DialogueNodePlot[] nodesPlot;

    public string[] nodeGenericAnyChars; // casual dialogue, any character
    public DialogueNodePlotAnyChar[] nodePlotAnyChars;    // plot dialogue, any character
 
    //TODO: load these 
    public string[] nodesNonCrustacean;
    public string[] nodesCrustacean;
    public string[] nodesVet;

    public string[] ittyBitty;
    public string[] protestorCatfish;
    public string[] horseshoeCrab;
    public string[] isobelle;
    public string[] seaStarDad;
    public string[] granny;
    public string[] gramps;
}

[Serializable]
public class DialogueNodeGeneric // casual dialogue, character specific
{
    public string character;
    public string[] text;
}

[Serializable]
public class DialogueNodePlot   // plot dialouge, character specific
{
    public int plotID;
    public string character;
    public string[] text;
}

[Serializable]
public class DialogueNodePlotAnyChar
{
    public int plotID;
    public string[] text;
}


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    private DialogueData dialogueData;
    [SerializeField] private DialogueObject dialogueObject;

    private Dictionary<Special, string[]> characterToDialgoue;

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
    }
    private void Start()
    {
        LoadJson();

        characterToDialgoue = new Dictionary<Special, string[]>
        {
            {Special.itty,             dialogueData.ittyBitty},
            {Special.protestorCatfish, dialogueData.protestorCatfish},
            {Special.horseshoe,        dialogueData.horseshoeCrab},
            {Special.isobelle,         dialogueData.isobelle},
            {Special.seaStarDad,       dialogueData.seaStarDad},
            {Special.granny,           dialogueData.granny},
            {Special.gramps,           dialogueData.gramps},
        };
    }

    private void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Dialogues");

        if (jsonFile != null)
        {
            string jsonString = jsonFile.text;

            dialogueData = JsonUtility.FromJson<DialogueData>(jsonString);
        }
        else
        {
            Debug.Log("file not found");
        }
    }

    public void GetDialogueGeneric(string character)
    {
        string text;
        for (int i = 0; i < dialogueData.nodesGeneric.Length; i++)
        {
            if (dialogueData.nodesGeneric[i].character == character)
            {
                text = dialogueData.nodesGeneric[i].text[UnityEngine.Random.Range(0, dialogueData.nodesGeneric[i].text.Length)];
                dialogueObject.ShowDialogue(text);
                return;
            }
        }
        GetDialogueGeneric();
    }
    public void GetDialogueGeneric()
    {
        string text = dialogueData.nodeGenericAnyChars[UnityEngine.Random.Range(0, dialogueData.nodeGenericAnyChars.Length)];
        dialogueObject.ShowDialogue(text);
    }

    public void GetDialoguePlot(string character, int stage)
    {
        string text;
        for (int i = 0; i < dialogueData.nodesPlot.Length; i++)
        {
            if (dialogueData.nodesPlot[i].character == character && dialogueData.nodesPlot[i].plotID == stage)
            {

                text = dialogueData.nodesPlot[i].text[UnityEngine.Random.Range(0, dialogueData.nodesPlot[i].text.Length)];
                dialogueObject.ShowDialogue(text);
                return;
            }
        }
        GetDialogueGeneric();
    }

    public void GetDialoguePlot(int stage)
    {
        string text = dialogueData.nodePlotAnyChars[stage].text[UnityEngine.Random.Range(0, dialogueData.nodePlotAnyChars[stage].text.Length)];
        dialogueObject.ShowDialogue(text);
    }

    public void GetDialogueVet()
    {
        int currDay = SaveManager.instance.GetProgression_CurrDay();
        int stage = currDay / 5;
        string text = dialogueData.nodesVet[stage];
        dialogueObject.ShowDialogue(text);
    }

    public void GetSpecialCharacterDialogue(Special characterName)
    {
        SaveManager saveManager = SaveManager.instance;
        string characterNameString = characterName.ToString();
        int dialgoueIdx = saveManager.GetCharacter_DialogueIdx(characterNameString);

        // safety check to make sure we're not asking for more dialogue than we have
        if (dialgoueIdx >= characterToDialgoue[characterName].Length)
        {
            saveManager.SaveCharacterData(characterNameString, dialgoueIdx, true);
            return;
        }

        string text = characterToDialgoue[characterName][dialgoueIdx];
        dialogueObject.ShowDialogue(text);

        saveManager.SaveCharacterData(characterNameString, dialgoueIdx + 1); // progress dialogue
    }

    public void ClearDialogue()
    {
        dialogueObject.ClearDialogue();
    }
}