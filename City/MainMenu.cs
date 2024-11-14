using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;


public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsPage;
    public GameObject firstMenu;
    public GameObject[] buttonsMain;
    public GameObject[] buttonsOptions;
    public GameObject[] buttonsFirst;
    public GameObject canvas;
    public GameObject loginPage;
    public GameObject registerPage;

    public static string serverIp = "127.0.0.1";


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(delayLogo());
    }

    void OnEnable()
    {

    }

    public void register()
    {
        canvas.transform.GetComponent<Image>().sprite = Resources.Load("Register", typeof(Sprite)) as Sprite;
        firstMenu.SetActive(false);
        registerPage.SetActive(true);
    }

    public void logIn()
    {
        canvas.transform.GetComponent<Image>().sprite = Resources.Load("Login", typeof(Sprite)) as Sprite;
        firstMenu.SetActive(false);
        loginPage.SetActive(true);
    }
    public void logOut()
    {
        canvas.transform.GetComponent<Image>().sprite = Resources.Load("FirstMenu", typeof(Sprite)) as Sprite;
        firstMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void playCity()
    {
        SceneManager.LoadSceneAsync(1);
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
        //StartCoroutine(SendDeleteRequest());
    }


    private IEnumerator SendDeleteRequest()
    {
        // יצירת בקשת מחיקה
        UnityWebRequest request = UnityWebRequest.Delete("http://" + serverIp + ":5000/api/Users/delete");

        // הוספת ה-Header עם ה-TOKEN
        request.SetRequestHeader("Authorization", "Bearer " + Login.token);

        // שליחת הבקשה
        yield return request.SendWebRequest();

        // בדיקת התגובה מהשרת
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("User deleted successfully!");
        }
        else
        {
            Debug.LogError($"Error deleting user: {request.error}");
        }
    }

    private IEnumerator delayLogo()
    {
        yield return new WaitForSeconds(3f);
        canvas.transform.GetComponent<Image>().sprite = Resources.Load("FirstMenu", typeof(Sprite)) as Sprite;
        firstMenu.SetActive(true);
    }
}
