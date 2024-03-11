using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] PlayerController player = null;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] LayerMask pogo;

    [SerializeField] float swingDelay = 0.5f;
    [SerializeField] float swingDistance = 1;
    [SerializeField] float coliderLength = 2.5f;
    [SerializeField] float coliderHeight = 2;

    private bool canSwing = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && canSwing)
        {
            Vector2 offset = Vector2.zero;
            Quaternion rotation = Quaternion.identity;
            Vector2 boxSize = Vector2.zero;

            PlayerController.Direction direction = player.GetFacingDirection();

            switch (direction)
            {
                case PlayerController.Direction.Left: 
                    offset = Vector2.left * swingDistance; 
                    rotation = Quaternion.Euler(0, 0, 180);
                    boxSize = new Vector2(coliderLength, coliderHeight);
                    break;
                case PlayerController.Direction.Right: 
                    offset = Vector2.right * swingDistance;
                    rotation = Quaternion.Euler(0, 0, 0);
                    boxSize = new Vector2(coliderLength, coliderHeight);
                    break;
                case PlayerController.Direction.Up: 
                    offset = Vector2.up * swingDistance;
                    rotation = Quaternion.Euler(0, 0, 90);
                    boxSize = new Vector2(coliderHeight, coliderLength);
                    break;
                case PlayerController.Direction.Down: 
                    offset = Vector2.down * swingDistance;
                    rotation = Quaternion.Euler(0, 0, -90);
                    boxSize = new Vector2(coliderHeight, coliderLength);
                    break;
            }

            Instantiate(hitEffect, transform.position + (Vector3)offset, rotation, gameObject.transform);
            Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + offset, boxSize, 0);
            StartCoroutine(SwingDelay());

            foreach(Collider2D collider in colliders)
            {
                string tag = collider.tag;

                if (direction == PlayerController.Direction.Down && tag == "Pogo")
                    player.Pogo();
                else if (direction == PlayerController.Direction.Down && tag == "SuperPogo")
                    player.SuperPogo();
                else if (tag == "Interactable" || tag == "HammerOnly")
                {
                    Component[] interactables = collider.gameObject.GetComponents(typeof(Interactable));
                    foreach (Component comp in interactables)
                    {
                        Interactable i = comp as Interactable;
                        i.Interact();
                    }
                }
            }
            
        }
    }

    IEnumerator SwingDelay()
    {
        canSwing = false;
        yield return new WaitForSeconds(swingDelay);
        canSwing = true;
    }
}
