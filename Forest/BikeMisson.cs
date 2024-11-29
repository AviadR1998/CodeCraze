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
    public static bool isQuestionActive = false;
    public GameObject finishMission;
    private AudioSource audioSource;
    public AudioSource BackgroundMusic;
    public GameObject bikeCamera;
    public AudioSource bikeAudioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && !isTaskActive && !NotSimultTasks.someMission)
        {
            NotSimultTasks.someMission = true;
            //Close background music.
            BackgroundMusic.Pause();
            isTaskActive = true;
            bikeCamera.SetActive(true);

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
            Firstcanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    public void ButtonBikeClick()
    {
        //Firstcanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        canvas.SetActive(true);

        //PLAYER get back to the original place.
        FindObjectOfType<FirstPersonController>().transform.position = originalPlayerPosition;
        FindObjectOfType<FirstPersonController>().transform.rotation = originalPlayerRotation;
        //CAMERA get back to the original place.
        FindObjectOfType<FirstPersonController>().playerCamera.transform.position = originalCameraPosition;
        FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = originalCameraRotation;
        FindObjectOfType<FirstPersonController>().cameraCanMove = true;
        FindObjectOfType<FirstPersonController>().playerCanMove = true;
        FindObjectOfType<FirstPersonController>().enableHeadBob = true;

        //reset all game.
        GameStarted();
        if (StartSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(StartSound);
        }
    }

    private void GameStarted()
    {
        //Reset num of question to ask to 0, reset oepration and make new question.
        questionCounter = 0;
        InitializeOperations();
        GenerateQuestion();
        isQuestionActive = true;
    }

    private void InitializeOperations()
    {
        // 0 = +, 1 = -, 2 = /, 3 = %, 4 = *
        operations = new List<int> { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 };
    }

    //This function works when user click on "Check answer" button.
    public void CheckAnswer()
    {
        int playerAnswer;
        if (int.TryParse(answerInput.text, out playerAnswer))
        {
            //Correct answer.
            if (playerAnswer == correctAnswer)
            {
                answerInput.image.color = Color.white;
                //Make succes sound.
                if (SuccSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(SuccSound);
                }
                questionCounter++;
                answerInput.text = "";
                //Didn't finish all questions.
                if (questionCounter < totalQuestions)
                {
                    isQuestionActive = false;
                    isMoving = true;
                    canvas.SetActive(false);
                }
                //Finish all questions.
                else
                {
                    canvas.SetActive(false);
                    isMoving = true;
                }
            }
            //Wrong answer.
            else
            {
                if (WrongSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(WrongSound);
                    answerInput.image.color = Color.red;
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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        //If the operations list is empty, stop and do not generate a new question.
        if (operations.Count == 0) return;
        int num1 = Random.Range(1, 13);
        int num2 = Random.Range(1, 13);
        //Pick a random index from the operations list.
        int randomIndex = Random.Range(0, operations.Count);
        // Get the operation at the randomly selected index.
        int operation = operations[randomIndex];
        //Remove the selected operation from the list.
        operations.RemoveAt(randomIndex);
        switch (operation)
        {
            case 0:
                questionText.text = "int " + num1 + " + " + num2 + " = ?";
                correctAnswer = num1 + num2;
                break;
            case 1:
                questionText.text = "int " + num1 + " - " + num2 + " = ?";
                correctAnswer = num1 - num2;
                break;
            case 2:
                num1 = Random.Range(2, 13);
                num2 = Random.Range(1, num1);
                //Make sure it's divided nice.
                while (num1 % num2 != 0)
                {
                    num2 = Random.Range(1, num1);
                }
                questionText.text = "int " + num1 + " / " + num2 + " = ?";
                correctAnswer = num1 / num2;
                break;
            case 3:
                questionText.text = "int " + num1 + " % " + num2 + " = ?";
                correctAnswer = num1 % num2;
                break;
            case 4:
                questionText.text = "int " + num1 + " * " + num2 + " = ?";
                correctAnswer = num1 * num2;
                break;
        }
    }


    void MoveTowardsNextPoint()
    {
        if (isMoving)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
            if (bikeAudioSource != null && !bikeAudioSource.isPlaying)
            {
                bikeAudioSource.Play();
            }
            //Check if the bike has reached the next point (distance is less than 0.1).
            if (Vector3.Distance(transform.position, dots[currentDotIndex].transform.position) < 0.1f)
            {
                currentDotIndex++;
                // If the bike has reached the end of the path (last point), reset the index to 0.
                if (currentDotIndex >= dots.Length)
                {
                    currentDotIndex = 0;
                }
                //Check if the player has completed all the questions (last question reached).
                if (questionCounter >= totalQuestions - 1)
                {
                    StartCoroutine(HandleQuestionsAndCompleteTask());
                    StartCoroutine(WaitBeforeDeactivatingTask());
                    canvas.SetActive(false);
                    isMoving = false;
                    if (bikeAudioSource != null)
                    {
                        bikeAudioSource.Stop();
                    }
                    StartCoroutine(ResetBikePositionWithDelay());
                    return;
                }
                //Player didn't finish all the queations.
                if (questionCounter < totalQuestions - 1)
                {
                    isQuestionActive = true;
                    isMoving = false;
                    if (bikeAudioSource != null)
                    {
                        bikeAudioSource.Stop();
                    }
                    canvas.SetActive(true);
                    GenerateQuestion();
                }
            }
            //Bike ride to next point.
            transform.position = Vector3.MoveTowards(transform.position, dots[currentDotIndex].transform.position, Time.deltaTime * speed);
            Vector3 direction = (dots[currentDotIndex].transform.position - transform.position).normalized;
            //We have where to move.
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

    private IEnumerator WaitBeforeDeactivatingTask()
    {
        //Wait 5 seconds and let the player do the task again if he want to.
        yield return new WaitForSeconds(5);
        isTaskActive = false;
    }

    public void CompleteTask()
    {
        NotSimultTasks.someMission = false;
        BackgroundMusic.Play();
        if (!isTaskCompletedOnce && TaskManager.currentTaskIndex == 5)
        {
            isTaskCompletedOnce = true;
            if (taskManager != null)
            {
                StartCoroutine(ActivateNextTaskWithDelay());
            }
            else
            {
                Debug.LogError("TaskManager is not assigned in the Inspector!");
            }
        }
    }

    private IEnumerator HandleQuestionsAndCompleteTask()
    {
        FreeQueBike.ask = true;
        FreeQueBike.keepWhile = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Finish all practice qustions.
        yield return new WaitUntil(() => !FreeQueBike.keepWhile);
        bikeCamera.SetActive(false);
        arrow.SetActive(true);
        //Finish mission sound + canvas.
        finishMission.GetComponent<SoundEffects>().PlaySoundClip();
        finishMission.SetActive(true);
        //Send player to next mission.
        CompleteTask();
        //Update state for saving the game.
        PauseMenu.updateSave("Forest", "Fire", 0);
    }

    private IEnumerator ResetBikePositionWithDelay()
    {
        //Wait 4 seconds and return bike to iniate position.
        yield return new WaitForSeconds(4);
        transform.position = new Vector3(37.59f, 4.62f, 46.37f);
        transform.rotation = Quaternion.Euler(0, 35.581f, 0);
    }
    private IEnumerator ActivateNextTaskWithDelay()
    {
        yield return new WaitForSeconds(5);
        taskManager.ActivateNextTask();
    }
}
