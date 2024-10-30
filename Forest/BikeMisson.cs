using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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


    private AudioSource audioSource;
    void Start()
    {
        // אתחול ה-AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Firstcanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
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

                    audioSource.clip = WrongSound;
                    audioSource.time = 1.0f;
                    audioSource.Play();
                }

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
                    }
                    canvas.SetActive(false);
                    isMoving = false;
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
}
