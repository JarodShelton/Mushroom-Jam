using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] float blowSpeed = 10;

    public AudioSource sfx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            AudioManager.Instance.PlaySFXLoop("sfx_player_fanBlowing", .3f);

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Vector2 direction = transform.up;
            player.ApplyForce(direction.normalized * blowSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioManager.Instance.StopSFXLoop("sfx_player_fanBlowing");

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Vector2 direction = transform.up;
            player.ApplyForce(direction.normalized * -blowSpeed);
        }
    }
}
