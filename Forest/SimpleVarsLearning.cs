using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using TreeEditor;


public class SimpleVarsLearning : MonoBehaviour
{
    public GameObject canvas;
    public GameObject canvaGameExplain;
    public TMP_Text expressionText; public string[] expressions = { "4", "Hello", "'A'", "3.14", "'c'", "5", "Apple", "3.1", "True", "False" };
    public string[] correctBoxes = { "Int", "String", "Char", "Float", "Char", "Int", "String", "Float", "Bool", "Bool" };
    private int currentExpressionIndex = 0;
    public GameObject player;
    public bool flagGame;
    public AudioSource correctSound;
    public GameObject backgroundMusicObject;
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
    public GameObject trig;
    private bool canCheck = true;
    private GameObject lastTouchedBox = null;

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
            audioSource.Stop();
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void ButtonInfoClick()
    {
        canvas.SetActive(false);
        canvaGameExplain.SetActive(true);
    }

    public void ButtonGameExplain()
    {
        canvaGameExplain.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //PLAYER get back to the original place.
        FindObjectOfType<FirstPersonController>().transform.position = originalPlayerPosition;
        FindObjectOfType<FirstPersonController>().transform.rotation = originalPlayerRotation;
        //CAMERA get back to the original place.
        FindObjectOfType<FirstPersonController>().playerCamera.transform.position = originalCameraPosition;
        FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = originalCameraRotation;
        FindObjectOfType<FirstPersonController>().cameraCanMove = true;
        FindObjectOfType<FirstPersonController>().playerCanMove = true;
        FindObjectOfType<FirstPersonController>().enableHeadBob = true;
        ShowNextExpression();
        //Game loop.
        StartCoroutine(GameLoop());
    }


    IEnumerator GameLoop()
    {
        //Need to track player movments.
        FirstPersonController playerController = player.GetComponent<FirstPersonController>();
        if (playerController != null)
        {
            //Game didn't finish yet.
            while (currentExpressionIndex < expressions.Length)
            {
                //Can check the answer.
                if (canCheck)
                {
                    bool isTouching = playerController.IsTouchingBox();
                    //Check if playet touch one of the boxes.
                    if (isTouching)
                    {
                        //Get the tag of the box the player is touching.
                        GameObject touchedBox = playerController.GetTouchedBox();
                        //Ensure the player is touch a new box.
                        if (touchedBox != lastTouchedBox)
                        {
                            //Update the last touched box.
                            lastTouchedBox = touchedBox;
                            //Playet touch the right Box.
                            if (touchedBox.CompareTag(correctBoxes[currentExpressionIndex]))
                            {
                                correctSound.Play();
                                //Update next expression index.
                                currentExpressionIndex++;
                                canCheck = false;
                                //Still have questions.
                                if (currentExpressionIndex < expressions.Length)
                                {
                                    //Display the next expression.
                                    ShowNextExpression();
                                    canCheck = true;
                                }
                                //Finish game.
                                else
                                {
                                    GameObject myPlayer = GameObject.FindGameObjectWithTag("Player");
                                    //Before asking practice queations move player to different position.
                                    myPlayer.transform.position = trig.transform.position;
                                    StartCoroutine(WaitBeforeDeactivatingTask());
                                    StartCoroutine(HandleQuestionsAndCompleteTask());
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
        audioSource.Play();
        // Cursor.lockState = CursorLockMode.Confined;
        // Cursor.visible = true;
        if (!isTaskCompletedOnce && TaskManager.currentTaskIndex == 3)
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
        //Open practice queations.
        FreeQueBox.ask = true;
        FreeQueBox.keepWhile = true;
        //Finish all practice qustions.
        yield return new WaitUntil(() => !FreeQueBox.keepWhile);
        CompleteTask();
        //Finish mission sound + canvas.
        finishMission.GetComponent<SoundEffects>().PlaySoundClip();
        finishMission.SetActive(true);
        //Update state.
        PauseMenu.updateSave("Forest", "Fish", 0);
    }
}
