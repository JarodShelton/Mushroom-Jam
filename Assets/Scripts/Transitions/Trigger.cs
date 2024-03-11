using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private bool _isEnter = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If player collides with the trigger, check the bool and run according method in parent
        if (other.GetComponent<PlayerController>())
        {
            SendMessageUpwards(_isEnter ? "EnterTrigger" : "ExitTrigger");
        }
    }
}
