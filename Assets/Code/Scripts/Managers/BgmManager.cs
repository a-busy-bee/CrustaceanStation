using NUnit.Framework;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    void Start()
    {
        StartMusic();
    }

    private void StartMusic()
    {
        AudioManager thisAudManager = GetComponent<AudioManager>();

        // play first element for now bc we dont have multiple musics lol
        thisAudManager.Play(thisAudManager.sounds[0].name);
    }
}
