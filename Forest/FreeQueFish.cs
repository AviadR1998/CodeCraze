using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FreeQueFish : MonoBehaviour
{
    public static bool ask;
    public static bool keepWhile, questionActivated = false;
    private Canvas qcanvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (ask)
        {
            //Open canvas.
            CreateQuestionsCanvas createQuestionsCanvas = GetComponent<CreateQuestionsCanvas>();
            if (createQuestionsCanvas != null)
            {
                qcanvas = createQuestionsCanvas.CreateQCanvas("CountPlusPlus.csv");
            }
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            ask = false;
            questionActivated = true;
        }

        //Once qcanva is off we know practice questions end.
        if (qcanvas != null && !qcanvas.gameObject.active && questionActivated)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            questionActivated = false;
            keepWhile = false;

        }



    }


}
