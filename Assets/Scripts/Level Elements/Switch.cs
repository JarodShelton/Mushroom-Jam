using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Switch : MonoBehaviour, Interactable
{
    [SerializeField] EventManager manager;

    private RadialTimer _rt;
    private Flipper _fl;
    private Light2D _light;

    private bool flipped = false;

    private void Awake()
    {
        _rt = GetComponent<RadialTimer>();
        _light = GetComponentInChildren<Light2D>();
        _light.enabled = false;
    }
    
    public void Interact()
    {
        if (!flipped)
        {
            AudioManager.Instance.PlaySFXClip("sfx_env_hitSwitch", 0.5f);

            flipped = true;
            manager.SetTrigger();
            _light.enabled = true;
            
            _rt.ResetTimer();
        }
        else
        {
            flipped = false;
            manager.ResetTrigger();
            _light.enabled = false;
            
            _rt.StopTimer();
        }
    }

    public void TimerEnded()
    {
        Interact();
    }
}
