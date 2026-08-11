using UnityEngine;
using System;
using System.Collections.Generic;

using Special = CrabInfo.SpecialCharacter;


[Serializable]
public class DialogueData
{


    public string[] nodeGenericAnyChars; // casual dialogue, any character
    public DialogueNodePlotAnyChar[] nodePlotAnyChars;    // plot dialogue, any character

    //TODO: load these 
    public string[] nodesVet;

    public string[] ittyBitty;
    public string[] protestorCatfish;
    public string[] horseshoeCrab;
    public string[] isobelle;
    public string[] seaStarDad;
    public string[] granny;
    public string[] gramps;
    public string[] gramps_ending;
    public string[] vet1;
    public string[] vet2;
    public string[] vet3;
    public string[] vet4Good;
    public string[] vet4Bad;
    public string[] mailkeeper;
    public string[] mailkeeperGoodEnding;
    public string[] mailkeeperBadEnding;
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
        GetDialogueGeneric();
    }
    public void GetDialogueGeneric()
    {
        //string text = dialogueData.nodeGenericAnyChars[UnityEngine.Random.Range(0, dialogueData.nodeGenericAnyChars.Length)];
        int idx = UnityEngine.Random.Range(0, dialogueData.nodeGenericAnyChars.Length);
        string text = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Dialogue, "dialouge_nodesGenericAnyChars_" + idx);

        dialogueObject.ShowDialogue(text);
    }

    public void GetDialoguePlot(string character, int stage)
    {
        GetDialogueGeneric();
    }

    public void GetDialoguePlot(int stage)
    {
        //string text = dialogueData.nodePlotAnyChars[stage].text[UnityEngine.Random.Range(0, dialogueData.nodePlotAnyChars[stage].text.Length)];

        int idx = UnityEngine.Random.Range(0, dialogueData.nodePlotAnyChars[stage].text.Length);
        string text = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Dialogue, "dialogue_nodePlotAnyChars_1_" + idx);
        
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

        //string text = characterToDialgoue[characterName][dialgoueIdx];

        string prefix = "";
        switch (characterName)
        {
            case Special.itty:
                prefix = "dialogue_ittyBitty_";
                break;

            case Special.protestorCatfish:
                prefix =  "dialogue_protestorCatfish_";
                break;

            case Special.horseshoe:
                prefix =  "dialogue_horseshoe_";
                break;

            case Special.isobelle:
                prefix = "dialogue_isobelle_";
                break;

            case Special.seaStarDad:
                prefix = "dialogue_seaStarDad_";
                break;

            case Special.granny:
                prefix = "dialogue_granny_";
                break;

            case Special.gramps:
                prefix = "dialogue_gramps_";
                break;
        }

        string text = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Dialogue, prefix + dialgoueIdx);

        dialogueObject.ShowDialogue(text, true, characterName);

        saveManager.SaveCharacterData(characterNameString, dialgoueIdx + 1); // progress dialogue
    }

    public DialogueObject.DialogueState GetDialogueState()
    {
        return dialogueObject.GetDialogueState();
    }

    public void ClearDialogue()
    {
        dialogueObject.Skip();
    }

    public void ShowCharacterLongDialogue(CutscenePlayer_Dialogue.DialogueType type, int idx)
    {
        string prefix = "";
        switch (type)
        {
            case CutscenePlayer_Dialogue.DialogueType.grampsEnding:
                prefix = "dialogue_grampsEnding_";
                break;
            case CutscenePlayer_Dialogue.DialogueType.vet1:
                prefix = "dialogue_visit1_";
                break;
            case CutscenePlayer_Dialogue.DialogueType.vet2:
                prefix = "dialogue_visit2_";
                break;
            case CutscenePlayer_Dialogue.DialogueType.vet3:
                prefix = "dialogue_visit3_";
                break;
            case CutscenePlayer_Dialogue.DialogueType.vet4good:
                prefix = "dialogue_visit4_good_";
                break;
            case CutscenePlayer_Dialogue.DialogueType.vet4bad:
                prefix = "dialogue_visit4_bad_";
                break;
        }

        string text = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Dialogue, prefix + idx);
        dialogueObject.ShowDialogue(text);
    }

    public int GetNumDialoguesForCharacterLongDialogue(CutscenePlayer_Dialogue.DialogueType type)
    {
        int numDialogues = 0;
        switch (type)
        {
            case CutscenePlayer_Dialogue.DialogueType.grampsEnding:
                numDialogues = dialogueData.gramps_ending.Length;
                break;
            case CutscenePlayer_Dialogue.DialogueType.vet1:
                numDialogues = dialogueData.vet1.Length;
                break;
            case CutscenePlayer_Dialogue.DialogueType.vet2:
                numDialogues = dialogueData.vet2.Length;
                break;
            case CutscenePlayer_Dialogue.DialogueType.vet3:
                numDialogues = dialogueData.vet3.Length;
                break;
            case CutscenePlayer_Dialogue.DialogueType.vet4good:
                numDialogues = dialogueData.vet4Good.Length;
                break;
            case CutscenePlayer_Dialogue.DialogueType.vet4bad:
                numDialogues = dialogueData.vet4Bad.Length;
                break;
        }

        return numDialogues;
    }

    public void ShowMailkeeperDialogue(int currDay)
    {
        if (!Constants.instance.DIALOGUE_dayToIdxMailkeeper.ContainsKey(currDay)) return;

        string text;
        int idx = Constants.instance.DIALOGUE_dayToIdxMailkeeper[currDay];
        if (currDay == 17 || currDay == 19)
        {
            if (PlotManager.instance.IsGoodEnding())
            {
                // access good dialogues
                //text = dialogueData.mailkeeperGoodEnding[idx];
                text = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Dialogue, "dialogue_mailkeeperGood_" + idx);
            }
            else
            {
                // access bad dialogues
                text = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Dialogue, "dialogue_mailkeeperBad_" + idx);
            }
        }
        else
        {
            // get dialogue from day
            text = LocalizationManager.instance.GetTextByStringKey(LocalizationManager.Table.Dialogue, "dialogue_mailkeeper_" + idx);
        }

        dialogueObject.ShowDialogue(text);
    }
}