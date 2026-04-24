using System;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //[SerializeField] private AudioSource audioSource;
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = 1; //PlayerPrefs.GetFloat("Volume"); REENABLE THIS LATER
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.name = s.name;
        }
    }
    public void Play(string name, bool randomize = false)
    {
        //audioSource.UnPause();

        print("playing: " + name);

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            print("sound not found:" + name);
            return;
        }
        if (s.source == null)
        {
            print("no audio source found for " + name);
            return;
        }
        if (randomize)
        {
            s.source.pitch = UnityEngine.Random.Range(0.75f, 1.25f);
        }
        s.source.Play();
    }
}
