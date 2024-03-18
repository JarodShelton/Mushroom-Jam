using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Event
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float speed = 5;

    private bool open = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(open && transform.position != endPoint.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPoint.position, speed*Time.deltaTime);
        }
        else if(!open && transform.position != startPoint.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPoint.position, speed * Time.deltaTime);
        }
    }
    public void Activate()
    {
        open = true;
    }

    public void Deactivate()
    {
        open = false;
    }

}
