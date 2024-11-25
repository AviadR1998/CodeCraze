using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToLastPoint : MonoBehaviour
{
    public TaskManager taskManager; 
    public GameObject endAllCanvas;
    public GameObject CastleBox;

    void Start()
    {
        string savedTask = Login.task;
        int taskIndex = GetTaskIndex(savedTask);
        TaskManager.currentTaskIndex = taskIndex;
        //ALL missions should be open.
        if (Login.world == "Free" || Login.task == "Finish")
        {
            for (int i = 0; i < 7; i++)
            {
                taskManager.ActivateArrowAndObject(i); 
            }
            StartCoroutine(ShowEndCanvasWithDelay());
            CastleBox.SetActive(true);
            return;
        }

        if (taskIndex != -1)
        {
            taskManager.ActivateTaskByIndex(taskIndex);
            if (taskIndex != 0)
            {
                for (int i = 0; i < taskIndex; i++)
                {
                    taskManager.ActivateArrowAndObject(i); 
                }
            }
        }
        else
        {
            Debug.LogWarning("Invalid task received: " + savedTask);
        }
    }

    private int GetTaskIndex(string task)
    {
        switch (task)
        {
            case "Swing":
                return 0;
            case "Sign":
                return 1;
            case "Coffee":
                return 2;
            case "Box":
                return 3;
            case "Fish":
                return 4;
            case "Bike":
                return 5;
            case "Fire":
                return 6;
            case "Finish":
                return 7;
            default:
                return -1; 
        }
    }
    private IEnumerator ShowEndCanvasWithDelay()
    {
        yield return new WaitForSeconds(1);
        //Open end canvas.
        endAllCanvas.SetActive(true);
        //wait 10 seconds and close canvas.
        yield return new WaitForSeconds(10);
        endAllCanvas.SetActive(false);
    }
}
