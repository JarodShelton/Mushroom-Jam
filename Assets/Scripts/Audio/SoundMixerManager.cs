using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    // TODO: Plug in audio sources to Sound Mixer Manager
    // TODO: Modify event listeners and min/max value for panel sliders once sound settings UI created

    // Set and interpolate log to linear volume

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f);

    }

    public void SetAmbienceVolume(float level)
    {
        audioMixer.SetFloat("ambienceVolume", Mathf.Log10(level) * 20f);

    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);

    }
}
