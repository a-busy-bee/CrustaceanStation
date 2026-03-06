using UnityEngine;
using System;

[Serializable]
public class DialogueData
{
    public DialogueNodeGeneric[] nodesGeneric;
    public DialogueNodePlot[] nodesPlot;

    public string[] nodeGenericAnyChars; // casual dialogue, any character
    public DialogueNodePlotAnyChar[] nodePlotAnyChars;    // plot dialogue, any character
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
    private DialogueData dialogueData;
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

    public string GetDialogueGeneric(string character)
    {
        for (int i = 0; i < dialogueData.nodesGeneric.Length; i++)
        {
            if (dialogueData.nodesGeneric[i].character == character)
            {
                return dialogueData.nodesGeneric[i].text[UnityEngine.Random.Range(0, dialogueData.nodesGeneric[i].text.Length)];
            }
        }
        return GetDialogueGeneric();
    }
    public string GetDialogueGeneric()
    {
        return dialogueData.nodeGenericAnyChars[UnityEngine.Random.Range(0, dialogueData.nodeGenericAnyChars.Length)];
    }

    public string GetDialoguePlot(string character, int stage)
    {
        for (int i = 0; i < dialogueData.nodesPlot.Length; i++)
        {
            if (dialogueData.nodesPlot[i].character == character && dialogueData.nodesPlot[i].plotID == stage)
            {
                return dialogueData.nodesPlot[i].text[UnityEngine.Random.Range(0, dialogueData.nodesPlot[i].text.Length)];
            }
        }
        return GetDialogueGeneric();
    }

    public string GetDialoguePlot(int stage)
    {
        return dialogueData.nodePlotAnyChars[stage].text[UnityEngine.Random.Range(0, dialogueData.nodePlotAnyChars[stage].text.Length)];
    }
}
