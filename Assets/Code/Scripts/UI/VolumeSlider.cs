using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public enum VolumeType
    {
        Master,
        Music,
        SFX
    }
    [SerializeField] private VolumeType volumeType;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;

    private void Start()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                float masterVol = MixerToSliderVal(SaveManager.instance.GetSettings_VolumeMaster());
                slider.value = masterVol;
                break;

            case VolumeType.Music:
                float musicVol = MixerToSliderVal(SaveManager.instance.GetSettings_VolumeMusic());
                slider.value = musicVol;
                break;

            case VolumeType.SFX:
                float sfxVol = MixerToSliderVal(SaveManager.instance.GetSettings_VolumeSFX());
                slider.value = sfxVol;
                break;
        }
    }

    public void OnChangeSlider(float value)
    {
        float newVol = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        AudioManager.instance.ChangeVolume(volumeType, newVol);
    }

    private float MixerToSliderVal(float value)
    {
        return Mathf.Pow(10f, value / 20f); 
    }
}