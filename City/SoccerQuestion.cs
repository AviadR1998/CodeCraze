using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoccerQuestion : MonoBehaviour
{
    public static string questionCodeStatic;
    public static string questionTextStatic;
    public static string explanationTextStatic;
    public static string[] optionsStatic = new string[4] { "", "", "", "" };
    public static string curentRightAnswerStatic;
    public static int timeStatic;
    public static bool questionAnswered, ifSelectedOpion;

    public GameObject soccerQuestionCanvas;
    public TMP_Text questionCode;
    public TMP_Text questionText;
    public TMP_Text explanationText;
    public TMP_Text[] optionsText;
    public TMP_Text timeText;
    string currentRightAnswer, currentAnswerPressed;
    

    // Start is called before the first frame update
    void Start()
    {
        ifSelectedOpion = questionAnswered = false;
    }

    void Update()
    {
        questionCode.text = questionCodeStatic;
        currentRightAnswer = curentRightAnswerStatic;
        questionText.text = questionTextStatic;
        for (int i = 0; i < optionsStatic.Length; i++)
        {
            optionsText[i].text = optionsStatic[i];
        }
        timeText.text = timeStatic + "";
    }

    public static void initiateQuestion(string questionCode, string questionText, string explanation, List<int> options, string correctAnswer)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SoccerMovment.haveAnswer = false;
        questionCodeStatic = questionCode;
        questionTextStatic = questionText;
        explanationTextStatic = explanation;
        for (int i = 0;i < options.Count; i++) 
        {
            optionsStatic[i] = options[i] + "";
        }
        curentRightAnswerStatic = correctAnswer;
        timeStatic = 10;
    }

    public void optionPressed(TMP_Text option)
    {
        if (questionAnswered)
        {
            return;
        }
        ifSelectedOpion = true;
        currentAnswerPressed = option.text;
        for (int i = 0; i < optionsText.Length; i++)
        {
            optionsText[i].color = Color.white;
        }
        option.color = Color.yellow;
    }

    public void okPress()
    {
        if (!ifSelectedOpion)
        {
            return;
        }
        if (questionAnswered)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SoccerMovment.ifInitiateTurn = SoccerMovment.haveAnswer = true;
            ifSelectedOpion = false;
            for (int i = 0; i < optionsText.Length; i++)
            {
                optionsText[i].color = Color.white;
            }
            timeText.color = Color.white;
            explanationText.text = "";
            timeStatic = 10;
            soccerQuestionCanvas.SetActive(false);
        }
        else
        {
            explanationText.text = "Explanation:\n" + explanationTextStatic;
            for (int i = 0; i < optionsText.Length; i++)
            {
                if (optionsText[i].text == currentAnswerPressed)
                {
                    optionsText[i].color = Color.red;
                }
                if (optionsText[i].text == currentRightAnswer)
                {
                    optionsText[i].color = Color.green;
                }
            }
            SoccerMovment.turn = currentAnswerPressed == currentRightAnswer;
        }
        questionAnswered = !questionAnswered;
    }
}
