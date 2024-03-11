using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
    [SerializeField] float lifeSpan = 0;
    [SerializeField] float velocity = 0;

    void Update()
    {
        transform.Translate(Vector3.right * velocity * Time.deltaTime);
    }

    void Start()
    {

        StartCoroutine(decay());
    }

    IEnumerator decay()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {
            Component[] interactables = collision.gameObject.GetComponents(typeof(Interactable));
            foreach (Component comp in interactables)
            {
                Interactable i = comp as Interactable;
                i.Interact();
            }
        }
        Destroy(gameObject);
    }
}
