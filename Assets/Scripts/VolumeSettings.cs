using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider SFXVolume;
    [SerializeField] private string exposedParamMusic;
    [SerializeField] private string exposedParamSFX;

    public void MusicSlider()
    {
        float vol = musicVolume.value;
        if (vol == 0.001f)
        {
            mixer.SetFloat(exposedParamMusic, -80f);
        }
        else
        {
            mixer.SetFloat(exposedParamMusic, Mathf.Log10(vol) * 20);
        }
    }

    public void SFXSlider()
    {
        float vol = SFXVolume.value;
        if (vol == 0.001f)
        {
            mixer.SetFloat(exposedParamSFX, -80f);
        }
        else
        {
            mixer.SetFloat(exposedParamSFX, Mathf.Log10(vol) * 20);
        }
    }
}
