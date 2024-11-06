using Unity.VisualScripting;
using UnityEngine;

public class InteractOnCollision : MonoBehaviour
{
    private bool isColliding = false;
    private bool ePressed = false;
    // public GameObject playerCamera;

    public GameObject canvas;
    GameObject torch;

    private void OnEnable()
    {
        print("This is me");
    }

    private void OnTriggerEnter(Collider other)
    {
        torch = other.GameObject();
        print("collide");
        // Check if the object we collided with has a specific tag (e.g., "Interactable")
        if (other.CompareTag("Interactable"))
        {
            isColliding = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!ePressed)
        {
            canvas.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            print("ZZZZZZ");
            // Do something, e.g., print a message or call a method
            Debug.Log("E key pressed while colliding with the object!");
            // PerformAction();
            ePressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object we are leaving has a specific tag (e.g., "Interactable")
        if (other.CompareTag("Interactable"))
        {
            isColliding = false;
        }
    }

    private void Update()
    {
        // Check if we are colliding with the object and the "E" key is pressed
        // if (ePressed)
        // {
        //     // Implement the action to be performed
        //     Debug.Log("Action performed!");
        //     torch.transform.position = GameObject.Find("FirstPersonController").transform.position + new Vector3(
        //     playerCamera.transform.forward.x,
        //     playerCamera.transform.forward.y,
        //     playerCamera.transform.forward.z);
        // }
    }

    // private void PerformAction()
    // {
    //     // Implement the action to be performed
    //     Debug.Log("Action performed!");
    //     torch.transform.position = GameObject.Find("FirstPersonController").transform.position + new Vector3(
    //     playerCamera.transform.forward.x,
    //     playerCamera.transform.forward.y,
    //     playerCamera.transform.forward.z);
    // }
}
