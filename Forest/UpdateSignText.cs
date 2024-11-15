using UnityEngine;
using TMPro;
using System.Collections;


public class UpdateSignText : MonoBehaviour
{
    public GameObject canvas;
    public TMP_InputField inputField;
    public TMP_Text signText;
    public AudioClip WriteSound;
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
    // public GameObject askQuestion;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isTaskActive)
        {
            isTaskActive = true;
            //Save Player data
            originalPlayerPosition = FindObjectOfType<FirstPersonController>().transform.position;
            originalPlayerRotation = FindObjectOfType<FirstPersonController>().transform.rotation;
            //Save Camera data
            originalCameraPosition = FindObjectOfType<FirstPersonController>().playerCamera.transform.position;
            originalCameraRotation = FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation;

            // Debug.Log("Original Player Position: " + originalPlayerPosition);
            // Debug.Log("Original Player Rotation: " + originalPlayerRotation.eulerAngles);
            // Debug.Log("Original Camera Position: " + originalCameraPosition);
            // Debug.Log("Original Camera Rotation: " + originalCameraRotation.eulerAngles);

            //change rotation + position
            FindObjectOfType<FirstPersonController>().playerCamera.transform.position = destination.position;
            FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = destination.rotation;
            FindObjectOfType<FirstPersonController>().cameraCanMove = false;
            FindObjectOfType<FirstPersonController>().playerCanMove = false;
            FindObjectOfType<FirstPersonController>().enableHeadBob = false;
            arrow.SetActive(false);
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inputField.text = "";
        }
    }

    private void Update()
    {

    }

    public void ButtonSingClick()
    {
        string userInput = inputField.text;
        if (userInput.Length > 7)
        {
            userInput = userInput.Substring(0, 7);
        }
        signText.text = userInput;
        canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (WriteSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(WriteSound);
        }
        StartCoroutine(ExampleCoroutine());
        arrow.SetActive(true);
        // askQuestion.SetActive(true);
        // while (askQuestion.active)
        // {

        // }
        // PracticeManage.ask = true;
        StartCoroutine(HandleQuestionsAndCompleteTask());


    }

    IEnumerator ExampleCoroutine()
    {

        yield return new WaitForSeconds(1.1f);
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
        StartCoroutine(WaitBeforeDeactivatingTask());
    }

    private IEnumerator WaitBeforeDeactivatingTask()
    {
        yield return new WaitForSeconds(4);
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
    private IEnumerator HandleQuestionsAndCompleteTask()
    {
        // הפעלת השאלות
        PracticeManage.ask = true;
        PracticeManage.keepWhile = true;
        // המתנה עד שהשאלות יסיימו
        yield return new WaitUntil(() => !PracticeManage.keepWhile);

        // קריאה ל-CompleteTask רק אחרי שהשאלות סיימו
        CompleteTask();
    }
}
