using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] MenuButton startButton;
    [SerializeField] MenuButton quitButton;

    private List<MenuButton> buttons = new List<MenuButton>();
    private int index = 0;
    private MenuButton last;

    // Start is called before the first frame update
    void Start()
    {
        buttons.Add(startButton);
        buttons.Add(quitButton);
        last = buttons[0];
        buttons[0].Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            index++;
            if(index >= buttons.Count)
                index = 0;
            last.Deselect();
            last = buttons[index];
            buttons[index].Select();
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            index--;
            if (index < 0)
                index = buttons.Count - 1;
            last.Deselect();
            last = buttons[index];
            buttons[index].Select();
        }

        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.C))
        {
            switch (index)
            {
                case 0: SceneManager.LoadScene(1); break;
                case 1: Application.Quit(); break;
            }
        }
    }
}
