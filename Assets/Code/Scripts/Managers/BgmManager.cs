using NUnit.Framework;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    void Start()
    {
        // play first element in bgm audiomanagers (they will only have one)
        foreach (AudioManager audioManager in GetComponentsInChildren<AudioManager>())
        {
            audioManager.Play(audioManager.sounds[0].name);
        }
    }
}
