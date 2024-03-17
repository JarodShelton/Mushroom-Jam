using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour, Interactable
{
    [SerializeField] Sprite[] _sprites;

    private SpriteRenderer _sr;

    bool set = false;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        if (!set)
        {
            set = true;
            _sr.sprite = _sprites[1];
        }
        else
        {
            set = false;
            _sr.sprite = _sprites[0];
        }
    }

    public void TimerEnded()
    {
        Interact();
    }
}
