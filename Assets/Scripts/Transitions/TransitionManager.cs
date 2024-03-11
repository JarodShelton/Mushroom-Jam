using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    
    private Camera _cam;
    private BoxCollider2D _enterTrigger;
    private BoxCollider2D _exitTrigger;
    private Vector3 _camLoc01;
    private Vector3 _camLoc02;

    private float _xOffset = 2;
    private float _yOffset = -1.5f;

    private void Awake()
    {
        _cam = Camera.main;
        _enterTrigger = transform.GetChild(0).GetComponent<BoxCollider2D>();
        _exitTrigger = transform.GetChild(1).GetComponent<BoxCollider2D>();
        _camLoc01 = transform.GetChild(2).transform.position;
        _camLoc02 = transform.GetChild(3).transform.position;

        // Reverses offset for player teleport if the enter trigger is to the right of the exit trigger
        if (_enterTrigger.transform.position.x > _exitTrigger.transform.position.x)
        {
            _xOffset = -2;
        }
    }

    // Called by Trigger scripts
    private void EnterTrigger()
    {
        _player.transform.SetPositionAndRotation(_exitTrigger.transform.position + new Vector3(_xOffset,_yOffset,0), Quaternion.identity);
        _cam.transform.SetPositionAndRotation(_camLoc02, Quaternion.identity);
    }
    
    // Called by Trigger scripts
    private void ExitTrigger()
    {
        _player.transform.SetPositionAndRotation(_enterTrigger.transform.position + new Vector3(-_xOffset, _yOffset,0), Quaternion.identity);
        _cam.transform.SetPositionAndRotation(_camLoc01, Quaternion.identity);
    }
}
