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
    public GameObject canvas;
    public GameObject loginPage;
    public GameObject registerPage;
    public GameObject practicePage;
    public GameObject freePlayPage;
    public GameObject chooseTopicsPanel;
    public UnityEngine.UI.Toggle[] topicToggleList;

    string[] topicList;
    public static string topicListSaved;
    public static string serverIp = "10.0.0.9";

    private bool[] topicToggleBoolList;

    // Start is called before the first frame update
    void Start()
    {
        if (RoomsMenu.activated)
        {
            RoomsMenu.activated = false;
            canvas.transform.GetComponent<Image>().sprite = Resources.Load("MainMenu", typeof(Sprite)) as Sprite;
            mainMenu.SetActive(true);
        }
        else
        {
            StartCoroutine(delayLogo());
        }
        topicList = new string[] { "Vars", "IO", "Arithmetic", "Logic", "If", "Loops", "Arrays", "Functions", "Class", "Recursion" };
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

    IEnumerator newGameRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("world", "Forest");
        form.AddField("task", "Swing");
        form.AddField("state", 0);
        using (UnityWebRequest request = UnityWebRequest.Post("http://" + MainMenu.serverIp + ":5000/api/Users/SaveState", form))
        {
            request.SetRequestHeader("authorization", "Bearer " + Login.token);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Login.world = "Forest";
                Login.task = "Swing";
                Login.state = 0;
            }
            else
            {
            }
        }
    }


    public void newGame()
    {
        StartCoroutine(newGameRequest());
        SceneManager.LoadSceneAsync(1);
    }

    public void playWorld(int i)
    {
        SceneManager.LoadSceneAsync(i);
    }

    public void continueFunc()
    {
        if (Login.world == "Free")
        {
            canvas.transform.GetComponent<Image>().sprite = Resources.Load("FirstMenu", typeof(Sprite)) as Sprite;
            mainMenu.SetActive(false);
            freePlayPage.SetActive(true);
        }
        else
        {
            if (Login.world == "Forest")
            {
                SceneManager.LoadSceneAsync(1);
            }
            if (Login.world == "City")
            {
                SceneManager.LoadSceneAsync(2);
            }
            if (Login.world == "Island")
            {
                SceneManager.LoadSceneAsync(3);
            }
        }

    }

    public void options()
    {
        mainMenu.SetActive(false);
        optionsPage.SetActive(true);
    }

    public void practice()
    {
        mainMenu.SetActive(false);
        practicePage.SetActive(true);
        topicToggleBoolList = new bool[topicToggleList.Length];
        for (int i = 0; i < topicToggleList.Length; i++)
        {
            topicToggleBoolList[i] = topicToggleList[i].isOn = false;
        }
    }
    
    public void clickCheckBox(int index)
    {
        topicToggleBoolList[index] = !topicToggleBoolList[index];
    }

    public void backFromPractice()
    {
        practicePage.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void backFromFreePlay()
    {
        freePlayPage.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void searchRooms()
    {
        topicListSaved = "";
        for (int i = 0; i < topicToggleBoolList.Length; i++) 
        {
            if (topicToggleBoolList[i])
            {
                topicListSaved += (topicList[i] + ", ");
            }
        }
        if (topicListSaved == "")
        {
            for (int i = 0; i < topicToggleBoolList.Length; i++)
            {
                topicListSaved += (topicList[i] + ", ");
            }
        }
        topicListSaved = topicListSaved.Remove(topicListSaved.Length - 2, 1);
        chooseTopicsPanel.SetActive(false);
        SceneManager.LoadSceneAsync(4);
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
        UnityWebRequest request = UnityWebRequest.Delete("http://" + serverIp + ":5000/api/Users/delete");
        request.SetRequestHeader("authorization", "Bearer " + Login.token);
        yield return request.SendWebRequest();
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
