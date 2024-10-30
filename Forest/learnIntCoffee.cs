using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using TMPro;
using System;
using UnityEngine.UI;
public class learnIntCoffee : MonoBehaviour
{
    public GameObject canvas;
    public GameObject canvasCoffee;
    //public GameObject canvasGotIt;
    public TMP_Text explainWorlds;
    public TMP_Text inputConsist;
    public TMP_InputField inputUser;
    public GameObject[] coffee;
    public GameObject canvasError;
    public AudioClip AddCoffeeSound;
    public AudioClip SuccSound;

    public AudioClip ErrSound;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            explainWorlds.text = "Let's Make Coffee Cups with int!\n" + "Hey there, young programmer! Today, we're going to learn how to use something called an integer or int in our game to make coffee cups appear at the café.\n\n" + "What is an int?\n" + "An int is a special type of number in programming that doesn't have any decimal points. It means whole numbers like 1, 2, 3, and so on.\n\n" +
            "And in a programming way, we would write it as: int coffeeCups = 3; which means i have 3 coffee cups " +
            "Why Use an int?\n" + "In our game, we want to decide how many coffee cups to show at the café. Since we can only have whole coffee cups (not half a cup!), an int is perfect for this job.\n";
        }
    }


    public void ButtonInfoClick()
    {
        canvas.SetActive(false);
        canvasCoffee.SetActive(true);
        //canvasGotIt.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        inputConsist.text = "int CupOfCoffee = 0;";
    }

    public void ButtonExit()
    {
        if (SuccSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(SuccSound);
        }
        inputUser.text = "";
        canvasCoffee.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }


    public void RunButtonClick()
    {
        //need to check is between 0 to 4.

        string userInput = inputUser.text;

        //nothing and click run is 0.
        if (string.IsNullOrEmpty(userInput))
        {
            userInput = 0.ToString(); ;
        }

        int num;
        if (!int.TryParse(userInput, out num) || num < 0 || num > 4) // בדיקה אם זה מספר בין 1 ל-4
        {
            if (ErrSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(ErrSound);
            }
            canvasError.SetActive(true);

            // Start coroutine to clear the error message after 3 seconds
            StartCoroutine(HideCanvasAfterTime(3f));

            return; // Exit the function early
        }

        if (AddCoffeeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(AddCoffeeSound);
        }

        for (int i = 3; i >= int.Parse(userInput); i--)
        {
            coffee[i].SetActive(false);
        }
        for (int i = 0; i < int.Parse(userInput); i++)
        {
            coffee[i].SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator HideCanvasAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Hide the canvas after the delay
        canvasError.SetActive(false);
    }


}
