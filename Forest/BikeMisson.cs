using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;


public class BikeMission : MonoBehaviour
{
    public GameObject Firstcanvas;
    public GameObject canvas;
    public GameObject[] dots;
    private int currentDotIndex = 0;
    public float speed = 2.2f;
    public TMP_InputField answerInput;
    public TextMeshProUGUI questionText;
    private int correctAnswer;
    private int questionCounter = 0;
    public int totalQuestions = 10;
    private bool isMoving = false;
    private List<int> operations;

    public AudioClip StartSound;

    public AudioClip WinSound;

    public AudioClip WrongSound;
    public AudioClip SuccSound;
    public Transform destination;
    private Vector3 originalPlayerPosition;
    private Quaternion originalPlayerRotation;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isTaskActive = false;
    public GameObject arrow;
    public TaskManager taskManager;
    private bool isTaskCompletedOnce = false;



    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            Firstcanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    public void ButtonBikeClick()
    {
        Firstcanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        canvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        FindObjectOfType<FirstPersonController>().transform.position = originalPlayerPosition;
        FindObjectOfType<FirstPersonController>().transform.rotation = originalPlayerRotation;
        FindObjectOfType<FirstPersonController>().playerCamera.transform.position = originalCameraPosition;
        FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = originalCameraRotation;
        FindObjectOfType<FirstPersonController>().cameraCanMove = true;
        FindObjectOfType<FirstPersonController>().playerCanMove = true;
        FindObjectOfType<FirstPersonController>().enableHeadBob = true;
        // Debug.Log("----------------------------------------------------------------");
        // Debug.Log("Player Position: " + FindObjectOfType<FirstPersonController>().transform.position);
        // Debug.Log("Player Rotation: " + FindObjectOfType<FirstPersonController>().transform.rotation.eulerAngles);
        // Debug.Log("Camera Position: " + FindObjectOfType<FirstPersonController>().playerCamera.transform.position);
        // Debug.Log("Camera Rotation: " + FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation.eulerAngles);

        GameStarted();
        if (StartSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(StartSound);
        }

    }


    private void GameStarted()
    {
        questionCounter = 0;
        InitializeOperations();
        GenerateQuestion();
    }


    private void InitializeOperations()
    {
        operations = new List<int> { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 }; // 0 = +, 1 = -, 2 = /, 3 = %, 4 = *
    }


    public void CheckAnswer()
    {
        int playerAnswer;
        if (int.TryParse(answerInput.text, out playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {

                if (SuccSound != null && audioSource != null)
                {

                    audioSource.PlayOneShot(SuccSound);
                }
                questionCounter++;
                answerInput.text = "";

                if (questionCounter < totalQuestions)
                {
                    isMoving = true;
                    canvas.SetActive(false);
                }
                else
                {
                    canvas.SetActive(false);
                    isMoving = true;
                }
            }
            else
            {
                if (WrongSound != null && audioSource != null)
                {

                    audioSource.PlayOneShot(WrongSound);

                }

            }
        }
        else
        {
            if (WrongSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(WrongSound);
            }
        }
    }


    void GenerateQuestion()
    {
        if (operations.Count == 0) return;

        int num1 = Random.Range(1, 13);
        int num2 = Random.Range(1, 13);
        int randomIndex = Random.Range(0, operations.Count);
        int operation = operations[randomIndex];
        operations.RemoveAt(randomIndex);

        switch (operation)
        {
            case 0:
                questionText.text = num1 + " + " + num2 + " = ?";
                correctAnswer = num1 + num2;
                break;
            case 1:
                questionText.text = num1 + " - " + num2 + " = ?";
                correctAnswer = num1 - num2;
                break;
            case 2:
                num1 = Random.Range(2, 13);
                num2 = Random.Range(1, num1);
                while (num1 % num2 != 0)
                {
                    num2 = Random.Range(1, num1);
                }
                questionText.text = num1 + " / " + num2 + " = ?";
                correctAnswer = num1 / num2;
                break;
            case 3:
                questionText.text = num1 + " % " + num2 + " = ?";
                correctAnswer = num1 % num2;
                break;
            case 4:
                questionText.text = num1 + " * " + num2 + " = ?";
                correctAnswer = num1 * num2;
                break;
        }


    }


    void MoveTowardsNextPoint()
    {
        if (isMoving)
        {

            if (Vector3.Distance(transform.position, dots[currentDotIndex].transform.position) < 0.1f)
            {
                currentDotIndex++;
                if (currentDotIndex >= dots.Length)
                {
                    currentDotIndex = 0;
                }


                if (questionCounter >= totalQuestions - 1)
                {
                    if (WinSound != null && audioSource != null)
                    {

                        audioSource.PlayOneShot(WinSound);
                        CompleteTask();
                        StartCoroutine(WaitBeforeDeactivatingTask());


                    }
                    canvas.SetActive(false);
                    arrow.SetActive(true);
                    isMoving = false;
                    ResetBikePosition();
                    return;
                }


                if (questionCounter < totalQuestions - 1)
                {
                    isMoving = false;
                    canvas.SetActive(true);
                    GenerateQuestion();
                }
            }


            transform.position = Vector3.MoveTowards(transform.position, dots[currentDotIndex].transform.position, Time.deltaTime * speed);


            Vector3 direction = (dots[currentDotIndex].transform.position - transform.position).normalized;


            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                targetRotation *= Quaternion.Euler(0, 90, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            }
        }
    }

    void Update()
    {
        MoveTowardsNextPoint();
    }
    void ResetBikePosition()
    {
        transform.position = new Vector3(37.59f, 4.62f, 46.37f);
        transform.rotation = Quaternion.Euler(0, 35.581f, 0);
    }

    private IEnumerator WaitBeforeDeactivatingTask()
    {
        yield return new WaitForSeconds(5);
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
