using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireMission : MonoBehaviour
{
    public GameObject canvas;
    public GameObject canvasGame;
    public GameObject campfire;
    public TMP_InputField inputField1;
    public TMP_InputField inputField2;
    public TMP_InputField inputField3;
    public AudioClip correctClip;
    public AudioClip WrongClip;
    private AudioSource audioSource;
    public AudioSource fireAudio;
    public float stopAfterSeconds = 15f;
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
    public GameObject endAllCanvas;
    public GameObject CastleBox;
    public AudioSource BackgroundMusic;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && !isTaskActive)
        {
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
            campfire.SetActive(false);
            isTaskActive = true;
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inputField1.text = "";
            inputField2.text = "";
            inputField3.text = "";
        }
    }

    void Update()
    {

    }

    public void ButtonInfoClick()
    {
        canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        canvasGame.SetActive(true);
        // Cursor.lockState = CursorLockMode.Confined;
        // Cursor.visible = true;
    }

    public void ButtonInfoClickGame()
    {
        string answer1 = inputField1.text.ToUpper();
        string answer2 = inputField2.text.ToUpper();
        string answer3 = inputField3.text.ToUpper();
        bool isCorrect1 = (answer1 == "AND");
        bool isCorrect2 = (answer2 == "NOT");
        bool isCorrect3 = (answer3 == "OR");
        //Player correct.
        if (answer1 == "AND" && answer2 == "NOT" && answer3 == "OR")
        {
            //Correct sound.
            audioSource.clip = correctClip;
            audioSource.Play();
            StartCoroutine(WaitBeforeDeactivatingTask());
            //Light fire.
            campfire.SetActive(true);
            canvasGame.SetActive(false);
            StartCoroutine(HandleQuestionsAndCompleteTask());
            //Make fire sound for only 15 seconds.
            Invoke("StopFireAudio", stopAfterSeconds);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            SetInputFieldColor(inputField1, Color.white);
            SetInputFieldColor(inputField2, Color.white);
            SetInputFieldColor(inputField3, Color.white);
            arrow.SetActive(true);
        }
        else
        //Player didn't answer the right answer.
        {
            audioSource.clip = WrongClip;
            audioSource.Play();
            SetInputFieldColor(inputField1, isCorrect1 ? Color.white : Color.red);
            SetInputFieldColor(inputField2, isCorrect2 ? Color.white : Color.red);
            SetInputFieldColor(inputField3, isCorrect3 ? Color.white : Color.red);
        }
    }

    void StopFireAudio()
    {
        fireAudio.Stop();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void SetInputFieldColor(TMP_InputField inputField, Color color)
    {
        inputField.image.color = color;
    }
    private IEnumerator WaitBeforeDeactivatingTask()
    {
        //wait 15 seconds and let user do the mission again if he want to.
        yield return new WaitForSeconds(15);
        isTaskActive = false;
    }

    public void CompleteTask()
    {
        BackgroundMusic.Play();
        if (!isTaskCompletedOnce && TaskManager.currentTaskIndex == 6)
        {
            isTaskCompletedOnce = true;
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

    private IEnumerator HandleQuestionsAndCompleteTask()
    {
        //Open practice questions.
        FreeQueFire.ask = true;
        FreeQueFire.keepWhile = true;
        //Waiy until player finish all the practice questions and then continue.
        yield return new WaitUntil(() => !FreeQueFire.keepWhile);
        //PLAYER get back to the original place.
        FindObjectOfType<FirstPersonController>().transform.position = originalPlayerPosition;
        FindObjectOfType<FirstPersonController>().transform.rotation = originalPlayerRotation;
        //CAMERA get back to the original place.
        FindObjectOfType<FirstPersonController>().playerCamera.transform.position = originalCameraPosition;
        FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = originalCameraRotation;
        FindObjectOfType<FirstPersonController>().cameraCanMove = true;
        FindObjectOfType<FirstPersonController>().playerCanMove = true;
        FindObjectOfType<FirstPersonController>().enableHeadBob = true;
        //Finish task sound.
        finishMission.GetComponent<SoundEffects>().PlaySoundClip();
        finishMission.SetActive(true);
        //Updae state.
        PauseMenu.updateSave("Forest", "Finish", 0);
        CastleBox.SetActive(true);
        CompleteTask();
        //Player finish all tasks we want to tell him he can move to castle.
        StartCoroutine(ShowEndCanvasWithDelay());
    }

    private IEnumerator ShowEndCanvasWithDelay()
    {
        //Wait 6 seconds until show canvas.
        yield return new WaitForSeconds(6);
        endAllCanvas.SetActive(true);
        //End canvas after 10 seconds.
        yield return new WaitForSeconds(10);
        endAllCanvas.SetActive(false);
    }
}
