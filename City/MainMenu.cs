using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Reflection;


public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingPage;
    public GameObject firstMenu;
    public GameObject canvas;
    public GameObject loginPage;
    public GameObject registerPage;
    public GameObject practicePage;
    public GameObject freePlayPage;
    public GameObject leaderBoardPage;
    public GameObject chooseTopicsPanel;
    public GameObject[] topicToggleList;
    public GameObject ipChangeEmpty;
    public GameObject AIImage;
    public TMP_InputField ipChangeInput;

    string[] topicList;
    public static string topicListSaved;
    public static string serverIp = "127.0.0.1";
    public static bool activateRace = false, GPTActive;

    private bool[] topicToggleBoolList;

    const int FULL_COLOR = 255, FOREST_INDEX = 1, CITY_INDEX = 2, ISLAND_INDEX = 3, RACE_INDEX = 4, DELAY_LOGO = 3;
    // Start is called before the first frame update
    void Start()
    {
        if (RoomsMenu.activated || PauseMenu.activated)
        {
            PauseMenu.activated = RoomsMenu.activated = false;
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
        GPTActive = true;
        AIImage.transform.GetComponent<Image>().sprite = Resources.Load("ChatGPT", typeof(Sprite)) as Sprite;
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
        Login.token = "";
        Login.usernameConnected = "";
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

            }
            else
            {
            }
        }
    }


    public void newGame()
    {
        Login.world = "Forest";
        Login.task = "Swing";
        Login.state = 0;
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
            canvas.transform.GetComponent<Image>().sprite = Resources.Load("FreePlay", typeof(Sprite)) as Sprite;
            mainMenu.SetActive(false);
            freePlayPage.SetActive(true);
        }
        else
        {
            if (Login.world == "Forest")
            {
                SceneManager.LoadSceneAsync(FOREST_INDEX);
            }
            if (Login.world == "City")
            {
                Movement.loadOnce = true;
                SceneManager.LoadSceneAsync(CITY_INDEX);
            }
            if (Login.world == "Island")
            {
                SceneManager.LoadSceneAsync(ISLAND_INDEX);
            }
        }

    }

    public void options()
    {
        SettingsMenu.activateButtons = true;
        settingPage.SetActive(true);
    }

    public void practice()
    {
        if (Login.world == "Free")
        {
            canvas.transform.GetComponent<Image>().sprite = Resources.Load("Race", typeof(Sprite)) as Sprite;
            mainMenu.SetActive(false);
            practicePage.SetActive(true);
            topicToggleBoolList = new bool[topicToggleList.Length];
            for (int i = 0; i < topicToggleList.Length; i++)
            {
                topicToggleBoolList[i] = false;
                topicToggleList[i].transform.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }
        }
    }

    public void clickCheckBox(int index)
    {
        topicToggleBoolList[index] = !topicToggleBoolList[index];
        if (topicToggleBoolList[index])
        {
            topicToggleList[index].transform.GetComponent<Image>().color = new Color(0, 0, 0, FULL_COLOR);
        }
        else
        {
            topicToggleList[index].transform.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }

    }

    public void backFromPractice()
    {
        canvas.transform.GetComponent<Image>().sprite = Resources.Load("MainMenu", typeof(Sprite)) as Sprite;
        practicePage.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void backFromFreePlay()
    {
        canvas.transform.GetComponent<Image>().sprite = Resources.Load("MainMenu", typeof(Sprite)) as Sprite;
        freePlayPage.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void leaderBoardUpload()
    {
        canvas.transform.GetComponent<Image>().sprite = Resources.Load("LeaderBoard", typeof(Sprite)) as Sprite;
        chooseTopicsPanel.SetActive(false);
        leaderBoardPage.SetActive(true);
    }

    public void backFromLeaderBoard()
    {
        canvas.transform.GetComponent<Image>().sprite = Resources.Load("Race", typeof(Sprite)) as Sprite;
        leaderBoardPage.SetActive(false);
        chooseTopicsPanel.SetActive(true);
    }

    public void searchRooms()
    {
        AdminMission.questions = new Stack<string>();
        AdminMission.answers = new Stack<string>();
        AdminMission.rightAnswers = new Stack<string>();
        AdminMission.loadAll = AdminMission.geminiActivate = false;
        activateRace = true;
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
        SceneManager.LoadSceneAsync(RACE_INDEX);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator delayLogo()
    {
        yield return new WaitForSeconds(DELAY_LOGO);
        canvas.transform.GetComponent<Image>().sprite = Resources.Load("FirstMenu", typeof(Sprite)) as Sprite;
        firstMenu.SetActive(true);
    }

    public void saveIP()
    {
        serverIp = ipChangeInput.text;
        print(serverIp);
    }

    public void changeModel()
    {
        GPTActive = !GPTActive;
        if (GPTActive)
        {
            AIImage.transform.GetComponent<Image>().sprite = Resources.Load("ChatGPT", typeof(Sprite)) as Sprite;
        }
        else
        {
            AIImage.transform.GetComponent<Image>().sprite = Resources.Load("Gemini", typeof(Sprite)) as Sprite;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ipChangeEmpty.SetActive(!ipChangeInput.IsActive());
        }
    }
}
