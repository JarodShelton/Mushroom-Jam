using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Vector2 _respawnPoint;
    
    private void Awake()
    {
        _respawnPoint = new Vector2(0, 0);
    }
    
    // Used by DoorTransition scripts
    public void SetRespawnPoint(Vector2 pos)
    {
        _respawnPoint = pos;
    }
    
    // Used by Player scripts
    public Vector2 GetRespawnPoint()
    {
        return _respawnPoint;
    }
}
