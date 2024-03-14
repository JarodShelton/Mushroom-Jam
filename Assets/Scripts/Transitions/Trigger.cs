using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private bool _isEnter = true;
    [SerializeField] private Vector2 _respawnPoint;

    private TransitionManager _tm;

    private void Awake()
    {
        _tm = GetComponentInParent<TransitionManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If player collides with the trigger, check the bool and run according method in parent
        if (other.GetComponent<PlayerController>())
        {
            _tm.SetSpawn(_respawnPoint);
            SendMessageUpwards(_isEnter ? "EnterTrigger" : "ExitTrigger");
        }
    }
}
