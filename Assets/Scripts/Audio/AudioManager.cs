using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public SoundData[] SFXSounds, ambienceSounds, musicSounds;
    [SerializeField] private AudioSource SFXSource, ambienceSource, musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // TODO: Plug in audio sources to Audio Manager
    // TODO: Make volume easier to edit on Audio Manager
        // OR: Add fields for clip volumes for each script...
            // Probably shouldn't need to do much editing in-engine...
    // TODO: Refactor


    // Starter documentation for PlaySFX method and some examples for calling methods below...
    public void PlaySFXClip(string name, Transform sourceTransform) // Do I need to pass volume?
    {
        // Spawn in audioSource gameObject
        AudioSource audioSource = Instantiate(SFXSource, sourceTransform.position, Quaternion.identity);

        // Match string of loaded SoundData clip to the string of call
        // EXAMPLE (of a call from another script): AudioManager.Instance.PlaySFX("sfx_player_sporeShot", transform, 1f)
            // You can create a serialized field for volume on any script or try passing it in directly or both
        SoundData sd = Array.Find(SFXSounds, x => x.name == name);
        // Check if name matches
        if (sd == null)
        {
            Debug.Log("Sound '" + name + "' Not Found");
        }
        // If so, assign clip from SoundData to audioSource gameObject and play from source
        else
        {
            SFXSource.clip = sd.clip;
            // TODO: Check if "OneShot" plays as expected
            SFXSource.PlayOneShot(sd.clip);
        }

        // Get length of clip
        float clipLength = audioSource.clip.length;
        // Destroy the clip after it's done playing
        Destroy(audioSource.gameObject, clipLength);
    }

    // TODO: PlayRandomSFXClip()
        // Need to pass in an array of audioClips
            // Consider that above method is taking a string matching an element from a Sound Data array
            // See if putting an array inside of an array in Unity Editor is allowed
                // Need to expand on Sound Data class
    // OR: Process audio differently at run-time, or both
    
    // TODO: Add fade-in's and -out's

    public void PlayAmbience(string name, Transform sourceTransform)
    {
        AudioSource audioSource = Instantiate(ambienceSource, sourceTransform.position, Quaternion.identity);
        
        SoundData sd = Array.Find(ambienceSounds, x => x.name == name);

        if (sd == null)
        {
            Debug.Log("Sound '" + name + "' Not Found");
        }
        else
        {
            ambienceSource.clip = sd.clip;
            ambienceSource.Play();
        }

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayMusic(string name, Transform sourceTransform)
    {
        AudioSource audioSource = Instantiate(ambienceSource, sourceTransform.position, Quaternion.identity);

        SoundData sd = Array.Find(musicSounds, x => x.name == name);

        if (sd == null)
        {
            Debug.Log("Sound '" + name + "' Not Found");
        }
        else
        {
            musicSource.clip = sd.clip;
            musicSource.Play();
        }

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
