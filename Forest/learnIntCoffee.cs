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
  public  GameObject canvasError;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            explainWorlds.text = "Let's Make Coffee Cups with int!\n" + "Hey there, young programmer! Today, we're going to learn how to use something called an integer or int in our game to make coffee cups appear at the café.\n\n" + "What is an int?\n" + "An int is a special type of number in programming that doesn't have any decimal points. It means whole numbers like 1, 2, 3, 10, 100, and so on.\n\n" +
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

        int num = int.Parse(userInput);
        //check it's b<etween 1-4. ////////////////
          if (num < 0 || num > 4)
        {
            canvasError.SetActive(true);

            // Start coroutine to clear the error message after 4 seconds
                StartCoroutine(HideCanvasAfterTime(3f));

            return; // Exit the function early
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
