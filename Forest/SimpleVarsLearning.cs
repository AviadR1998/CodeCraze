using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class SimpleVarsLearning : MonoBehaviour
{
    public GameObject canvas;
    public GameObject canvaGameExplain;
    public TMP_Text expressionText; public string[] expressions = { "4", "Hello", "'A'", "3.14", "'c'", "5", "Apple", "3.1", "True", "False" };
    public string[] correctBoxes = { "Int", "String", "Char", "Float", "Char", "Int", "String", "Float", "Bool", "Bool" };

    private int currentExpressionIndex = 0;
    public GameObject player; //player data
    public bool flagGame;
    public AudioSource correctSound; // Sound for correct answer.
    public GameObject backgroundMusicObject; // כאן תגרור את ה-Empty Object של המוזיקה
    private AudioSource audioSource;

    public AudioSource FinishSound;
    public Transform destination;
    private Vector3 originalPlayerPosition;
    private Quaternion originalPlayerRotation;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isTaskActive = false;
    public GameObject arrow;

    public TaskManager taskManager;
    private bool isTaskCompletedOnce = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = backgroundMusicObject.GetComponent<AudioSource>();

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
            audioSource.Stop();
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ButtonInfoClick()
    {
        canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        canvaGameExplain.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }


    public void ButtonGameExplain()
    {
        canvaGameExplain.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        FindObjectOfType<FirstPersonController>().transform.position = originalPlayerPosition;
        FindObjectOfType<FirstPersonController>().transform.rotation = originalPlayerRotation;
        FindObjectOfType<FirstPersonController>().playerCamera.transform.position = originalCameraPosition;
        FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = originalCameraRotation;
        FindObjectOfType<FirstPersonController>().cameraCanMove = true;
        FindObjectOfType<FirstPersonController>().playerCanMove = true;
        FindObjectOfType<FirstPersonController>().enableHeadBob = true;
        ShowNextExpression();
        //game loop
        StartCoroutine(GameLoop());
        audioSource.Play();
    }
    private bool canCheck = true;
    private GameObject lastTouchedBox = null;

    IEnumerator GameLoop()
    {
        FirstPersonController playerController = player.GetComponent<FirstPersonController>();
        if (playerController != null)
        {
            while (currentExpressionIndex < expressions.Length)
            {
                if (canCheck)
                {
                    bool isTouching = playerController.IsTouchingBox();
                    if (isTouching)
                    {
                        GameObject touchedBox = playerController.GetTouchedBox();
                        if (touchedBox != lastTouchedBox)
                        {
                            lastTouchedBox = touchedBox;
                            if (touchedBox.CompareTag(correctBoxes[currentExpressionIndex]))
                            {
                                correctSound.Play();
                                currentExpressionIndex++;
                                canCheck = false;
                                if (currentExpressionIndex < expressions.Length)
                                {
                                    ShowNextExpression();
                                    canCheck = true;
                                }
                                else
                                {
                                    if (FinishSound != null && audioSource != null)
                                    {
                                        FinishSound.Play();
                                        arrow.SetActive(true);
                                        CompleteTask();
                                        StartCoroutine(WaitBeforeDeactivatingTask());


                                    }
                                    expressionText.text = "";
                                    yield break;
                                }
                                continue;
                            }
                        }
                    }
                }
                yield return null;
            }
        }

    }



    void ShowNextExpression()
    {
        if (!expressionText.gameObject.activeSelf)
        {
            expressionText.gameObject.SetActive(true);
        }
        expressionText.text = expressions[currentExpressionIndex];
    }

    private IEnumerator WaitBeforeDeactivatingTask()
    {
        yield return new WaitForSeconds(120);
        isTaskActive = false;
    }

    // Update is called once per frame
    void Update()
    {

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
