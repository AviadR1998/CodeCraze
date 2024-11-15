using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TaskManager : MonoBehaviour
{
    // מערך חצים לכל משימה לפי הסדר
    public GameObject[] taskArrows;

    // מערך אובייקטים לכל משימה
    public GameObject[] taskObjects;

    // אינדקס המשימה הנוכחית
    private int currentTaskIndex = 0;

    public GameObject taskCanvas; // הקנבס לתצוגה זמנית

    void Update()
    {
        // בדיקה אם לחצן TAB נלחץ
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //Debug.Log("Cheat activated! Skipping to the next task.");
            ActivateNextTask();
        }
    }


    void Start()
    {
        // התחלת המשימה הראשונה
        ActivateTaskByIndex(currentTaskIndex);
    }

    // הפעלת המשימה לפי מספר האינדקס
    public void ActivateTaskByIndex(int taskIndex)
    {
        switch (taskIndex)
        {
            case 0: // Task 1 - Swing in the park
                ActivateArrowAndObject(taskIndex);
                ShowCanvasWithMessage("Task 1: Head to the park and find the pink arrow near the swing!");
                break;

            case 1: // Task 2 - Sign near the swing
                ActivateArrowAndObject(taskIndex);
                ShowCanvasWithMessage("Task 2: Go to the sign next to the swing, marked with the pink arrow.");
                break;

            case 2: // Task 3 - Coffee area
                ActivateArrowAndObject(taskIndex);
                ShowCanvasWithMessage("Task 3: Walk to the café area where the pink arrow points.");
                break;

            case 3: // Task 4 - Boxes near the castle
                ActivateArrowAndObject(taskIndex);
                ShowCanvasWithMessage("Task 4: Make your way to the castle and look for the boxes near the pink arrow.");
                break;

            case 4: // Task 5 - Boat at the river
                ActivateArrowAndObject(taskIndex);
                ShowCanvasWithMessage("Task 5: Head to the river and find the boat near the pink arrow.");
                break;

            case 5: // Task 6 - Red bike
                ActivateArrowAndObject(taskIndex);
                ShowCanvasWithMessage("Task 6: Go to the bike trail and find the red bike by the pink arrow.");
                break;

            case 6: // Task 7 - Camping area
                ActivateArrowAndObject(taskIndex);
                ShowCanvasWithMessage("Task 7: Walk to the camping area where the pink arrow is pointing.");
                break;

            default:
                ShowCanvasWithMessage("Congratulations! You’ve completed all the tasks!");
                break;
        }
    }


    private void ShowCanvasWithMessage(string message)
    {
        // מציג את הקנבס
        taskCanvas.SetActive(true);

        // משנה את הטקסט של המשימה
        taskCanvas.GetComponentInChildren<TMP_Text>().text = message;

        // מסתיר את הקנבס אחרי 4 שניות
        StartCoroutine(HideCanvasAfterDelay());
    }

    private IEnumerator HideCanvasAfterDelay()
    {
        yield return new WaitForSeconds(6); // מחכה 4 שניות
        taskCanvas.SetActive(false); // מסתיר את הקנבס
    }


    // פונקציה להפעלת חץ ואובייקט
    private void ActivateArrowAndObject(int index)
    {
        if (index < taskArrows.Length && index < taskObjects.Length)
        {
            taskArrows[index].SetActive(true); // הפעלת החץ
            taskObjects[index].SetActive(true); // הפעלת האובייקט
        }
    }

    // מעבר למשימה הבאה
    public void ActivateNextTask()
    {
        currentTaskIndex++; // עדכון האינדקס
        ActivateTaskByIndex(currentTaskIndex); // הפעלת המשימה הבאה
    }
}
