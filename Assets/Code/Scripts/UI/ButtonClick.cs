using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] private string audioName;
    void PlayAudio()
    {
        if (GetComponent<AudioManager>() == null)
        {
            print("needs audio manager");
            return;
        }
        GetComponent<AudioManager>().Play(audioName);
    }

    private void Awake()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(PlayAudio);
    }
}
