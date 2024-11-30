using System.Collections.Generic;
using UnityEngine;



//This script manage the camera while canvases are on or off
public class CanvasCameraControl : MonoBehaviour
{
    public List<Canvas> canvases = new List<Canvas>();
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

    public void AddCanvas(Canvas canvas)
    {
        canvases.Add(canvas);
    }
}

