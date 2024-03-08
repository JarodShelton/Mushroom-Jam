using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayingEffect : MonoBehaviour
{
    [SerializeField] float lifeSpan = 0;

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
