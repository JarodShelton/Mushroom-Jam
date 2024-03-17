using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, Interactable
{
    [SerializeField] EventManager manager;

    private bool flipped = false;

    public void Interact()
    {
        if (!flipped)
        {
            AudioManager.Instance.PlaySFXClip("sfx_env_hitSwitch", 0.5f);

            flipped = true;
            manager.SetTrigger();
        }
        else
        {
            flipped = false;
            manager.ResetTrigger();
        }
    }
}
