using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MissionInputOutput : MonoBehaviour
{
    public GameObject canvas;
    public TMP_InputField inputField;
    public SwingMovement swingScript;
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
    public GameObject canvasE;
    private const int fiveSeconds = 5;
    private const int fourSeconds = 4;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isTaskActive && !NotSimultTasks.someMission)
        {
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
        }
    }

    public void ButtonSwingClick()
    {
        StartCoroutine(ButtonSwingClickCoroutine());
    }

    private IEnumerator ButtonSwingClickCoroutine()
    {
        //Number of swings are correct.
        if (int.TryParse(inputField.text, out int numberOfRotations) && numberOfRotations >= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
            inputField.image.color = Color.white;
            inputField.text = "";
            //Move swing.
            swingScript.StartCarousel(numberOfRotations);
            canvasE.SetActive(false);
            //Wait until swing finish to move.
            yield return new WaitUntil(() => !swingScript.isCarouselRunning);

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
            arrow.SetActive(true);
            //Finish mission canvas.
            finishMission.GetComponent<SoundEffects>().PlaySoundClip();
            finishMission.SetActive(true);
            CompleteTask();
            //Update state.
            if (TaskManager.currentTaskIndex <= 0)
            {
                PauseMenu.updateSave("Forest", "Sign", 0);
            }
        }
        else
        {
            //Not a valid number.
            if (ErrSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(ErrSound);
            }
            inputField.image.color = Color.red;
            inputField.text = "";
        }
    }

    void Update()
    {
    }

    private IEnumerator WaitBeforeDeactivatingTask()
    {
        yield return new WaitForSeconds(fourSeconds);
        isTaskActive = false;
    }

    public void CompleteTask()
    {
        NotSimultTasks.someMission = false;
        BackgroundMusic.Play();
        if (!isTaskCompletedOnce && TaskManager.currentTaskIndex == 0)
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
