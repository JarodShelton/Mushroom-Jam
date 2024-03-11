using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] int numTriggersToActivate = 0;
    [SerializeField] bool reoccuring = false;
    [SerializeField] bool reversable = false;
    [SerializeField] GameObject[] events;

    bool triggered = false;
    int triggers = 0;


    private void Start()
    {
        triggers = numTriggersToActivate;
    }

    public void SetTrigger()
    {
        triggers--;
        if(triggers <= 0 && (reoccuring || !triggered))
            ActivateEvents();
    }

    public void ResetTrigger()
    {
        triggers++;
        if(triggers > 0 && reversable && triggered)
            DeactivateEvents();
    }

    private void ActivateEvents()
    {
        triggered = true;
        foreach (GameObject e in events)
        {
            Component[] eventArray = e.gameObject.GetComponents(typeof(Event));
            foreach (Component comp in eventArray)
            {
                Event i = comp as Event;
                i.Activate();
            }
        }
    }

    private void DeactivateEvents()
    {
        foreach (GameObject e in events)
        {
            Component[] eventArray = e.gameObject.GetComponents(typeof(Event));
            foreach (Component comp in eventArray)
            {
                Event i = comp as Event;
                i.Deactivate();
            }
        }
    }
}
