using SocketIOClient.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject surePanel;
    static public bool isPaused, canPause;
    public static string world, task;
    public static int state;

    // Start is called before the first frame update
    public void Start()
    {
        isPaused = false;
        canPause = true;
        world = "";
        task = "";
        state = 0;
    }

    public static void updateSAve(string str1, string str2, int num)
    {
        world = str1;
        task = str2;
        state = num;
    }

    public void resume()
    {
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = Cursor.visible = false;
        Time.timeScale = 1.0f;
    }

    IEnumerator dataToServer()
    {
        //string jsonUser = "{\"world\": " + world + ", \"task\": " + task + ", \"state\": " + state + "}", response;
        WWWForm form = new WWWForm();
        form.AddField("world", world);
        form.AddField("task", task);
        form.AddField("state", state);
        using (UnityWebRequest request = UnityWebRequest.Post("http://" + MainMenu.serverIp + ":5000/api/Users/Save", form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + Login.token);
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
        StartCoroutine(dataToServer());
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
