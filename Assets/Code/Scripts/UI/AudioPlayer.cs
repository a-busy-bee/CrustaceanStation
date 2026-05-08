using System.Runtime.InteropServices;
using UnityEngine;

// Plays a sound from the AudioManager the sound is under
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] string defaultSound;
    [SerializeField] private AudioManager audioManager;

    public void Play(string soundName = "")
    {
        if (audioManager == null)
        {
            print("No AudioManager provided");
            return;
        }

        if (!string.IsNullOrEmpty(soundName))
        {
            // manually input audio to play (side case)
            audioManager.Play(soundName);
        }
        else
        {
            audioManager.Play(defaultSound);
        }
    }
}
