using UnityEngine;


//This script manage fake boat that let the player press e to sail
public class BoatController : MonoBehaviour
{
    public GameObject boat;
    public Camera boatCamera, getOnBoatCanvas;
    public int missionIndex;
    public bool ropeOnStatue = false;

    private bool isPlayerInTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (GameFlow.mission >= missionIndex || GameFlow.finishAllMissions))
        { // Ensure only the player triggers this
            isPlayerInTrigger = true;
            getOnBoatCanvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Player Exit: " + name);
            isPlayerInTrigger = false;
            getOnBoatCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown("e"))
        {
            print(name);
            getOnBoatCanvas.gameObject.SetActive(false);
            ChangeCameraFocus.isSailing = true;
            boat.gameObject.SetActive(true);
            boatCamera.gameObject.SetActive(true);
            gameObject.SetActive(false);

            if (ropeOnStatue)
            {
                StatueLimitation.shouldLimit = false;
            }
            isPlayerInTrigger = false;
        }

    }
}
