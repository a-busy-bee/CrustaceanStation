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

    private void Awake()
    {
        print(slider.value);
        print(PlayerPrefs.GetFloat("Volume"));
        //slider.value = 1f;//PlayerPrefs.GetFloat("Volume");
    }
    private void Start()
    {
        Mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume", 1) * 20));
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

        PlayerPrefs.SetFloat("Volume", Value);
        PlayerPrefs.Save();
    }


    public enum AudioMixMode
    {
        LinearAudioSourceVolume,
        LinearMixerVolume,
        LogrithmicMixerVolume
    }
}