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
    [SerializeField] private GameObject _prevLevel;
    [SerializeField] private GameObject _nextLevel;
    
    private Camera _cam;
    private BoxCollider2D _enterTrigger;
    private BoxCollider2D _exitTrigger;
    private Vector3 _prevCamPos;
    private Vector3 _nextCamPos;

    // Camera Lerp
    private float _direction = 1f;
    private float _lerpSpeed = 1 / 0.3f;
    private float _pos = 0f;
    private Coroutine _lerp = null;

    private bool _enterIsVertical;
    private bool _exitIsVertical;
    private float _xOffset = 1.5f;

    private void Awake()
    {
        // Refs
        _cam = Camera.main;
        _enterTrigger = transform.GetChild(0).GetComponent<BoxCollider2D>();
        _exitTrigger = transform.GetChild(1).GetComponent<BoxCollider2D>();
        _prevCamPos = _prevLevel.transform.GetChild(0).transform.position;
        _nextCamPos = _nextLevel.transform.GetChild(0).transform.position;
        
        // Setting boolean if the triggers are horizontal (So I can fix my respawn location math)
        Vector3 enterScale = _enterTrigger.transform.localScale;
        _enterIsVertical = enterScale.y >= enterScale.x;
        Vector3 exitScale = _exitTrigger.transform.localScale;
        _exitIsVertical = exitScale.y >= exitScale.x;
        
        // Reverses offset for player teleport if the enter trigger is to the right of the exit trigger
        if (_enterIsVertical && _exitIsVertical)
        {
            if (_enterTrigger.transform.position.x > _exitTrigger.transform.position.x)
            {
                _xOffset *= -1;
            }
        }
        else
        {
            if (_enterTrigger.transform.position.y > _exitTrigger.transform.position.y)
            {
                _xOffset *= -1;
            }
        }
    }

    // Called by Trigger scripts
    private void EnterTrigger()
    {
        // Y offset to make sure player spawns on the ground
        float yOffset = (_exitTrigger.transform.localScale.y / 2f) - 0.5f;
        
        if(_exitIsVertical)
        {        
            _player.transform.SetPositionAndRotation(_exitTrigger.transform.position + new Vector3(_xOffset,-yOffset,0), Quaternion.identity);
            // Bases the respawn point off of the opposite trigger's location and scale
            Vector2 pos = _exitTrigger.transform.position + new Vector3(_xOffset, -yOffset, 0);
            SetSpawn(pos);
        }
        else
        {
            _player.transform.SetPositionAndRotation(_exitTrigger.transform.position + new Vector3(0,_xOffset,0), Quaternion.identity);
            // Bases the respawn point off of the opposite trigger's location and scale
            Vector2 pos = _exitTrigger.transform.position + new Vector3(0, _xOffset, 0);
            SetSpawn(pos);
        }
        
        if(_lerp != null)
            StopCoroutine(_lerp);
        _direction = 1;
        _lerp = StartCoroutine(LerpCamera());
    }
    
    // Called by Trigger scripts
    private void ExitTrigger()
    {
        // Y offset to make sure player spawns on the ground
        float yOffset = (_enterTrigger.transform.localScale.y / 2f) - 0.5f;

        if (_enterIsVertical)
        {
            _player.transform.SetPositionAndRotation(
                _enterTrigger.transform.position + new Vector3(-_xOffset, -yOffset, 0), Quaternion.identity);
            // Bases the respawn point off of the opposite trigger's location and scale
            Vector2 pos = _enterTrigger.transform.position + new Vector3(-_xOffset, -yOffset, 0);
            SetSpawn(pos);
        }
        else
        {
            _player.transform.SetPositionAndRotation(
                _enterTrigger.transform.position + new Vector3(0, -_xOffset, 0), Quaternion.identity);
            // Bases the respawn point off of the opposite trigger's location and scale
            Vector2 pos = _enterTrigger.transform.position + new Vector3(0, -_xOffset, 0);
            SetSpawn(pos);
        }

        if (_lerp != null)
            StopCoroutine(_lerp);
        _direction = -1;
        _lerp = StartCoroutine(LerpCamera());
    }

    private void SetSpawn(Vector2 pos)
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
            _cam.transform.position = Vector3.Lerp(_prevCamPos, _nextCamPos, _pos);
            yield return null;
        }
    }
}
