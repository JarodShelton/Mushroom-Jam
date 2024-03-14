using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] PlayerController player = null;
    [SerializeField] HammerHitbox hitbox = null;

    [SerializeField] float swingDelay = 0.5f;
    [SerializeField] float swingDistance = 1;
    [SerializeField] float hitboxActiveDuration = 0.1f;

    private bool canSwing = true;

    private void Start()
    {
        hitbox.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && canSwing)
        {
            Vector2 offset = Vector2.zero;
            Quaternion rotation = Quaternion.identity;

            PlayerController.Direction direction = player.GetFacingDirection();

            switch (direction)
            {
                case PlayerController.Direction.Left: 
                    offset = Vector2.left * swingDistance; 
                    rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case PlayerController.Direction.Right: 
                    offset = Vector2.right * swingDistance;
                    rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case PlayerController.Direction.Up: 
                    offset = Vector2.up * swingDistance;
                    rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case PlayerController.Direction.Down: 
                    offset = Vector2.down * swingDistance;
                    rotation = Quaternion.Euler(0, 0, -90);
                    break;
            }

            hitbox.transform.position = (Vector2) transform.position + offset;
            hitbox.transform.rotation = rotation;
            StartCoroutine(ActivateHitbox());
            StartCoroutine(SwingDelay());
            
        }
    }

    IEnumerator ActivateHitbox()
    {
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveDuration);
        hitbox.gameObject.SetActive(false);
    }

    IEnumerator SwingDelay()
    {
        canSwing = false;
        yield return new WaitForSeconds(swingDelay);
        canSwing = true;
    }
}
