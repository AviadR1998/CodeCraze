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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void ButtonInfoClickGame()
    {
        string answer1 = inputField1.text.ToUpper();
        string answer2 = inputField2.text.ToUpper();
        string answer3 = inputField3.text.ToUpper();
        bool isCorrect1 = (answer1 == "AND");
        bool isCorrect2 = (answer2 == "NOT");
        bool isCorrect3 = (answer3 == "OR");

        if (answer1 == "AND" && answer2 == "NOT" && answer3 == "OR")
        {

            audioSource.clip = correctClip;
            audioSource.Play();
            FindObjectOfType<FirstPersonController>().transform.position = originalPlayerPosition;
            FindObjectOfType<FirstPersonController>().transform.rotation = originalPlayerRotation;
            FindObjectOfType<FirstPersonController>().playerCamera.transform.position = originalCameraPosition;
            FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = originalCameraRotation;
            FindObjectOfType<FirstPersonController>().cameraCanMove = true;
            FindObjectOfType<FirstPersonController>().playerCanMove = true;
            FindObjectOfType<FirstPersonController>().enableHeadBob = true;
            StartCoroutine(WaitBeforeDeactivatingTask());
            campfire.SetActive(true);
            canvasGame.SetActive(false);
            CompleteTask();
            Invoke("StopFireAudio", stopAfterSeconds); 
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            SetInputFieldColor(inputField1, Color.white);
            SetInputFieldColor(inputField2, Color.white);
            SetInputFieldColor(inputField3, Color.white);
            arrow.SetActive(true);
        }
        else
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
        fireAudio.Stop();  // הפסק את האודיו
    }
    // Start is called before the first frame update
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
        yield return new WaitForSeconds(15);
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
