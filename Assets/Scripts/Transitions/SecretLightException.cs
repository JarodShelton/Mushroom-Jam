using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SecretLightException : MonoBehaviour, Interactable
{
    [SerializeField] private Light2D _secretLight;
    
    public void Interact()
    {
        _secretLight.lightOrder = -1;
    }
}
