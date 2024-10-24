using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject surePanel;
    static public bool isPaused, canPause;

    // Start is called before the first frame update
    public void Start()
    {
        isPaused = false;
        canPause = true;
    }

    public void resume()
    {
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = Cursor.visible = false;
        Time.timeScale = 1.0f;
    }

    public void saveGame()
    {

    }

    public void settings()
    {

    }

    public void exit()
    {
        surePanel.SetActive(true);
    }

    public void sureOk()
    {
        Application.Quit();
    }

    public void sureBack()
    {
        surePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            if (isPaused)
            {
                resume();
                surePanel.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
                isPaused = Cursor.visible = true;
                pausePanel.SetActive(true);
            }
        }
    }
}
