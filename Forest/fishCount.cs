using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using TMPro;
using System;

public class fishCount : MonoBehaviour
{
    public GameObject arrow;
    public GameObject target;
    public GameObject playerCamera;
    public GameObject canvas;
    public GameObject endcanvas;
    public GameObject animationCanvas;
    public TMP_Text explainWorlds;
    public TMP_Text explainCode;
    public TMP_Text animationCode;
    bool flag = false;
    public GameObject[] fish;
    int i = 0;
    public AudioClip BloopSound;
    private AudioSource audioSource;
    public GameObject arrow2;
    public Transform destination;
    private Vector3 originalPlayerPosition;
    private Quaternion originalPlayerRotation;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isTaskActive = false;
    public GameObject finishMission;
    public TaskManager taskManager;
    private bool isTaskCompletedOnce = false;
    public AudioSource BackgroundMusic;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
            arrow2.SetActive(false);
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            explainWorlds.text = "Meet Count!😊 Count is a special number that loves to keep track of things.\n Imagine Count starts at 0. So, we say: count = 0;\n " +
            "This means Count is holding the number 0 right now.\n Every time we write count++, Count gets one more.\n It's like giving Count an extra toy to hold! ";

            explainCode.text = "int main() {\n \tint fish = 1;  // Starting with 1 fish in the lake.\n \tfish++;  // Now fish is 2 so we have two fish in the lake..\n \tfish++; // Now fish is 3 so we have three fish in the lake..\n \treturn 0;.\n";

            //Remove all fishs from river.
            i = 0;
            foreach (GameObject f in fish)
            {
                f.SetActive(false); //remove fish
            }
        }

    }
    public void ButtonInfoClick()
    {
        canvas.SetActive(false);
        flag = true;
        animationCode.text = "Press + and look at the fish in the lake.\nEvery time you hear the sound of water, you'll see a new fish in the river";
        animationCanvas.SetActive(true);
    }

    void Update()
    {
        if (flag == true)
        {
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                if (i <= 3)
                {
                    //Make sounds when new fish is in the river.
                    if (BloopSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(BloopSound);
                    }
                    fish[i].SetActive(true);
                    i++;
                }
            }
        }
        //No more fishs to put.
        if (i == 4)
        {
            animationCanvas.SetActive(false);
            endcanvas.SetActive(true);
            if (Input.GetKeyDown(KeyCode.C))
            {
                endcanvas.SetActive(false);
                i++;
                StartCoroutine(HandleQuestionsAndCompleteTask());
            }
        }
    }
    private IEnumerator WaitBeforeDeactivatingTask()
    {
        yield return new WaitForSeconds(5);
        arrow2.SetActive(true);
        isTaskActive = false;
    }

    public void CompleteTask()
    {
        BackgroundMusic.Play();
        if (!isTaskCompletedOnce && TaskManager.currentTaskIndex == 4)
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
    private IEnumerator HandleQuestionsAndCompleteTask()
    {
        //Start practice questions.
        FreeQueFish.ask = true;
        FreeQueFish.keepWhile = true;
        //Wait until player finish all pratice questions.
        yield return new WaitUntil(() => !FreeQueFish.keepWhile);

        //PLAYER get back to the original place.
        FindObjectOfType<FirstPersonController>().transform.position = originalPlayerPosition;
        FindObjectOfType<FirstPersonController>().transform.rotation = originalPlayerRotation;
        //CAMERA get back to the original place.
        FindObjectOfType<FirstPersonController>().playerCamera.transform.position = originalCameraPosition;
        FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = originalCameraRotation;
        FindObjectOfType<FirstPersonController>().cameraCanMove = true;
        FindObjectOfType<FirstPersonController>().playerCanMove = true;
        FindObjectOfType<FirstPersonController>().enableHeadBob = true;
        //Finish mission sounds.
        finishMission.GetComponent<SoundEffects>().PlaySoundClip();
        finishMission.SetActive(true);
        CompleteTask();
        //Save state.
        PauseMenu.updateSave("Forest", "Bike", 0);
        //Wait 5 seconds and let the player do the mission again if he want to.
        StartCoroutine(WaitBeforeDeactivatingTask());
    }
}
