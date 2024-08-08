using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject surePanel;

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    public void resume()
    {
        pauseCanvas.SetActive(false);
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
        
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
