using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PracticeManage : MonoBehaviour
{
    public GameObject askQuestion;

    public static bool ask;

    public static bool keepWhile , questionActivated = false;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ask)
        {
            askQuestion.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            ask = false;
            questionActivated = true;
        }

        if (!askQuestion.active && questionActivated)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            questionActivated = false;
            keepWhile = false;

        }



    }


}
