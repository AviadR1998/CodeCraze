// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TorchCollector : MonoBehaviour
// {
//     public GameObject player;  // Reference to the player GameObject
//     public GameObject newTorch;
//     private bool isPlayerNear = false;  // To track if the player is near the torch
//     private bool isTorchCollected = false;  // To track if the torch has been collected

//     // Update is called once per frame
//     void Update()
//     {
//         if (isPlayerNear && !isTorchCollected)
//         {
//             if (Input.GetKeyDown(KeyCode.E))  // When the player presses 'E'
//             {
//                 CollectTorch();
//             }
//         }
//     }

//     // This method runs when the player enters the trigger zone
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.gameObject == player)
//         {
//             isPlayerNear = true;
//             // Show prompt here (UI can be implemented for better UX)
//             Debug.Log("Press 'E' to collect the torch.");
//         }
//     }

//     // This method runs when the player exits the trigger zone
//     private void OnTriggerExit(Collider other)
//     {
//         if (other.gameObject == player)
//         {
//             isPlayerNear = false;
//             // Hide prompt if the player leaves the area
//             Debug.Log("Player left the torch area.");
//         }
//     }

//     // This method handles collecting the torch
//     private void CollectTorch()
//     {
//         isTorchCollected = true;
//         // Add the torch to the box (you can implement this as needed)
//         Debug.Log("Torch added to the box.");
//         gameObject.SetActive(false);  // Optional: Hide the torch after collection
//         newTorch.SetActive(true);
//     }
// }


using UnityEngine;
using UnityEngine.UI;

public class TorchCollector : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public GameObject wantAddTorchCanvas; // Reference to the canvas with yes/no options
    public GameObject newTorch;
    public Button yesButton; // Reference to the "yes" button
    public Button noButton; // Reference to the "no" button
    private bool isPlayerNear = false; // To track if the player is near the torch
    private bool isTorchCollected = false; // To track if the torch has been collected

    void Start()
    {
        // Hide the canvas initially
        wantAddTorchCanvas.SetActive(false);

        // Set up listeners for the buttons
        yesButton.onClick.AddListener(CollectTorch);
        noButton.onClick.AddListener(CloseCanvas);
    }

    void Update()
    {
        // No need for input detection here since we are using buttons
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !isTorchCollected)
        {
            isPlayerNear = true;
            // Show the canvas with yes/no options when near the torch
            wantAddTorchCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = false;
            // Hide the canvas when the player leaves the area
            wantAddTorchCanvas.SetActive(false);
        }
    }

    // This method handles collecting the torch
    private void CollectTorch()
    {
        if (!isPlayerNear)
        {
            return;
        }
        isTorchCollected = true;
        Debug.Log("Torch added to the box.");
        gameObject.SetActive(false); // Hide the torch after collection

        // Hide the canvas after collecting the torch
        wantAddTorchCanvas.SetActive(false);
        newTorch.SetActive(true);
    }

    // This method is called when the "no" button is pressed
    private void CloseCanvas()
    {
        wantAddTorchCanvas.SetActive(false);
    }
}
