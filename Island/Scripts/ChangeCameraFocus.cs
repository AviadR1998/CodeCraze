using UnityEngine;



//This script respawn the player and change its camera focus and lock its camera
public class ChangeCameraFocus : MonoBehaviour
{
    public Transform player;        // The player object
    public Transform focusTarget;   // The target object the player should face
    public Transform targetLocation;
    public GameObject[] objectsToDeactivate = null, objectsToActivate = null;
    public Canvas dangerArea;
    public static bool isSailing = false, goingBack = false;
    public bool workOnSail = false, justWhenBack = false;
    public float tooCloseToTarget = 0.1f, enoughRotationToTarget = 1f;
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
            // Ensure player looks at target
            RotatePlayerTowardsTarget();
        }
    }

    private void RotatePlayerTowardsTarget()
    {
        if (focusTarget == null || player == null)
        {
            Debug.LogError("FocusTarget or Player is not assigned.");
            return;
        }

        // Find direction to the target
        Vector3 directionToTarget = focusTarget.position - player.position;
        directionToTarget.y = 0; // Keep rotation on the y-axis

        if (directionToTarget.magnitude < tooCloseToTarget)
        {
            Debug.LogWarning("Target and player are too close; skipping rotation.");
            return;
        }

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly rotate the player
        player.rotation = Quaternion.Slerp(player.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Check if rotation is close enough to target
        if (Quaternion.Angle(player.rotation, targetRotation) < enoughRotationToTarget)
        {
            shouldLookAtTarget = false; // Unlock rotation
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if ((!isSailing || (workOnSail && (!justWhenBack || goingBack))) && !StatueLimitation.shouldLimit)
        {
            if (workOnSail)
            {
                isSailing = false;
            }
            shouldLookAtTarget = true;
            foreach (GameObject gameObject in objectsToDeactivate)
            {
                gameObject.SetActive(false);
            }
            foreach (GameObject gameObject in objectsToActivate)
            {
                gameObject.SetActive(true);
            }
            player.transform.position = targetLocation.position;
            if (dangerArea != null)
            {
                dangerArea.gameObject.SetActive(true);
            }
        }

    }



    // Optionally, call this method to allow the player to look at the target again
    public void LockLookAtTarget(bool lockTarget)
    {
        shouldLookAtTarget = lockTarget;
    }
}

