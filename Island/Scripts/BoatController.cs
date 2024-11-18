using UnityEngine;
using UnityEngine.InputSystem;  // Needed for input handling

public class BoatController : MonoBehaviour
{
    // public Transform player;
    // public Transform boatPosition;  // Position on boat for the player
    // public float sailingSpeed = 5f;
    // private bool isSailing = false;
    // private FirstPersonController playerController;  // Reference to the player movement script

    // void Start()
    // {
    //     // Find and store the FirstPersonController component
    //     playerController = player.GetComponent<FirstPersonController>();
    //     if (playerController == null)
    //     {
    //         Debug.LogError("Player's FirstPersonController script not found!");
    //     }
    // }

    // void Update()
    // {
    //     if (isSailing)
    //     {
    //         // Lock player movement
    //         // playerController.enabled = false;

    //         // Move the boat forward
    //         transform.Translate(Vector3.forward * sailingSpeed * Time.deltaTime);

    //         // Check if the player wants to stop sailing (for example, pressing 'E')
    //         if (Keyboard.current.eKey.wasPressedThisFrame)
    //         {
    //             StopSailing();
    //         }
    //     }
    //     else
    //     {
    //         // Check if the player is near the boat and presses 'E' to start sailing
    //         if (Vector3.Distance(player.position, transform.position) < 2f &&
    //             Keyboard.current.eKey.wasPressedThisFrame)
    //         {
    //             StartSailing();
    //         }
    //     }
    // }

    // void StartSailing()
    // {
    //     isSailing = true;
    //     player.position = boatPosition.position;  // Move player to the boat position
    //     player.SetParent(transform);              // Parent player to the boat
    // }

    // void StopSailing()
    // {
    //     isSailing = false;
    //     player.SetParent(null);                   // Unparent player from the boat
    //     playerController.enabled = true;          // Unlock player movement
    // }




    public GameObject boat;
    public Camera boatCamera;
    public Canvas getOnBoatCanvas;
    private bool isRender = true;

    private bool isPlayerInTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { // Ensure only the player triggers this
            isPlayerInTrigger = true;
            getOnBoatCanvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            getOnBoatCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown("e"))
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                // Disable the MeshRenderer
                meshRenderer.enabled = false;
            }
            getOnBoatCanvas.gameObject.SetActive(false);
            isRender = false;
            boat.gameObject.SetActive(true);
            boatCamera.gameObject.SetActive(true);
        }

    }
}
