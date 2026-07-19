using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer Mixer;
    [SerializeField]
    private AudioManager[] audioManagers = new AudioManager[0];
    [SerializeField]
    private AudioMixMode MixMode;

    [SerializeField] private Slider slider;
    [SerializeField] private bool isMasterVolumeControl;

    private void Start()
    {
        float volume = SaveManager.instance.GetSettings_Volume();
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
        SaveManager.instance.SaveSettings(SaveManager.SettingsType.volume, Value.ToString());
    }


    public enum AudioMixMode
    {
        LinearAudioSourceVolume,
        LinearMixerVolume,
        LogrithmicMixerVolume
    }
}