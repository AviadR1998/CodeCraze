using UnityEngine;
using UnityEngine.InputSystem;  // Needed for input handling

public class BoatController : MonoBehaviour
{
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

            getOnBoatCanvas.gameObject.SetActive(false);
            ChangeCameraFocus.isSailing = true;
            boat.gameObject.SetActive(true);
            boatCamera.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

    }
}
