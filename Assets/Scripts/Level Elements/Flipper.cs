using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour, Interactable
{
    [SerializeField] SpriteRenderer sprite;

    bool set = false;
    // Start is called before the first frame update
    void Start()
    {
        sprite.color = Color.red;
    }

    public void Interact()
    {
        if (!set)
        {
            set = true;
            sprite.color = Color.green;
        }
        else
        {
            set = false;
            sprite.color = Color.red;
        }
    }
}
