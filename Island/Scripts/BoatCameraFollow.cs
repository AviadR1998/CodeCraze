using UnityEngine;

public class BoatCameraFollow : MonoBehaviour
{
    public Transform boat;      // Assign the boat's transform in the Inspector
    public Vector3 offset = new Vector3(0, 5, 10);  // Adjust offset as needed
    public Transform lookAtObj;
    public float followSpeed = 5f;

    // Mouse Look Parameters
    public float lookSpeedX = 2.0f;
    public float lookSpeedY = 2.0f;
    private float rotationY = 0f;
    private float rotationX = 0f;

    // Look-at parameters
    public float lookAtSpeed = 2.0f; // Speed at which camera looks at the lookAtObj
    public float lookAtThreshold = 0.1f; // Threshold when to switch to free-look mode
    private bool isLookingAtTarget = true; // Track if the camera is still looking at the target

    // Store the camera's final rotation when transitioning to free-look mode
    private Quaternion finalLookAtRotation;

    private void Update()
    {
        if (isLookingAtTarget)
        {
            // Camera smoothly looks at the target (lookAtObj)
            Vector3 targetDir = lookAtObj.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);

            // If the camera is close enough to the target orientation, allow free mouse movement
            if (Quaternion.Angle(transform.rotation, targetRotation) < lookAtThreshold)
            {
                isLookingAtTarget = false; // Switch to free mouse look mode
                finalLookAtRotation = transform.rotation; // Store the final rotation
            }
        }
        else
        {
            // Free mouse look mode
            rotationX += Input.GetAxis("Mouse X") * lookSpeedX; // Horizontal mouse movement
            rotationY -= Input.GetAxis("Mouse Y") * lookSpeedY; // Vertical mouse movement

            // Clamp the vertical rotation to prevent flipping the camera upside down
            rotationY = Mathf.Clamp(rotationY, -30f, 30f);

            // Update the camera's rotation based on mouse input and the stored final rotation
            transform.rotation = finalLookAtRotation * Quaternion.Euler(rotationY, rotationX, 0);
        }
    }

    void LateUpdate()
    {
        Vector3 targetPosition = boat.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        //transform.LookAt(lookAtObj);  // Make the camera look at the boat
    }

    public void lookOtherDirectionX()
    {
        offset.x *= -1;
    }

    public void lookOtherDirectionZ()
    {
        offset.z *= -1;
    }
}

