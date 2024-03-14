using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightTrigger : MonoBehaviour
{
    [SerializeField] private Light2D _triggeredLight;
    [SerializeField] private float _lightIntensity;
    [SerializeField] private float _lightFadeTime;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If player collides with the trigger, check the bool and run according method in parent
        if (other.GetComponent<PlayerController>())
        {
            StartCoroutine(FadeLight(_triggeredLight, _lightIntensity, _lightFadeTime));
        }
    }
    
    private IEnumerator FadeLight(Light2D lightToFade, float intensity, float fadeTime)
    {
        float elapsedTime = 0f;
        float initialIntensity = lightToFade.intensity;
        while (elapsedTime < fadeTime)
        {
            float newIntensity = Mathf.Lerp(initialIntensity, intensity, elapsedTime / fadeTime);
            lightToFade.intensity = newIntensity;
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the intensity is set to the target value at the end
        lightToFade.intensity = intensity;
    }
}
