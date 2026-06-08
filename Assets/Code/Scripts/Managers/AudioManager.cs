using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    //[SerializeField] private AudioSource audioSource;
    public Sound[] sounds = new Sound[0];
    private Sound _currentTrack;
    private float localVol = 1f;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            if (s == null)
            {
                continue;
            }
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.name = s.name;
        }
    }
    public void Play(string name, bool randomize = false)
    {
        //audioSource.UnPause();

        //Debug.Log("playing: " + name);

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
            s.source.pitch = UnityEngine.Random.Range(0.85f, 1.25f);
        }
        s.source.Play();
        _currentTrack = s;
    }

    public void UpdateVolume(float vol)
    {
        localVol = vol;
        foreach (Sound s in sounds)
        {
            if (s == null)
            {
                continue;
            }
            s.source.volume = vol * s.volume;
        }
    }

    public void UpdateMasterVolume(float masterVol)
    {
        foreach (Sound s in sounds)
        {
            if (s == null)
            {
                continue;
            }
            s.source.volume = masterVol * localVol * s.volume;
        }
    }

    public void Crossfade(string nextTrackName, float duration)
    {

        Sound nextTrack = Array.Find(sounds, sound => sound.name == nextTrackName);

        if (nextTrack == null)
        {
            Debug.LogWarning("Track not found: " + nextTrackName);
            return;
        }

        // check if alr playing
        if (_currentTrack != null && _currentTrack.name == nextTrackName)
        {
            if (!_currentTrack.source.isPlaying) _currentTrack.source.Play();
            return;
        }
        StartCoroutine(FadeRoutine(_currentTrack, nextTrack, duration));
    }

    private IEnumerator FadeRoutine(Sound oldTrack, Sound newTrack, float duration)
    {
        //print("running fade routine from " + oldTrack.name + " to " + newTrack.name);
        float currentTime = 0;

        newTrack.source.volume = 0;
        newTrack.source.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;

            if (oldTrack != null)
                oldTrack.source.volume = Mathf.Lerp(sounds[0].source.volume, 0f, t);

            newTrack.source.volume = Mathf.Lerp(0f, sounds[0].source.volume, t);

            yield return null;
        }

        if (oldTrack != null)
        {
            oldTrack.source.Stop();
        }

        newTrack.source.volume = sounds[0].source.volume;
        _currentTrack = newTrack;
    }
}
