using UnityEngine;

public class CursorManager : MonoBehaviour
{
    // References to the two canvases to ignore
    [SerializeField] private Canvas canvasToIgnore1;
    [SerializeField] private Canvas canvasToIgnore2;
    [SerializeField] private Canvas canvasToIgnore3;
    [SerializeField] private Canvas canvasToIgnore4;

    void Update()
    {
        // Find all canvases in the scene
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();

        // Check if any canvas is active except the ignored ones
        bool isAnyCanvasActive = false;
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas == canvasToIgnore1 || canvas == canvasToIgnore2
                || canvas == canvasToIgnore3 || canvas == canvasToIgnore4)
                continue; // Skip ignored canvases

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
