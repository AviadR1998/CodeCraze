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



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isTaskActive)
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
            //remoce shakes.
            FindObjectOfType<FirstPersonController>().enableHeadBob = false;
            arrow.SetActive(false);
            arrow.SetActive(false);
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    public void ButtonSwingClick()
    {
        StartCoroutine(ButtonSwingClickCoroutine());
    }

    private IEnumerator ButtonSwingClickCoroutine()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        if (int.TryParse(inputField.text, out int numberOfRotations))
        {
            inputField.text = "";
            swingScript.StartCarousel(numberOfRotations);
            canvas.SetActive(false);
            yield return new WaitUntil(() => !swingScript.isCarouselRunning);
            FindObjectOfType<FirstPersonController>().transform.position = originalPlayerPosition;
            FindObjectOfType<FirstPersonController>().transform.rotation = originalPlayerRotation;
            FindObjectOfType<FirstPersonController>().playerCamera.transform.position = originalCameraPosition;
            FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = originalCameraRotation;
            FindObjectOfType<FirstPersonController>().cameraCanMove = true;
            FindObjectOfType<FirstPersonController>().playerCanMove = true;
            FindObjectOfType<FirstPersonController>().enableHeadBob = true;
            StartCoroutine(WaitBeforeDeactivatingTask());
            arrow.SetActive(true);
            CompleteTask();

        }
        else
        {
            if (ErrSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(ErrSound);
            }
            inputField.text = "";
        }
    }

    void Update()
    {
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
