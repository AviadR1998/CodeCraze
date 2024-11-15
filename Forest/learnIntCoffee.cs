using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using TMPro;
using System;
using UnityEngine.UI;
public class learnIntCoffee : MonoBehaviour
{
    public GameObject canvas;
    public GameObject canvasCoffee;
    //public GameObject canvasGotIt;
    public TMP_Text explainWorlds;
    public TMP_Text inputConsist;
    public TMP_InputField inputUser;
    public GameObject[] coffee;
    public GameObject canvasError;
    public AudioClip AddCoffeeSound;
    public AudioClip SuccSound;
    public AudioClip ErrSound;
    private AudioSource audioSource;
    public Transform destination;
    private Vector3 originalPlayerPosition;
    private Quaternion originalPlayerRotation;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isTaskActive = false;
    public GameObject arrow;
    public TaskManager taskManager;
    private bool isTaskCompletedOnce = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && !isTaskActive)
        {
            isTaskActive = true;
            //Save Player data
            originalPlayerPosition = FindObjectOfType<FirstPersonController>().transform.position;
            originalPlayerRotation = FindObjectOfType<FirstPersonController>().transform.rotation;
            //Save Camera data
            originalCameraPosition = FindObjectOfType<FirstPersonController>().playerCamera.transform.position;
            originalCameraRotation = FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation;
            //change rotation + position
            FindObjectOfType<FirstPersonController>().playerCamera.transform.position = destination.position;
            FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = destination.rotation;
            FindObjectOfType<FirstPersonController>().cameraCanMove = false;
            FindObjectOfType<FirstPersonController>().playerCanMove = false;
            FindObjectOfType<FirstPersonController>().enableHeadBob = false;
            arrow.SetActive(false);
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            explainWorlds.text = "Let's Make Coffee Cups with int!😊\n" + "Hey there, young programmer! Today, we're going to learn how to use something called an integer or int in our game to make coffee cups appear at the café.\n\n" + "What is an int?\n" + "An int is a special type of number in programming that doesn't have any decimal points. It means whole numbers like 1, 2, 3, -4 (also negative numbers as u can see) and so on.\n\n" +
            "And in a programming way, we would write it as: int coffeeCups = 3; which means i have 3 coffee cups " +
            "Why Use an int?\n" + "In our game, we want to decide how many coffee cups to show at the café. Since we can only have whole coffee cups (not half a cup!), an int is perfect for this job.\n";
        }
    }


    public void ButtonInfoClick()
    {
        canvas.SetActive(false);
        canvasCoffee.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        inputConsist.text = "int CupOfCoffee = 0;";
    }

    public void ButtonExit()
    {
        if (SuccSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(SuccSound);
        }
        FindObjectOfType<FirstPersonController>().transform.position = originalPlayerPosition;
        FindObjectOfType<FirstPersonController>().transform.rotation = originalPlayerRotation;
        FindObjectOfType<FirstPersonController>().playerCamera.transform.position = originalCameraPosition;
        FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = originalCameraRotation;
        FindObjectOfType<FirstPersonController>().cameraCanMove = true;
        FindObjectOfType<FirstPersonController>().playerCanMove = true;
        FindObjectOfType<FirstPersonController>().enableHeadBob = true;
        StartCoroutine(WaitBeforeDeactivatingTask());
        inputUser.text = "";
        canvasCoffee.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        arrow.SetActive(true);
        CompleteTask();

    }


    public void RunButtonClick()
    {
        //need to check if input is between 0 to 4.
        string userInput = inputUser.text;
        //empty
        if (string.IsNullOrEmpty(userInput))
        {
            userInput = 0.ToString(); ;
        }

        int num;
        if (!int.TryParse(userInput, out num) || num < 0 || num > 4) // בדיקה אם זה מספר בין 1 ל-4
        {
            if (ErrSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(ErrSound);
            }
            canvasError.SetActive(true);
            StartCoroutine(HideCanvasAfterTime(3f));
            return;
        }

        if (AddCoffeeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(AddCoffeeSound);
        }

        for (int i = 3; i >= int.Parse(userInput); i--)
        {
            coffee[i].SetActive(false);
        }
        for (int i = 0; i < int.Parse(userInput); i++)
        {
            coffee[i].SetActive(true);
        }

    }

    void Update()
    {

    }

    private IEnumerator HideCanvasAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        canvasError.SetActive(false);
    }


    private IEnumerator WaitBeforeDeactivatingTask()
    {
        yield return new WaitForSeconds(4);
        isTaskActive = false;
    }

    public void CompleteTask()
    {
        if (!isTaskCompletedOnce)
        {
            isTaskCompletedOnce = true; // מסמן שהמשימה הושלמה

            if (taskManager != null)
            {
                taskManager.ActivateNextTask();
            }
            else
            {
                Debug.LogError("TaskManager is not assigned in the Inspector!");
            }
        }
    }

}
