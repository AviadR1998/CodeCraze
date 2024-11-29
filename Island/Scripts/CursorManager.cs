using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Canvas[] canvasesToIgnore;

    void Update()
    {
        // Find all canvases in the scene
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();

        // Check if any canvas is active except the ignored ones
        bool isAnyCanvasActive = false;
        bool toIgnore = false;
        foreach (Canvas canvas in allCanvases)
        {

            foreach (Canvas canvasToIgnore in canvasesToIgnore)
            {
                if (canvasToIgnore == canvas)
                {
                    toIgnore = true;
                    break;
                }
            }
            if (toIgnore)
            {
                toIgnore = false;
                continue;
            }

            if (canvas.gameObject.activeInHierarchy)
            {
                isAnyCanvasActive = true;
                break;
            }
        }

        // Update cursor state based on the result
        if (isAnyCanvasActive)
        {
            // Turn on the cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Hide and lock the cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
