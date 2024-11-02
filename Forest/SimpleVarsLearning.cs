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

    public Transform cameraTargetPosition;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private bool isTaskActive = false;
    public GameObject arrow;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = backgroundMusicObject.GetComponent<AudioSource>();

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            arrow.SetActive(false);
            audioSource.Stop();
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isTaskActive = true;
            originalCameraPosition = Camera.main.transform.position;
            originalCameraRotation = Camera.main.transform.rotation;
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
        Camera.main.transform.position = originalCameraPosition;
        Camera.main.transform.rotation = originalCameraRotation;
        isTaskActive = false;
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
                                    if (FinishSound != null && audioSource != null) // בדיקה אם יש אודיו קליפ ו-AudioSource
                                    {
                                        FinishSound.Play();
                                        arrow.SetActive(true);

                                    }
                                    expressionText.text = "";
                                    yield break;
                                }
                                continue;
                            }
                        }
                    }
                }
                yield return null; // ממתין לפריים הבא
            }
        }

    }



    void ShowNextExpression()
    {
        if (!expressionText.gameObject.activeSelf)
        {
            expressionText.gameObject.SetActive(true); // הפעלת האובייקט של הטקסט
        }
        expressionText.text = expressions[currentExpressionIndex];
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
