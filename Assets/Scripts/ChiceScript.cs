using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChiceScript : MonoBehaviour
{
    //public GameObject TextBox;
    public Button Choice1, Choice2, Choice3, Choiice4;
    public int ChoiceMade;
    private Image Choice1Image;
    public Color highlightColor;

    public void ChoiceOption1()
    {
        ChoiceMade = 1;
        Choice1Image.color = highlightColor;
    }

    public void ChoiceOption2()
    {
        ChoiceMade = 2;
    }

    public void ChoiceOption3()
    {
        ChoiceMade = 3;
    }

    public void ChoiceOption4()
    {
        ChoiceMade = 4;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
