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

       public AudioSource fireAudio;  // ה-AudioSource שמחובר לאובייקט של האש
    public float stopAfterSeconds = 15f;  // הזמן לאחריו האודיו יפסיק (10 שניות)



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            //audioSource.Stop();
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
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

        if (answer1 == "AND" && answer2 == "NOT" && answer3 == "OR")
        {
            audioSource.clip = correctClip;
            audioSource.Play();
            campfire.SetActive(true);
            canvasGame.SetActive(false);
             Invoke("StopFireAudio", stopAfterSeconds);  // עצור את האודיו אחרי 10 שניות
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

        }
        else
        {
        audioSource.clip = WrongClip;
        audioSource.Play(); 

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

    // Update is called once per frame
    void Update()
    {

    }
}
