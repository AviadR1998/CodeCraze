using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public TMP_InputField userName;
    public TMP_InputField password;
    public TMP_InputField confirmPassword;
    public TMP_InputField age;
    public TMP_InputField email;
    public GameObject errorBox;

    public TMP_Text errorText;

    public GameObject firstMenu;

    public GameObject register;
    public GameObject canvas;
    
    public GameObject mainMenu;

     public GameObject login;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        userName.text="";
        password.text="";
        confirmPassword.text="";
        age.text="";
        email.text="";
        errorBox.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void buttonRegisterClick()
    {
      //checking user name.
      string userNameInput = userName.text; 
      if (userNameInput.Length == 0)
       {
        errorBox.SetActive(true);
        errorText.text=  "User name can not be empty";
        return;
       }
      for (int i = 0; i < userNameInput.Length; i++)
      {
       if (!char.IsLetterOrDigit(userNameInput[i]))
       {
        errorBox.SetActive(true);
        errorText.text=  "User name should contain letters and numbers only";
        return;
       }
      } 

       //checking password.
       string passwordInput = password.text;
       if (passwordInput.Length == 0)
       {
        errorBox.SetActive(true);
        errorText.text=  "Password can not be empty";
        return;
       } 
       if (passwordInput.Length <6)
       {
        errorBox.SetActive(true);
        errorText.text= "Password length should be at least 6 letters.";
        return;
       }  
        bool hasNumber = false;
        bool hasCapitalLetter = false;

       for (int i = 0; i < passwordInput.Length; i++)
        {
         if (char.IsDigit(passwordInput[i])) 
         {
            hasNumber= true;
         }
          if (char.IsUpper(passwordInput[i])) 
         {
            hasCapitalLetter= true;
         }
        }
        if (!hasNumber || !hasCapitalLetter )
        {
        errorBox.SetActive(true);
        errorText.text=  "Password must be at least 6 characters long with one capital lette and one number";
        return;   
        }

        //checking confirm password.
        string passwordInputConfirm = confirmPassword.text;
        if (passwordInput != passwordInputConfirm)
        {
        errorBox.SetActive(true);
        errorText.text=  "Passwords do not match";
        return;     
        }

         //checking age.
        string ageInput = age.text;
        if (ageInput.Length == 0)
        {
        errorBox.SetActive(true);
        errorText.text=  "Age can not be empty";
        return;   
        }

         for (int i = 0; i < ageInput.Length; i++)
        {
            if (!char.IsDigit(ageInput[i]))
            {
                errorBox.SetActive(true);
                errorText.text=  "Age must be a number";
                return;

            }
        }

        int number = int.Parse(ageInput);
        if (number < 0)
        {
        errorBox.SetActive(true);
        errorText.text=  "Age invalid";
        return;   
        }

        //checking email.
        string emailnput = email.text;
         if (emailnput.Length == 0)
       {
        errorBox.SetActive(true);
        errorText.text=  "Email can not be empty";
        return;
       } 
        string pattern = @"^\w+@gmail\.com$";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        if (!regex.IsMatch(emailnput))
        {
        errorBox.SetActive(true);
        errorText.text=  "Email must be in format: name@gmail.com";
        return;   
        }
        StartCoroutine(dataToServer());
    }

    

    IEnumerator dataToServer()
    {
        string response="pass";
        string uri = "http://" + MainMenu.serverIp + ":5000/api/Users/";
        WWWForm form = new WWWForm();
        form.AddField("username", userName.text);
        form.AddField("password", password.text);
        form.AddField("age", age.text);
        form.AddField("mail", email.text);
        form.AddField("world", "empty");
        form.AddField("task", "empty");
        form.AddField("state", -1);
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError){
                response = request.error;
                errorBox.SetActive(true);
                errorText.text=  "Username already exist";

            }
            else
            {
                canvas.transform.GetComponent<Image>().sprite = Resources.Load("Login", typeof(Sprite)) as Sprite;
                login.SetActive(true); 
                register.SetActive(false);
            }
        }
    }


        public void backToFirstMenu()
        {
          canvas.transform.GetComponent<Image>().sprite = Resources.Load("FirstMenu", typeof(Sprite)) as Sprite;
          firstMenu.SetActive(true); 
          register.SetActive(false);
        
        }


}
