using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
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
}
