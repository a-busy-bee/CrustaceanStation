using UnityEngine;
using System;

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

    public void ClearDialogue()
    {
        dialogueObject.ClearDialogue();
    }

    
}
