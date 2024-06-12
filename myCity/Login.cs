using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.WSA;

public class Login : MonoBehaviour
{
    public static string token;
    public GameObject mainMenu;
    public GameObject firstMenu;
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public GameObject errorMessage;
    public GameObject canvas;
    public GameObject loginPage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        token = "";
        usernameField.text="";
        passwordField.text="";
        errorMessage.SetActive(false);
    }

    public IEnumerator loginRequest()
    {
        string jsonUser = "{\"username\": " + usernameField.text + ", \"password\": " + passwordField.text + "}", response;
        /*using (var webRequest = new UnityWebRequest("http://localhost:5000/api/Tokens/", "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonUser));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.certificateHandler = new ForceAcceptAllCertificates();
            await webRequest.SendWebRequest();
            int responseCode = (HttpStatus)webRequest.responseCode;
        }*/
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:5000/api/Tokens/", form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                errorMessage.SetActive(true);
            else
            {
                token = request.downloadHandler.text;
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
