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

    public Transform cameraTargetPosition;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private bool isTaskActive = false;

    public GameObject arrow;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            arrow.SetActive(false);
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isTaskActive = true;
            originalCameraPosition = Camera.main.transform.position;
            originalCameraRotation = Camera.main.transform.rotation;
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

            // המתנה עד שהקרוסלה תסיים את הסיבוב
            yield return new WaitUntil(() => !swingScript.isCarouselRunning);

            Camera.main.transform.position = originalCameraPosition;
            Camera.main.transform.rotation = originalCameraRotation;
            arrow.SetActive(true);
            isTaskActive = false;
        }
        else
        {
            if (ErrSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(ErrSound);
            }
            inputField.text = ""; // איפוס שדה הקלט במקרה של שגיאה
        }
    }




    // Update is called once per frame
    void Update()
    {
        if (isTaskActive)
        {
            Camera.main.transform.position = cameraTargetPosition.position;
            Camera.main.transform.rotation = cameraTargetPosition.rotation;

        }
    }
}
