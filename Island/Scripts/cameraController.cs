using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Canvas[] canvases; // Array of all canvases to monitor
    public MonoBehaviour cameraControlScript; // Reference to the camera control script
    private bool canMoveCamera = true; // Flag to manage camera movement state

    void Update()
    {
        // Check if any canvas is active
        bool isAnyCanvasActive = false;
        foreach (Canvas canvas in canvases)
        {
            if (canvas.gameObject.activeInHierarchy)
            {
                isAnyCanvasActive = true;
                break;
            }
        }

        // Enable or disable camera movement script
        if (isAnyCanvasActive && canMoveCamera)
        {
            DisableCameraMovement();
            canMoveCamera = false;
        }
        else if (!isAnyCanvasActive && !canMoveCamera)
        {
            EnableCameraMovement();
            canMoveCamera = true;
        }
    }

    void DisableCameraMovement()
    {
        cameraControlScript.enabled = false; // Disables the camera movement script
    }

    void EnableCameraMovement()
    {
        cameraControlScript.enabled = true; // Enables the camera movement script
    }
}

