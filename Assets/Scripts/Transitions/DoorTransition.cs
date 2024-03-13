using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DoorTransition : MonoBehaviour, Interactable
{
    [SerializeField] private Light2D _bigLight;
    [SerializeField] private List<Light2D> _lights;
    [SerializeField] private Vector2 _newRespawnPoint;
    [SerializeField] private GameObject _particles;
    
    private RoomManager _rm;
    private SpriteRenderer _sr;
    private BoxCollider2D _bc;
    
    private void Awake()
    {
        _rm = GetComponentInParent<RoomManager>();
        _sr = GetComponent<SpriteRenderer>();
        _bc = GetComponent<BoxCollider2D>();
    }
    
    public void Interact()
    {
        // Hide Sprite (Make an animation later)
        _sr.color = Color.clear;
        _bc.enabled = false;
        
        // Instantiate Particles
        Instantiate(_particles, transform.position, quaternion.identity);
        
        // If there's a _bigLight, activate it
        _bigLight?.gameObject.SetActive(true);
        
        // Activate or fade in smaller lights (Coroutine later)
        foreach (Light2D light in _lights)
        {
            light.intensity = 1;
        }
        
        // Set new checkpoint respawn point
        _rm.SetRespawnPoint(_newRespawnPoint);
    }
}
