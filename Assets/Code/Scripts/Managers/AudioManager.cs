using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //[SerializeField] private AudioSource audioSource;
    public Sound[] sounds;
    private Sound _currentTrack;

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
        _currentTrack = s;
    }

    public void Crossfade(string nextTrackName, float duration)
    {
        Sound nextTrack = Array.Find(sounds, sound => sound.name == nextTrackName);

        if (nextTrack == null)
        {
            Debug.LogWarning("Track not found: " + nextTrackName);
            return;
        }
        print("starting fade routine");
        StartCoroutine(FadeRoutine(_currentTrack, nextTrack, duration));
    }

    private IEnumerator FadeRoutine(Sound oldTrack, Sound newTrack, float duration)
    {
        print("running fade routine for" + newTrack.name + " from " + oldTrack.name);
        float currentTime = 0;

        newTrack.source.volume = 0;
        newTrack.source.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;

            if (oldTrack != null)
                oldTrack.source.volume = Mathf.Lerp(1f, 0f, t);

            newTrack.source.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        if (oldTrack != null)
        {
            oldTrack.source.Stop();
        }

        newTrack.source.volume = 1f;
        _currentTrack = newTrack;
    }
}
