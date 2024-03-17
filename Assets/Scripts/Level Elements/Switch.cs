using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, Interactable
{
    [SerializeField] EventManager manager;

    private RadialTimer _rt;
    private Flipper _fl;

    private bool flipped = false;

    private void Awake()
    {
        _rt = GetComponent<RadialTimer>();
    }

    public void Interact()
    {
        if (!flipped)
        {
            AudioManager.Instance.PlaySFXClip("sfx_env_hitSwitch", 0.5f);

            flipped = true;
            manager.SetTrigger();
            
            _rt.ResetTimer();
        }
        else
        {
            flipped = false;
            manager.ResetTrigger();
            
            _rt.StopTimer();
        }
    }

    public void TimerEnded()
    {
        Interact();
    }
}
