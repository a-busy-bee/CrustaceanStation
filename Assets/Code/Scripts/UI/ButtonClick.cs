using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;

    void PlayButtonAudio()
    {
        audioManager.Play("button");
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlayButtonAudio);
    }
}
