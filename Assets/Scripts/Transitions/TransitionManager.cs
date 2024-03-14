using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private RoomManager _rm;
    
    private Camera _cam;
    private BoxCollider2D _enterTrigger;
    private BoxCollider2D _exitTrigger;
    private Vector3 _camLoc01;
    private Vector3 _camLoc02;

    private float _direction = 1f;
    private float _lerpSpeed = 1/ 0.3f;
    private float _pos = 0f;

    private Coroutine _lerp = null;

    private float _xOffset = 1.5f;
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
        if(_lerp != null)
            StopCoroutine(_lerp);
        _direction = 1;
        _lerp = StartCoroutine(LerpCamera());
    }
    
    // Called by Trigger scripts
    private void ExitTrigger()
    {
        _player.transform.SetPositionAndRotation(_enterTrigger.transform.position + new Vector3(-_xOffset, _yOffset,0), Quaternion.identity);
        if (_lerp != null)
            StopCoroutine(_lerp);
        _direction = -1;
        _lerp = StartCoroutine(LerpCamera());
    }

    public void SetSpawn(Vector2 pos)
    {
        _rm.SetRespawnPoint(pos);
    }

    IEnumerator LerpCamera()
    {
        float target = _direction > 0 ? 1 : 0;
        while (_pos != target)
        {
            _pos += _lerpSpeed * _direction * Time.deltaTime;
            _pos = Mathf.Clamp(_pos, 0, 1);
            _cam.transform.position = Vector3.Lerp(_camLoc01, _camLoc02, _pos);
            yield return null;
        }
    }
}
