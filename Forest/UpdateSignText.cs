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
    private bool isTaskActive = false;

    public Transform cameraTargetPosition;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    public GameObject arrow;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isTaskActive)
        {
            arrow.SetActive(false);
            isTaskActive = true;
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inputField.text = "";
            originalCameraPosition = Camera.main.transform.position;
            originalCameraRotation = Camera.main.transform.rotation;

            // Debug.Log("Original Camera Position: " + originalCameraPosition);
            // Debug.Log("Original Camera Rotation: " + originalCameraRotation.eulerAngles);
            // Debug.Log("Moving camera to target position");
        }
    }

    private void Update()
    {
        if (isTaskActive)
        {
            Camera.main.transform.position = cameraTargetPosition.position;
            Camera.main.transform.rotation = cameraTargetPosition.rotation;
        }
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


        // Debug.Log("Returning camera to original position");
        // Debug.Log("Original Camera Position: " + originalCameraPosition);
        // Debug.Log("Original Camera Rotation: " + originalCameraRotation.eulerAngles);
        if (WriteSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(WriteSound);
        }

        StartCoroutine(ExampleCoroutine());
        arrow.SetActive(true);

        // Camera.main.transform.position = originalCameraPosition;
        // Camera.main.transform.rotation = originalCameraRotation;
    }

    IEnumerator ExampleCoroutine()
    {

        yield return new WaitForSeconds(1.1f);
        Camera.main.transform.position = originalCameraPosition;
        Camera.main.transform.rotation = originalCameraRotation;
        isTaskActive = false;
    }
}
