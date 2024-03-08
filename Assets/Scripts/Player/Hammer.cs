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
    [SerializeField] float coliderLength = 2.5f;
    [SerializeField] float coliderHeight = 2;

    private bool canSwing = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && canSwing)
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
            bool hitPogo = Physics2D.BoxCast((Vector2)transform.position, boxSize, 0, offset, swingDistance, pogo);
            if (hitPogo)
                Debug.Log("Hit!");

            StartCoroutine(SwingDelay());
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
