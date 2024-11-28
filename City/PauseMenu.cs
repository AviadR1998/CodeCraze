using SocketIOClient.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject surePanel;
    public GameObject leaderBoardPage;
    public GameObject pauseButtons;
    public GameObject settingPage;
    static public bool isPaused, canPause;
    public static bool activated = false;

    // Start is called before the first frame update
    public void Start()
    {
        isPaused = false;
        canPause = true;
        activated = true;
    }

    public static void updateSave(string str1, string str2, int num)
    {
        if (Login.world != "Free") {
            Login.world = str1;
            Login.task = str2;
            Login.state = num;
        }
    }

    public void resume()
    {
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = Cursor.visible = false;
        Time.timeScale = 1.0f;
    }

    public void backFromLeaderBoard()
    {
        pausePanel.transform.GetComponent<Image>().sprite = Resources.Load("PauseMenu", typeof(Sprite)) as Sprite;
        leaderBoardPage.SetActive(false);
        pauseButtons.SetActive(true);
    }

    IEnumerator dataToServer()
    {
        WWWForm form = new WWWForm();
        form.AddField("world", Login.world);
        form.AddField("task", Login.task);
        form.AddField("state", Login.state);
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

    public void saveGame()
    {
        if (Login.world != "Free")
        {
            StartCoroutine(dataToServer());
        }
    }

    public void settings()
    {
        SettingsMenu.activateButtons = false;
        settingPage.SetActive(true);
    }

    public void leaderBoard()
    {
        pausePanel.transform.GetComponent<Image>().sprite = Resources.Load("LeaderBoard", typeof(Sprite)) as Sprite;
        pauseButtons.SetActive(false);
        leaderBoardPage.SetActive(true);
    }

    public void exit()
    {
        StartCoroutine(dataToServer());
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(0);
        //surePanel.SetActive(true);
    }

    public void sureOk()
    {
        StartCoroutine(dataToServer());
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(0);
        //Application.Quit();
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
                leaderBoardPage.SetActive(false);
                pausePanel.SetActive(true);
                pauseButtons.SetActive(true);
            }
        }
    }
}
