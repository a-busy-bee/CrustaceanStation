using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public enum SoundNames
    {
        Rain,
        Waves,
        Punch,
        Click,
        DoorClose,
        DoorOpen,
        KioskButton,
        Switch,
        Ticket
    }

    public enum ThemeNames
    {
        CheckingIntoStation,
        CrustyCorp,
        HermitWaltz,
        Iso,
        Mailroom,
        RidingInStyle
    }


    [SerializeField] private Sound[] sounds;
    private SoundBase currentSFX;
    
    [SerializeField] private BkgTheme[] themes;
    [SerializeField] private AudioSource themeSource; // child obj
    [SerializeField] private AudioSource nextThemeSource; // child obj
    private float localVol = 1f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        // SFX
        GameObject audioManagerObj = gameObject.transform.GetChild(1).gameObject;
        foreach (Sound s in sounds)
        {
            s.source = audioManagerObj.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        SetTheme(ThemeNames.CheckingIntoStation);
    }

    #region SFX
    public void PlaySound(SoundNames name, bool randomize = false)
    {
        SoundBase s = Array.Find(sounds, sound => sound.name == name);

        if (s == null || s.source == null) return;

        if (randomize)
        {
            s.source.pitch = UnityEngine.Random.Range(0.85f, 1.25f);
        }
        s.source.Play();
    }

    public void StopSound(SoundNames name)
    {
        SoundBase s = Array.Find(sounds, sound => sound.name == name);

        if (s == null || s.source == null) return;

        s.source.Stop();
    }

    public void CrossfadeSFX(SoundNames nextTrackName, float duration)
    {
        SoundBase nextTrack = Array.Find(sounds, sound => sound.name == nextTrackName);

        if (nextTrack == null) return;

        // check if alr playing
        if (currentSFX != null && currentSFX == nextTrack)
        {
            if (!currentSFX.source.isPlaying) currentSFX.source.Play();
            return;
        }
        StartCoroutine(FadeSFXRoutine(currentSFX, nextTrack, duration));
    }

    private IEnumerator FadeSFXRoutine(SoundBase oldTrack, SoundBase newTrack, float duration)
    {
        float currentTime = 0;

        newTrack.volume = 0;
        newTrack.source.volume = 0f;
        newTrack.source.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;

            if (oldTrack != null)
                oldTrack.volume = Mathf.Lerp(1f, 0f, t);

            newTrack.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        if (oldTrack != null)
        {
            oldTrack.source.Stop();
        }

        newTrack.volume = 1f;
    }

    public void ChangeSFXVolume(float newVolume)
    {
        foreach (Sound s in sounds)
        {
            s.volume = newVolume;
            s.source.volume = newVolume;
        }
    }

    #endregion

    #region Music
    public void SetTheme(ThemeNames name) // only called in title screen when everything starts up 
    {
        BkgTheme theme = Array.Find(themes, sound => sound.name == name);

        if (theme == null) return;

        themeSource.loop = theme.loop;
        themeSource.volume = theme.volume;
        themeSource.clip = theme.clip;
        themeSource.Play();
    }

    public void SwitchTheme(ThemeNames name)
    {
        BkgTheme theme = Array.Find(themes, theme => theme.name == name);

    }

    public void CrossfadeMusic(ThemeNames nextTrackName, float duration)
    {
        // check if alr playing
        if (themeSource.clip == nextThemeSource.clip)
        {
            if (!themeSource.isPlaying) themeSource.Play();
            return;
        }

        BkgTheme theme = Array.Find(themes, theme => theme.name == nextTrackName);

        nextThemeSource.clip = theme.clip;
        nextThemeSource.loop = theme.loop;
        nextThemeSource.volume = 0f;
        StartCoroutine(FadeMusicRoutine(themeSource, nextThemeSource, duration));
    }

    private IEnumerator FadeMusicRoutine(AudioSource oldTrack, AudioSource newTrack, float duration)
    {
        float currentTime = 0;

        newTrack.volume = 0;
        newTrack.volume = 0f;
        newTrack.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;

            if (oldTrack != null)
                oldTrack.volume = Mathf.Lerp(1f, 0f, t);

            newTrack.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        if (oldTrack != null)
        {
            oldTrack.Stop();
        }

        newTrack.volume = 1f;

        AudioSource temp = themeSource; // the ol' switcheroo
        themeSource = nextThemeSource;
        nextThemeSource = temp;
    }

    public void ChangeMusicVolume(float newVolume)
    {
        themeSource.volume = newVolume;
    }

    #endregion
}
