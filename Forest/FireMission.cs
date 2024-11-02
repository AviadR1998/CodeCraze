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

    public Transform cameraTargetPosition;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private bool isTaskActive = false;
    private AudioSource audioSource;

    public AudioSource fireAudio;
    public float stopAfterSeconds = 15f;

    public GameObject arrow;




    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            arrow.SetActive(false);
            campfire.SetActive(false);
            isTaskActive = true;
            //audioSource.Stop();
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inputField1.text = "";
            inputField2.text = "";
            inputField3.text = "";
            originalCameraPosition = Camera.main.transform.position;
            originalCameraRotation = Camera.main.transform.rotation;
        }
    }

    void Update()
    {
        if (isTaskActive)
        {
            Camera.main.transform.position = cameraTargetPosition.position;
            Camera.main.transform.rotation = cameraTargetPosition.rotation;

        }

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
            Camera.main.transform.position = originalCameraPosition;
            Camera.main.transform.rotation = originalCameraRotation;
            isTaskActive = false;
            campfire.SetActive(true);
            canvasGame.SetActive(false);
            Invoke("StopFireAudio", stopAfterSeconds);  // עצור את האודיו אחרי 10 שניות
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

    // Update is called once per frame

}
