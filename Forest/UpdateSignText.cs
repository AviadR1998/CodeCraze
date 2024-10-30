using UnityEngine;
using TMPro;

public class UpdateSignText : MonoBehaviour
{
    public GameObject canvas;
    public TMP_InputField inputField;  // שדה הקלט שהמשתמש מכניס לתוכו טקסט
    public TMP_Text signText; // טקסט השלט שמשתנה
    public AudioClip WriteSound; // כאן תגררי את הסאונד
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
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            inputField.text = "";
        }
    }

    public void ButtonSingClick()
    {
        canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        string userInput = inputField.text;
        signText.text = userInput;
        if (WriteSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(WriteSound);
        }
    }

    void Update()
    {


    }







}
