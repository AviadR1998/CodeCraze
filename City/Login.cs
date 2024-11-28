using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.WSA;

[Serializable]
public class QuestionJSON
{
    public string id;
    public string question;
    public List<string> options;
    public string answer;
    public string explanation;
    public string topic;
}

[Serializable]
public class QuestionListJSON
{
    public List<QuestionJSON> questionList;
}

[Serializable]
public class SaveInfoJSON
{
    public string world;
    public string task;
    public int state;
}

public class Login : MonoBehaviour
{
    public static string token, usernameConnected;
    public static string world, task;
    public static int state;
    public GameObject mainMenu;
    public GameObject firstMenu;
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public GameObject errorMessage;
    public GameObject canvas;
    public GameObject loginPage;

    string[] topics = { "Var", "InputOutput", "CountPlusPlus", "Arithmetic", "Logical", "for", "if", "array", "while", "dowhile" };


    // Start is called before the first frame update
    void Start()
    {
        /*world = "City";
        task = "Array";
        state = 1;*/
    }

    void OnEnable()
    {
        usernameConnected = "";
        token = "";
        usernameField.text="";
        passwordField.text="";
        errorMessage.SetActive(false);
    }

    public IEnumerator loginRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);
        using (UnityWebRequest request = UnityWebRequest.Post("http://" + MainMenu.serverIp + ":5000/api/Tokens/", form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                errorMessage.SetActive(true);
            else
            {
                usernameConnected = usernameField.text;
                token = request.downloadHandler.text;
                foreach(string topic in topics)
                {
                    UnityWebRequest requestQuestions = UnityWebRequest.Get("http://" + MainMenu.serverIp + ":5000/api/Questions/Topic/" + topic);
                    requestQuestions.SetRequestHeader("authorization", "Bearer " + Login.token);
                    yield return requestQuestions.SendWebRequest();
                    if (requestQuestions.result == UnityWebRequest.Result.Success)
                    {
                        QuestionListJSON questionList = JsonUtility.FromJson<QuestionListJSON>(requestQuestions.downloadHandler.text);
                        string csvText = "";
                        foreach (QuestionJSON question in questionList.questionList)
                        {
                            csvText += question.question + "$," + question.options[0] + "$," + question.options[1] + "$," + question.options[2] + "$," + question.options[3] + "$," + question.answer + "$," + question.explanation + "\n";
                        }
                        File.WriteAllText("./" + topic + "CSV.csv", csvText);
                    }
                    else
                    {
                        print("error getting questions");
                    }
                }

                UnityWebRequest requestSaves = UnityWebRequest.Get("http://" + MainMenu.serverIp + ":5000/api/Users/GetState");
                requestSaves.SetRequestHeader("authorization", "Bearer " + token);
                yield return requestSaves.SendWebRequest();
                if (requestSaves.result == UnityWebRequest.Result.Success)
                {
                    SaveInfoJSON saveInfo = JsonUtility.FromJson<SaveInfoJSON>(requestSaves.downloadHandler.text);
                    world = saveInfo.world;
                    task = saveInfo.task;
                    state = saveInfo.state;
                }
                else
                {
                    print("error getting saves");
                }

                canvas.transform.GetComponent<Image>().sprite = Resources.Load("MainMenu", typeof(Sprite)) as Sprite;
                mainMenu.SetActive(true); 
                loginPage.SetActive(false);
            }
        }
    }

    public void login()
    {
        StartCoroutine(loginRequest());
    }

    public void backToFirstMenu() {
        canvas.transform.GetComponent<Image>().sprite = Resources.Load("FirstMenu", typeof(Sprite)) as Sprite;
          firstMenu.SetActive(true); 
          loginPage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
