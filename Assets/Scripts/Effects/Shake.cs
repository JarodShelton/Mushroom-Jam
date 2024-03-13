using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour, Interactable
{
    public Vector3 amount = new Vector3(1f, 1f, 0);
    public float duration = 1;
    public float speed = 10;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public bool deltaMovement = true;

    public void Interact()
    {
        ScreenShake.ShakeOnce(duration, speed, amount, Camera.main, deltaMovement, curve);
    }
}
