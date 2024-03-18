using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] MenuButton respawnButton;
    [SerializeField] MenuButton quitButton;
    [SerializeField] PlayerController player;

    private List<MenuButton> buttons = new List<MenuButton>();
    private int index = 0;
    private MenuButton last;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        buttons.Add(respawnButton);
        buttons.Add(quitButton);
        last = buttons[0];
        buttons[0].Select();
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!paused)
            {
                paused = true;
                Time.timeScale = 0f;
                menu.SetActive(true);
            }
            else
            {
                paused = false;
                Time.timeScale = 1f;
                menu.SetActive(false);
            }
        }

        if (paused)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                index++;
                if (index >= buttons.Count)
                    index = 0;
                last.Deselect();
                last = buttons[index];
                buttons[index].Select();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                index--;
                if (index < 0)
                    index = buttons.Count - 1;
                last.Deselect();
                last = buttons[index];
                buttons[index].Select();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.C))
            {
                switch (index)
                {
                    case 0: 
                        player.KillButton();
                        paused = false;
                        Time.timeScale = 1f;
                        menu.SetActive(false); 
                        break;

                    case 1: 
                        SceneManager.LoadScene(2); 
                        break;
                }
            }
        }

    }
}
