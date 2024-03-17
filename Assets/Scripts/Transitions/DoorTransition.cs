using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DoorTransition : MonoBehaviour, Interactable
{
    [Header("Particle Prefab")]
    [SerializeField] private GameObject _decayParticles;
    [SerializeField] private GameObject _smashParticles;
    [SerializeField] private bool _facesLeft;
    
    [Header("Big Light")]
    [SerializeField] private Light2D _bigLight;
    [SerializeField] private float _bLightIntensity;
    [SerializeField] private float _bLightFadeTime;
    
    [Header("Other Lights (Children)")]
    [SerializeField] private bool _hasRandomDelay;
    [SerializeField] private float _oLightIntensity;
    [SerializeField] private float _oLightFadeTime;

    [Header("Respawn Point")] 
    [SerializeField] private Vector2 _respawnPoint;
    
    private RoomManager _rm;
    private SpriteRenderer _sr;
    private BoxCollider2D _bc;
    
    private Vector2 _newRespawnPoint;
    
    // Allows changing of particle shape so it fits all the door sizes
    private ParticleSystem _ps;
    private readonly ParticleSystemShapeType _boxShape = ParticleSystemShapeType.Box;
    private readonly ParticleSystemShapeType _coneShape = ParticleSystemShapeType.Cone;
    private Vector3 _boxSize;
    
    private void Awake()
    {
        // Y offset to make sure player spawns on the ground
        var localScale = transform.localScale;
        float yOffset = (localScale.y / 2f) - 0.5f;
        
        // Bases the respawn point off of the door's location and scale
        _newRespawnPoint = transform.position - new Vector3(0, yOffset, 0);

        // Set particle shape size to the door's scale
        _boxSize = new Vector3(localScale.x, localScale.y, localScale.z);
        
        // Refs
        _rm = GetComponentInParent<RoomManager>();
        _sr = GetComponent<SpriteRenderer>();
        _bc = GetComponent<BoxCollider2D>();
    }

    public void Interact()
    {
        var localScale = transform.localScale;
        
        // AudioManager.Instance.PlaySFXClip("sfx_env_destroyWall", 0.5f);
        AudioManager.Instance.PlaySFXClip("sfx_level_destroyScreenBarrier", 0.5f);
    
        // Hide Sprite (Make an animation later)
        _sr.color = Color.clear;
        _bc.enabled = false;

        // Instantiate Particles
        GameObject decayParticle = Instantiate(_decayParticles, transform.position, quaternion.identity);
        GameObject smashParticle = Instantiate(_smashParticles, transform.position, Quaternion.Euler(0, 90, 0));
        
        // Apply changes to the particle's shape
        ParticleSystem ps = decayParticle.GetComponent<ParticleSystem>();
        var shape = ps.shape;
        shape.shapeType = _boxShape;
        shape.scale = _boxSize;
        
        ParticleSystem ps2 = smashParticle.GetComponent<ParticleSystem>();
        var shape2 = ps2.shape;
        shape2.shapeType = _coneShape;
        // Shoots down
        if (localScale.x > localScale.y)
        {
            smashParticle.transform.rotation = Quaternion.Euler(90, 90, 0);
        }
        // Shoots left
        else if (_facesLeft)
        {
            smashParticle.transform.rotation = Quaternion.Euler(0, -90, 0);
        }

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

        // Set new checkpoint respawn point (If spawn is set in editor, use that)
        _rm.SetRespawnPoint(_respawnPoint != Vector2.zero ? _respawnPoint : _newRespawnPoint);
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
