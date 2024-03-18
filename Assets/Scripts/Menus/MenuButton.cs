using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    bool selected = false;
    [SerializeField] Color selectColor = new Color(28 / 255f, 140 / 255f, 136 / 255f);
    [SerializeField] Color deselectColor = new Color(120 / 255f, 140 / 255f, 128 / 255f);

    public void Select()
    {
        if (!selected)
        {
            selected = true;
        }
        gameObject.GetComponent<Image>().color = selectColor;
    }

    public void Deselect()
    {
        if (selected)
        {
            selected = false;
        }
        gameObject.GetComponent<Image>().color = deselectColor;
    }
}
