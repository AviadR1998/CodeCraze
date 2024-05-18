using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsPage;
    public GameObject firstMenu;
    public GameObject[] buttonsMain;
    public GameObject[] buttonsOptions;
    public GameObject[] buttonsFirst;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnGUI()
    {
        /*for (int i = 0; i < buttonsMain.Length; i++)
        {
            buttonsMain[i].transform.position = new Vector3(Screen.width / 2, (Screen.height * (8 - i) / 9) + (-(Screen.height / 26) * i), 0);
            buttonsMain[i].GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 6, 1);
        }*/
        //buttonsMain[0].transform.position = new Vector3(Screen.width / 2, Screen.height * 4 / 5 - 20, 0);
        //buttonsMain[0].GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 6, Screen.height / 500000);
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public void signIn() { }

    public void logIn()
    {
        canvas.transform.GetComponent<Image>().sprite = Resources.Load("MainMenu", typeof(Sprite)) as Sprite;
        firstMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void logOut() 
    {
        canvas.transform.GetComponent<Image>().sprite = Resources.Load("FirstMenu", typeof(Sprite)) as Sprite;
        mainMenu.SetActive(false);
        firstMenu.SetActive(true);
    }

    public void playCity()
    {
        //SceneManager.LoadSceneAsync(1);
    }

    public void options()
    {
        mainMenu.SetActive(false);
        optionsPage.SetActive(true);
    }

    public void restart()
    {
        
    }

    public void backToMainMenu()
    {
        optionsPage.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
    