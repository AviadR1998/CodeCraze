using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MissionInputOutput : MonoBehaviour
{
    public GameObject canvas;
    public TMP_InputField inputField;

    public SwingMovement swingScript;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }


    public void ButtonSwingClick()
    {
        canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        int numberOfRotations = int.Parse(inputField.text);
        inputField.text = "";
        canvas.SetActive(false);
        swingScript.StartCarousel(numberOfRotations);


    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
