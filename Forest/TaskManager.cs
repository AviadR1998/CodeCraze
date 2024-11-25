using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TaskManager : MonoBehaviour
{
    public GameObject[] taskArrows;
    public GameObject[] taskObjects;
    public static int currentTaskIndex = 0;
    public GameObject CastleBox;
    public GameObject taskCanvas; // הקנבס לתצוגה זמנית

    void Update()
    {
        //Cheats for playing the game :)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ActivateNextTask();
        }
    }

    void Start()
    {

    }

   
    public void ActivateTaskByIndex(int taskIndex)
    {
        switch (taskIndex)
        {
            case 0: // Task 1 - Swing in the park
                ActivateArrowAndObject(taskIndex);
                ShowCanvasWithMessage("Task 1: Head to the park and find the pink arrow near the swing!😀");
                break;

            case 1: // Task 2 - Sign near the swing
                ActivateArrowAndObject(taskIndex);
                PauseMenu.updateSave("Forest", "Sign", 0);
                ShowCanvasWithMessage("Task 2: Go to the sign next to the swing, marked with the pink arrow!😀");
                break;

            case 2: // Task 3 - Coffee area
                ActivateArrowAndObject(taskIndex);
                PauseMenu.updateSave("Forest", "Coffee", 0);
                ShowCanvasWithMessage("Task 3: Walk to the café area where the pink arrow points!😀");
                break;

            case 3: // Task 4 - Boxes near the castle
                ActivateArrowAndObject(taskIndex);
                PauseMenu.updateSave("Forest", "Box", 0);
                ShowCanvasWithMessage("Task 4: Make your way to the castle and look for the boxes near the pink arrow!😀");
                break;

            case 4: // Task 5 - Boat at the river
                ActivateArrowAndObject(taskIndex);
                PauseMenu.updateSave("Forest", "Fish", 0);
                ShowCanvasWithMessage("Task 5: Head to the river and find the boat near the pink arrow!😀");
                break;

            case 5: // Task 6 - Red bike
                ActivateArrowAndObject(taskIndex);
                PauseMenu.updateSave("Forest", "Bike", 0);
                ShowCanvasWithMessage("Task 6: Go to the bike trail and find the red bike by the pink arrow!😀");
                break;

            case 6: // Task 7 - Camping area
                ActivateArrowAndObject(taskIndex);
                PauseMenu.updateSave("Forest", "Fire", 0);
                ShowCanvasWithMessage("Task 7: Walk to the camping area where the pink arrow is pointing!😀");
                break;

            default:
                PauseMenu.updateSave("Forest", "Finish", 0);
                //ShowCanvasWithMessage("Congratulations! You’ve completed ALL the tasks!😀");
                CastleBox.SetActive(true);
                break;
        }
    }


    private void ShowCanvasWithMessage(string message)
    {
       
        taskCanvas.SetActive(true);
        taskCanvas.GetComponentInChildren<TMP_Text>().text = message;
        StartCoroutine(HideCanvasAfterDelay());
    }

    private IEnumerator HideCanvasAfterDelay()
    {
        yield return new WaitForSeconds(6); 
        taskCanvas.SetActive(false); 
    }


    public void ActivateArrowAndObject(int index)
    {
        if (index < taskArrows.Length && index < taskObjects.Length)
        {
            //Arrow.
            taskArrows[index].SetActive(true); 
            //Object to clash.
            taskObjects[index].SetActive(true); 
        }
    }

    public void ActivateNextTask()
    {
        currentTaskIndex++; 
        ActivateTaskByIndex(currentTaskIndex); 
    }
}
