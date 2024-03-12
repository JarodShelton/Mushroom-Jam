using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHitbox : MonoBehaviour
{
    [SerializeField] PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController.Direction direction = player.GetFacingDirection();
        string tag = collision.gameObject.tag;

        if (direction == PlayerController.Direction.Down && tag == "Pogo")
            player.Pogo();
        else if (direction == PlayerController.Direction.Down && tag == "SuperPogo")
            player.SuperPogo();
        else if (tag == "Interactable" || tag == "HammerOnly")
        {
            Component[] interactables = collision.gameObject.GetComponents(typeof(Interactable));
            foreach (Component comp in interactables)
            {
                Interactable i = comp as Interactable;
                i.Interact();
            }
        }
    }
}
