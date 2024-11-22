using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceScript : MonoBehaviour
{
    //public GameObject TextBox;
    public Button Choice1, Choice2;
    public int ChoiceMade;
    public GameObject objectToActive;
    public GameObject ObjectToDeactivate;

    public GameObject canvas;

    public void ChoiceOption1()
    {
        ChoiceMade = 1;
    }

    public void ChoiceOption2()
    {
        ChoiceMade = 2;
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void OnClickSaveBtn()
    {
        if (ChoiceMade == 1)
        {
            objectToActive.SetActive(true);
            ObjectToDeactivate.SetActive(false);
            canvas.SetActive(false);
        }
        else if (ChoiceMade == 2)
        {
            canvas.SetActive(false);
        }
    }
}
