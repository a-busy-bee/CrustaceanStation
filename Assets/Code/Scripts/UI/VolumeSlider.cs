using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;




/*
i went into this iwth the audiomanager stuff so it probably doesnt work however it originally
did but this is some of the stuff that was there in case u want it
*/

public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer Mixer;
    [SerializeField]
    private AudioMixMode MixMode;

    [SerializeField] private Slider slider;
    private void Awake()
    {
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