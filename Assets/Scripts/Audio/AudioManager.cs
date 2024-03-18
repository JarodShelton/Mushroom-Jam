using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public SoundData[] SFXSounds, ambienceSounds, musicSounds;
    //[SerializeField] AudioSource SFXSource, ambienceSource, musicSource;
    public AudioSource SFXSource, ambienceSource, musicSource;

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            PlayMainMenuMusic("music_mainMenuTheme", .15f);
        }
        else if (scene.name == "Game-Mikey")
        {
            PlayLevelMusic("music_levelTheme", .15f);
        }
        //PlayMusic("music_mainMenuTheme", .15f);
    }

    // TODO: Plug in audio sources to Audio Manager (1/3)
    // TODO: Make volume easier to edit on Audio Manager
    // OR: Add fields for clip volumes for each script...
    // Probably shouldn't need to do much editing in-engine...
    // TODO: Refactor


    // Starter documentation for PlaySFX method and some examples for calling methods below...


    //public void PlaySFXClip(string name, Transform sourceTransform, float volume)
    public void PlaySFXClip(string name, float volume)
    {
        // Spawn in audioSource gameObject
        // AudioSource audioSource = Instantiate(SFXSource, sourceTransform.position, Quaternion.identity);

        // Match string of loaded SoundData clip to the string of call
        SoundData sd = Array.Find(SFXSounds, x => x.name == name);

        // Spawn in audioSource gameObject
        // AudioSource audioSource = Instantiate(SFXSource, sourceTransform.position, Quaternion.identity);

        // Check if name matches
        if (sd == null)
        {
            Debug.Log("Sound '" + name + "' Not Found");
        }
        // If so, assign clip from SoundData to audioSource gameObject and play from source
        else
        {
            SFXSource.clip = sd.clip;

            // Asign volume
            SFXSource.volume = volume;
            SFXSource.PlayOneShot(sd.clip);
        }
    }

    // Get length of clip
    // float clipLength = audioSource.clip.length;
    // Destroy the clip after it's done playing
    // Destroy(audioSource.gameObject, clipLength);
    public void PlayLevelMusic(string name, float volume)
    {
        // AudioSource audioSource = Instantiate(ambienceSource, sourceTransform.position, Quaternion.identity);

        SoundData sd = Array.Find(musicSounds, x => x.name == name);

        if (sd == null)
        {
            Debug.Log("Sound '" + name + "' Not Found");
        }
        else
        {
            musicSource.clip = sd.clip;

            musicSource.volume = volume;
            musicSource.Play();
        }

    }

    public void PlayMainMenuMusic(string name, float volume)
    {
        // AudioSource audioSource = Instantiate(ambienceSource, sourceTransform.position, Quaternion.identity);

        SoundData sd = Array.Find(musicSounds, x => x.name == name);

        if (sd == null)
        {
            Debug.Log("Sound '" + name + "' Not Found");
        }
        else
        {
            musicSource.clip = sd.clip;

            musicSource.volume = volume;
            musicSource.Play();
        }

        //float clipLength = audioSource.clip.length;
        //Destroy(audioSource.gameObject, clipLength);
    }

}

    // TODO: PlayRandomSFXClip()
        // Need to pass in an array of audioClips
            // Consider that above method is taking a string matching an element from a Sound Data array
            // See if putting an array inside of an array in Unity Editor is allowed
                // Need to expand on Sound Data class
    // OR: Process audio differently at run-time, or both
    
    // TODO: Add fade-in's and -out's

   