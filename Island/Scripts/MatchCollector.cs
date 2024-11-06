using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchCollector : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public GameObject wantAddMatchCanvas; // Reference to the canvas with yes/no options
    public GameObject newMatch;
    public Button yesButton; // Reference to the "yes" button
    public Button noButton; // Reference to the "no" button
    private bool isPlayerNear = false; // To track if the player is near the torch
    private bool isMatchCollected = false; // To track if the torch has been collected

    void Start()
    {
        // Hide the canvas initially
        wantAddMatchCanvas.SetActive(false);

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
        if (other.gameObject == player && !isMatchCollected)
        {
            isPlayerNear = true;
            // Show the canvas with yes/no options when near the torch
            wantAddMatchCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNear = false;
            // Hide the canvas when the player leaves the area
            wantAddMatchCanvas.SetActive(false);
        }
    }

    // This method handles collecting the torch
    private void CollectTorch()
    {
        if (!isPlayerNear)
        {
            return;
        }
        isMatchCollected = true;
        Debug.Log("Match added to the box.");
        gameObject.SetActive(false); // Hide the torch after collection

        // Hide the canvas after collecting the torch
        wantAddMatchCanvas.SetActive(false);
        newMatch.SetActive(true);
    }

    // This method is called when the "no" button is pressed
    private void CloseCanvas()
    {
        wantAddMatchCanvas.SetActive(false);
    }
}
