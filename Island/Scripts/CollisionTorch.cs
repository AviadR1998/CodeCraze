using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractOnCollision : MonoBehaviour
{
    private bool isColliding = false;
    private bool ePressed = false;

    private int stam = 1;

    public Button Choice1, Choice2;
    public int ChoiceMade;
    // public GameObject objectToActive;
    private string ObjectToDeactivate;
    // public GameObject playerCamera;

    public GameObject canvas;
    GameObject torch;

    public void OnTriggerEnter(Collider other)
    {
        torch = other.GameObject();
        print("collide");
        print("Triggered: " + gameObject.name);
        stam++;
        ObjectToDeactivate = gameObject.name;
        // Check if the object we collided with has a specific tag (e.g., "Interactable")
        if (other.CompareTag("Interactable"))
        {
            isColliding = true;
        }
    }

    // public void OnTriggerStay(Collider other)
    // {
    //     if (!ePressed)
    //     {
    //         canvas.SetActive(true);
    //         print("Triggered: " + gameObject.name);
    //     }
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         print("ZZZZZZ");
    //         // Do something, e.g., print a message or call a method
    //         Debug.Log("E key pressed while colliding with the object!");
    //         // PerformAction();
    //         ePressed = true;
    //     }
    // }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object we are leaving has a specific tag (e.g., "Interactable")
        if (other.CompareTag("Interactable"))
        {
            isColliding = false;
        }
    }


    public void ChoiceOption1()
    {
        ChoiceMade = 1;
    }

    public void ChoiceOption2()
    {
        ChoiceMade = 2;
    }


    public void OnClickSaveBtn()
    {
        if (ChoiceMade == 1)
        {
            // objectToActive.SetActive(true);
            // ObjectToDeactivate.SetActive(false);
            // objectToActive.SetActive(true);
            print("1");
            print(stam);
            print(ObjectToDeactivate);
            GameObject.Find(ObjectToDeactivate).SetActive(false);
            canvas.SetActive(false);
        }
        else if (ChoiceMade == 2)
        {
            print("2");
            canvas.SetActive(false);
        }
    }
}
