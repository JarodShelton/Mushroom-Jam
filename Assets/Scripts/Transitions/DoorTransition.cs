using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DoorTransition : MonoBehaviour, Interactable
{
    [Header("Particle Prefab")]
    [SerializeField] private GameObject _particles;
    
    [Header("Big Light")]
    [SerializeField] private Light2D _bigLight;
    [SerializeField] private float _bLightIntensity;
    [SerializeField] private float _bLightFadeTime;
    
    [Header("Other Lights (Children)")]
    [SerializeField] private bool _hasRandomDelay;
    [SerializeField] private float _oLightIntensity;
    [SerializeField] private float _oLightFadeTime;
    
    private RoomManager _rm;
    private SpriteRenderer _sr;
    private BoxCollider2D _bc;
    
    private Vector2 _newRespawnPoint;
    
    private void Awake()
    {
        // Y offset to make sure player spawns on the ground
        float yOffset = (transform.localScale.y / 2f) - 0.5f;
        // Bases the respawn point off of the door's location and scale
        _newRespawnPoint = transform.position - new Vector3(0, yOffset, 0);
        
        
        // Refs
        _rm = GetComponentInParent<RoomManager>();
        _sr = GetComponent<SpriteRenderer>();
        _bc = GetComponent<BoxCollider2D>();
    }

    public void Interact()
    {
        // AudioManager.Instance.PlaySFXClip("sfx_env_destroyWall", 0.5f);
        AudioManager.Instance.PlaySFXClip("sfx_level_destroyScreenBarrier", 0.5f);
    
        // Hide Sprite (Make an animation later)
        _sr.color = Color.clear;
        _bc.enabled = false;

        // Instantiate Particles
        Instantiate(_particles, transform.position, quaternion.identity);

        // If there's a _bigLight, fade it in
        if (_bigLight != null)
        {
            StartCoroutine(FadeLight(_bigLight, _bLightIntensity, _bLightFadeTime));
        }

        // Fade in other lights, must be children of the door. 
        foreach (Transform child in transform)
        {
            // Ref
            Light2D childLight = child.GetComponent<Light2D>();
            // Makes sure it's enabled
            childLight.enabled = true;
            
            // Coroutine with an if statement. Checks if small lights should fade in at random times or together
            StartCoroutine(_hasRandomDelay
                ? FadeLight(childLight, _oLightIntensity, Random.Range(0f, _oLightFadeTime))
                : FadeLight(childLight, _oLightIntensity, _oLightFadeTime));
        }

        // Set new checkpoint respawn point
        _rm.SetRespawnPoint(_newRespawnPoint);
    }

    private IEnumerator FadeLight(Light2D lightToFade, float intensity, float fadeTime)
    {
        float elapsedTime = 0f;
        float initialIntensity = lightToFade.intensity;
        while (elapsedTime < fadeTime)
        {
            float newIntensity = Mathf.Lerp(initialIntensity, intensity, elapsedTime / fadeTime);
            lightToFade.intensity = newIntensity;
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the intensity is set to the target value at the end
        lightToFade.intensity = intensity;
    }
    
}
