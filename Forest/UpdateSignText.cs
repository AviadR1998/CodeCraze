using UnityEngine;
using TMPro;
using System.Collections;

public class UpdateSignText : MonoBehaviour
{
    public GameObject canvas;
    public TMP_InputField inputField;
    public TMP_Text signText;
    public AudioClip WriteSound;
    public AudioSource BackgroundMusic;
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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isTaskActive && !NotSimultTasks.someMission)
        {
            NotSimultTasks.someMission = true;
            BackgroundMusic.Pause();
            isTaskActive = true;

            //Save PLAYER position and rotation.
            originalPlayerPosition = FindObjectOfType<FirstPersonController>().transform.position;
            originalPlayerRotation = FindObjectOfType<FirstPersonController>().transform.rotation;
            //Save CAMERA position and rotation.
            originalCameraPosition = FindObjectOfType<FirstPersonController>().playerCamera.transform.position;
            originalCameraRotation = FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation;
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
        //Limit to name size.
        if (userInput.Length > 7)
        {
            userInput = userInput.Substring(0, 7);
        }
        //Put the input on the sign.
        signText.text = userInput;
        canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Write sound.
        if (WriteSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(WriteSound);
        }
        arrow.SetActive(true);
        //Wait couple of seconds.
        StartCoroutine(HandleQuestionsAndCompleteTask());
    }

    IEnumerator BacktoPosition()
    {
        yield return new WaitForSeconds(0.4f);

        //PLAYER get back to the original place.
        FindObjectOfType<FirstPersonController>().transform.position = originalPlayerPosition;
        FindObjectOfType<FirstPersonController>().transform.rotation = originalPlayerRotation;
        //CAMERA get back to the original place.
        FindObjectOfType<FirstPersonController>().playerCamera.transform.position = originalCameraPosition;
        FindObjectOfType<FirstPersonController>().playerCamera.transform.rotation = originalCameraRotation;
        FindObjectOfType<FirstPersonController>().cameraCanMove = true;
        FindObjectOfType<FirstPersonController>().playerCanMove = true;
        FindObjectOfType<FirstPersonController>().enableHeadBob = true;

        StartCoroutine(WaitBeforeDeactivatingTask());
    }

    private IEnumerator WaitBeforeDeactivatingTask()
    {
        yield return new WaitForSeconds(4);
        isTaskActive = false;
    }
    public void CompleteTask()
    {
        NotSimultTasks.someMission = false;
        BackgroundMusic.Play();
        if (!isTaskCompletedOnce && TaskManager.currentTaskIndex == 1)
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
        yield return new WaitForSeconds(1.2f);
        //Start practice questions.
        FreeQueSign.ask = true;
        FreeQueSign.keepWhile = true;
        //Finish all practice qustions.
        yield return new WaitUntil(() => !FreeQueSign.keepWhile);
        CompleteTask();
        //Send player to next mission.
        finishMission.GetComponent<SoundEffects>().PlaySoundClip();
        finishMission.SetActive(true);
        //Update state for saving the game.
        PauseMenu.updateSave("Forest", "Coffee", 0);
        StartCoroutine(BacktoPosition());
    }
}
