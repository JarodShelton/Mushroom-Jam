using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] float blowSpeed = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Vector2 direction = transform.up;
            player.ApplyForce(direction.normalized * blowSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Vector2 direction = transform.up;
            player.ApplyForce(direction.normalized * -blowSpeed);
        }
    }
}