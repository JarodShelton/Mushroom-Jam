using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    bool selected = false;

    public void Select()
    {
        if (!selected)
        {
            selected = true;
        }
        gameObject.GetComponent<Image>().color = new Color(62/255f, 95 / 255f, 135 / 255f);
    }

    public void Deselect()
    {
        if (selected)
        {
            selected = false;
        }
        gameObject.GetComponent<Image>().color = new Color(128 / 255f, 128 / 255f, 128 / 255f);
    }
}
