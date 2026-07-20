using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioManager[] audioManagers = new AudioManager[0];
    [SerializeField] private AudioMixMode MixMode;

    [SerializeField] private Slider slider;
    [SerializeField] private bool isMasterVolumeControl;

    public enum SliderType
    {
        master,
        sfx,
        music
    }
    [SerializeField] private SliderType sliderType;
    
    public enum AudioMixMode
    {
        LinearAudioSourceVolume,
        LinearMixerVolume,
        LogrithmicMixerVolume
    }

    private void Start()
    {
        float volume = SaveManager.instance.GetSettings_VolumeMaster(); // default

        switch (sliderType)
        {
            case SliderType.sfx:
                volume = SaveManager.instance.GetSettings_VolumeSFX();
                break;
            case SliderType.music:
                volume = SaveManager.instance.GetSettings_VolumeMusic();
                break;
        }
        Mixer.SetFloat("Volume", Mathf.Log10(volume * 20));
    }

    public void OnChangeSlider(float Value)
    {
        switch (MixMode)
        {
            case AudioMixMode.LinearAudioSourceVolume:
                if (isMasterVolumeControl)
                {
                    foreach (AudioManager audioManager in audioManagers)
                    {
                        audioManager.UpdateMasterVolume(Value);
                    }
                }
                else
                {
                    foreach (AudioManager audioManager in audioManagers)
                    {
                        audioManager.UpdateVolume(Value);
                    }
                }
                break;
            case AudioMixMode.LinearMixerVolume:
                Mixer.SetFloat("Volume", (-80 + Value * 80));
                break;
            case AudioMixMode.LogrithmicMixerVolume:
                Mixer.SetFloat("Volume", Mathf.Log10(Value) * 20);
                break;
        }

        float a = Mathf.Log10(Value) * 20;

        SaveManager.SettingsType settingsType = SaveManager.SettingsType.volumeMaster;
        switch (sliderType)
        {
            case SliderType.sfx:
                settingsType = SaveManager.SettingsType.volumeSFX;
                break;
            case SliderType.music:
                settingsType = SaveManager.SettingsType.volumeMusic;
                break;
        }
        
        SaveManager.instance.SaveSettings(settingsType, Value.ToString());
    }
}