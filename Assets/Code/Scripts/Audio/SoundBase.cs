using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SoundBase
{
    public AudioClip clip;

    public bool loop;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

}
