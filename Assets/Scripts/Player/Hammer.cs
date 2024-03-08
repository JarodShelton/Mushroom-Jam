using System.Collections;
using System.Collections.Generic;
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

    private bool canSwing = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && canSwing)
        {
            Vector2 offset = Vector2.zero;

            PlayerController.Direction direction = player.GetFacingDirection();

            switch (direction)
            {
                case PlayerController.Direction.Left: offset = Vector2.left * swingDistance; break;
                case PlayerController.Direction.Right: offset = Vector2.right * swingDistance; break;
                case PlayerController.Direction.Up: offset = Vector2.up * swingDistance; break;
                case PlayerController.Direction.Down: offset = Vector2.down * swingDistance; break;
            }
            Instantiate(hitEffect, transform.position + (Vector3)offset, Quaternion.identity, gameObject.transform);
            bool hitPogo = Physics2D.BoxCast((Vector2)transform.position, new Vector2(2, 3f), 0, offset, swingDistance, pogo);
            if (direction == PlayerController.Direction.Down && hitPogo)
                player.Pogo();
        }
    }

    IEnumerator SwingDelay()
    {
        canSwing = false;
        yield return new WaitForSeconds(swingDelay);
        canSwing = true;
    }
}
