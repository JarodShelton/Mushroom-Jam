using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class RoomSkipper : MonoBehaviour
{
    private bool _once = false;
    private int _transitionIndex = 0;
    private TransitionManager[] _transitionList;
    private PlayerController _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _transitionList = FindObjectsOfType<TransitionManager>();
        
        System.Array.Sort( _transitionList,
            (a,b) => { return a.name.CompareTo( b.name); });

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _player.KillButton();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            if (!_once)
            {
                OnceEvent();
                _once = true;
            }

            if (_transitionIndex == 0)
            {
                _transitionIndex = _transitionList.Length - 1;
                _transitionList[_transitionIndex - 1].EnterTrigger();  
            }
            else if(_transitionIndex == 1)
            {
                _player.transform.position = new Vector3(0, -2, 0);
                if (Camera.main != null) Camera.main.transform.position = new Vector3(1.5f, 0, -10);
                _transitionIndex -= 1;
            }
            else
            {
                _transitionIndex -= 1;
                _transitionList[_transitionIndex].ExitTrigger();  
            }
            Debug.Log(("Index: ") + _transitionIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (!_once)
            {
                OnceEvent();
                _once = true;
            }
            
            if (_transitionIndex == _transitionList.Length - 1)
            {
                _transitionIndex = 0;
                _transitionList[0].ExitTrigger();  
            }
            else
            {
                _transitionIndex += 1;
                _transitionList[_transitionIndex - 1].EnterTrigger();  
            }
            Debug.Log(("Index: ") + _transitionIndex);
        }
    }

    private void OnceEvent()
    {
        var foundLights = FindObjectsOfType<Light2D>();
        foreach (var lig in foundLights)
        {
            lig.intensity = 0.2f;
        }
    }
}
