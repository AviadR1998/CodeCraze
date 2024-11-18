// using UnityEngine;

// public class MissionTeleportAndLook : MonoBehaviour
// {
//     //public Camera mainCamera;          // Reference to the main camera
//     public Transform targetLocation;    // Location to teleport the player
//     public Transform focusTarget;       // Object for the camera to look at
//     public GameObject player;           // Player GameObject

//     private bool missionActivated = false; // Ensures mission triggers only once

//     void OnTriggerEnter(Collider other)
//     {
//         if (other.gameObject == player && !missionActivated)
//         {
//             missionActivated = true;
//             StartMission();
//         }

//         // missionActivated = true;
//         // StartMission();
//     }

//     private void StartMission()
//     {
//         // Move the player to the target location
//         //player.transform.position = targetLocation.position;

//         // Ensure the camera's position is aligned with the player's position
//         //mainCamera.transform.position = player.transform.position + new Vector3(0, 2, -5); // Adjust as needed for distance/angle

//         // Make the camera look directly at the focus target

//         Camera mainCamera = Camera.main;
//         // Vector3 targetPosition = mainCamera.transform.position + mainCamera.transform.forward * 30;
//         // mainCamera.transform.LookAt(targetPosition);
//         mainCamera.transform.LookAt(focusTarget.position);
//         print("InStartMission");
//     }
// }


using UnityEngine;

public class ChangeCameraFocus : MonoBehaviour
{
    public Transform player;        // The player object
    public Transform focusTarget;   // The target object the player should face
    public Transform targetLocation;
    public GameObject[] objectsToDeactivate = null;
    private float rotationSpeed = 5f;  // Rotation speed for smooth rotation
    private bool shouldLookAtTarget = false;  // Flag to control if the player should look at the target

    void Start()
    {
        if (player == null || focusTarget == null)
        {
            Debug.LogError("Player or Focus Target is not assigned.");
        }
    }

    void Update()
    {
        if (shouldLookAtTarget)
        {
            // Rotate player towards target
            RotatePlayerTowardsTarget();
        }
        else
        {
            // Allow the player to move freely (no rotation lock)
            // You can add logic here for free camera movement or player rotation if needed
        }
    }

    void OnTriggerEnter(Collider other)
    {
        shouldLookAtTarget = true;
        foreach (GameObject gameObject in objectsToDeactivate)
        {
            gameObject.SetActive(false);
        }
        player.transform.position = targetLocation.position;
    }

    private void RotatePlayerTowardsTarget()
    {
        // Find the direction from the player to the target
        Vector3 directionToTarget = focusTarget.position - player.position;
        directionToTarget.y = 0;  // Keep the rotation on the y-axis only (no vertical rotation)

        // Calculate the target rotation based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly rotate the player towards the target
        player.rotation = Quaternion.Slerp(player.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // If the player is close to the target rotation, release the lock
        if (Quaternion.Angle(player.rotation, targetRotation) < 1f)
        {
            shouldLookAtTarget = false;  // Player can now look around freely
        }
    }

    // Optionally, call this method to allow the player to look at the target again
    public void LockLookAtTarget(bool lockTarget)
    {
        shouldLookAtTarget = lockTarget;
    }
}

