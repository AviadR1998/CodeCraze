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
    public TMP_Text explainWorlds;
    public TMP_Text inputConsist;
    public TMP_InputField inputUser;
    public GameObject[] coffee;
    public GameObject canvasError;
    public AudioClip AddCoffeeSound;
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
    public GameObject finishMission;
    public AudioSource BackgroundMusic;
    private const int fiveSeconds = 5;
    private const int fourSeconds = 4;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && !isTaskActive && !NotSimultTasks.someMission)
        {
            PauseMenu.canPause = false;
            NotSimultTasks.someMission = true;
            BackgroundMusic.Pause();
            isTaskActive = true;

            //Save PLAYER position and rotation.
            originalPlayerPosition = FindObjectOfType<FirstPersonController>().transform.position;
            originalPlayerRotation = FindObjectOfType<FirstPersonController>().transform.rotation;
            //Save CAMERA position and rotation.
            originalCameraPosition = FindObjectOfType<FirstPersonController>().playerCamera.transform.position;
            originalCameraRotation = FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation;
            //CHANGE position and rotation.
            FindObjectOfType<FirstPersonController>().playerCamera.transform.position = destination.position;
            FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = destination.rotation;
            FindObjectOfType<FirstPersonController>().cameraCanMove = false;
            FindObjectOfType<FirstPersonController>().playerCanMove = false;
            FindObjectOfType<FirstPersonController>().enableHeadBob = false;

            arrow.SetActive(false);
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            explainWorlds.text = "Let’s Make Coffee Cups with int! \n" +
    "Hey there, young programmer! We’ve already learned what an `int` is, " +
    "but now it’s time to dive a little deeper and see how we can use it in a fun way in our game.\n" +
    "Today, we’re going to use an `int` to make coffee cups appear at the café!\n\n" +

    "What is an int?\n" +
    "An `int` is a special type of number in programming that doesn’t have any decimal points. " +
    "It means whole numbers, like:\n" +
    "- `1`, `2`, `3`, or even negative numbers like `-4`.\n" +
    "In programming, we’d write it like this:\n" +
    "int coffeeCups = 3; // This means I have 3 coffee cups.\n\n" +

    "Why Use an int?\n" +
    "In our game, we want to decide how many coffee cups to show at the café. " +
    "Since we can only have whole coffee cups (not half a cup or 2.5 cups), an `int` is the perfect tool for this job.\n";
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
        finishMission.GetComponent<SoundEffects>().PlaySoundClip();
        finishMission.SetActive(true);
        PauseMenu.canPause = true;
        //Save state.
        if (TaskManager.currentTaskIndex <= 2)
        {
            PauseMenu.updateSave("Forest", "Box", 0);
        }
        //PLAYER get back to the original place.
        FindObjectOfType<FirstPersonController>().transform.position = originalPlayerPosition;
        FindObjectOfType<FirstPersonController>().transform.rotation = originalPlayerRotation;
        //CAMERA get back to the original place.
        FindObjectOfType<FirstPersonController>().playerCamera.transform.position = originalCameraPosition;
        FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = originalCameraRotation;
        FindObjectOfType<FirstPersonController>().cameraCanMove = true;
        FindObjectOfType<FirstPersonController>().playerCanMove = true;
        FindObjectOfType<FirstPersonController>().enableHeadBob = true;

        //Wait 4 seconds and let the player do the task again if he want to.
        StartCoroutine(WaitBeforeDeactivatingTask());
        inputUser.text = "";
        canvasCoffee.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        arrow.SetActive(true);
        CompleteTask();
    }

    public void RunButtonClick()
    {
        string userInput = inputUser.text;
        //Empty is like 0.
        if (string.IsNullOrEmpty(userInput))
        {
            userInput = 0.ToString(); ;
        }

        //Check if input is between 0 to 4.
        int num;
        //Not correct number- if number is noy less then 0 or more then 4.
        //Use 'out' to store the converted integer value if TryParse succeeds
        if (!int.TryParse(userInput, out num) || num < 0 || num > 4)
        {
            if (ErrSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(ErrSound);
            }
            //Raise and error and remove it ater 3 seconds.
            canvasError.SetActive(true);
            StartCoroutine(HideCanvasAfterTime(3f));
            return;
        }
        else
        {
            //Correct number.
            if (AddCoffeeSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(AddCoffeeSound);
            }
            //Show cup of coffee and remove if needed.
            for (int i = 3; i >= int.Parse(userInput); i--)
            {
                coffee[i].SetActive(false);
            }
            for (int i = 0; i < int.Parse(userInput); i++)
            {
                coffee[i].SetActive(true);
            }
        }
    }

    void Update()
    {

    }

    private IEnumerator HideCanvasAfterTime(float delay)
    {
        //Close error mesage after "3 delay" seconds.
        yield return new WaitForSeconds(delay);
        canvasError.SetActive(false);
    }

    private IEnumerator WaitBeforeDeactivatingTask()
    {
        yield return new WaitForSeconds(fourSeconds);
        isTaskActive = false;
    }

    public void CompleteTask()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        NotSimultTasks.someMission = false;
        BackgroundMusic.Play();
        if (!isTaskCompletedOnce && TaskManager.currentTaskIndex == 2)
        {
            isTaskCompletedOnce = true;

            if (taskManager != null)
            {
                StartCoroutine(ActivateNextTaskWithDelay());
            }
            else
            {
                Debug.LogError("TaskManager is not assigned in the Inspector!");
            }
        }
    }
    private IEnumerator ActivateNextTaskWithDelay()
    {
        yield return new WaitForSeconds(fiveSeconds);
        taskManager.ActivateNextTask();
    }
}
