using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Transform anchor;
    Vector2 shakeVector = Vector3.zero;
    float shakeDuration = 0f;
    float shakeTimer = 0f;
    float undirectedShakeAmplitude = 0f;
    bool directedShake = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 tempPosition = (Vector2) anchor.position;
        if (shakeTimer < shakeDuration)
        {
            if (directedShake)
            {
                tempPosition += shakeVector * Mathf.Sin(Mathf.PI / shakeDuration * shakeTimer);
            }
            else
            {
                float angle = Random.Range(0f, Mathf.PI * 2f);
                Vector2 randomVector = new Vector2(Mathf.Cos(angle) * undirectedShakeAmplitude, Mathf.Sin(angle) * undirectedShakeAmplitude);
                tempPosition += randomVector * Mathf.Sin(Mathf.PI / shakeDuration * shakeTimer);
            }

            shakeTimer += Time.deltaTime;
        }

        transform.position = tempPosition;
        
    }

    // Shakes in many random positions
    public void Shake(float amplitude, float duration)
    {
        if (shakeTimer >= shakeDuration || amplitude >= shakeVector.magnitude)
        {
            directedShake = false;
            shakeDuration = duration;
            undirectedShakeAmplitude = amplitude;
        }
    }

    public void Shake(float angle, float amplitude, float duration)
    {
        float angleRadians = angle * Mathf.Deg2Rad;
        if (shakeTimer >= shakeDuration || amplitude >= shakeVector.magnitude)
        {
            directedShake = true;
            shakeVector = new Vector3(Mathf.Cos(angleRadians) * amplitude, Mathf.Sin(angleRadians) * amplitude, 0);
            shakeDuration = duration;
            shakeTimer = 0f;
        }
    }

    public void Shake(Vector2 direction, float amplitude, float duration)
    {
        if (shakeTimer >= shakeDuration || amplitude >= shakeVector.magnitude)
        {
            directedShake = true;
            shakeVector = direction.normalized * amplitude;
            shakeDuration = duration;
            shakeTimer = 0f;
        }
    }

}