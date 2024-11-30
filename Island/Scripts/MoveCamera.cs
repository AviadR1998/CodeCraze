using UnityEngine;
using UnityEngine.UI;


//This script manage the camera movement of a mission and change its look at position possinly twice
public class MoveCamera : MonoBehaviour
{
    public Transform npc, startLocation, player, lookHere, lookHere2;
    public Camera mainCamera;
    public Button nextBtn1, nextBtn2;
    public int numOfClicksToStartRotate1 = 1, numOfClicksToStartRotate2 = 1;
    public float rotationSpeed = 2f; // Adjust for smoother or faster rotation
    public float stopThreshold = 1f; // Threshold angle to stop rotating (in degrees)
    public float startLockIn = 1f;
    public int initNumOfClicks = 0;
    public float extraYAxis = 0.5f;
    private bool isSmoosh1 = false, isSmoosh2 = false;
    private int numOfClicks1 = 0, numOfClicks2 = 0;



    // Start is called before the first frame update
    void Start()
    {
        if (nextBtn1 != null)
        {
            nextBtn1.onClick.AddListener(startSmooshLookAt);
        }

        if (nextBtn2 != null)
        {
            nextBtn2.onClick.AddListener(startSmooshLookAt2);
        }


    }

    public void InitStartMission()
    {
        GetComponent<BlockPlayerCamera>().stopCamera();
        Invoke("lockCameraAndMovePlayer", startLockIn);
        numOfClicks1 = initNumOfClicks;
        numOfClicks2 = initNumOfClicks;
        isSmoosh1 = false;
        isSmoosh2 = false;
    }

    public void lockCameraAndMovePlayer()
    {
        player.transform.position = startLocation.position;
        player.transform.rotation = startLocation.rotation;
        player.LookAt(new Vector3(npc.position.x, player.position.y + extraYAxis, npc.position.z));
        mainCamera.transform.LookAt(new Vector3(npc.position.x, player.position.y + extraYAxis, npc.position.z));
    }

    public void startSmooshLookAt()
    {
        if (++numOfClicks1 == numOfClicksToStartRotate1)
        {
            isSmoosh1 = true;
        }
    }

    public void startSmooshLookAt2()
    {
        if (++numOfClicks2 == numOfClicksToStartRotate2)
        {
            isSmoosh2 = true;
        }
    }

    void Update()
    {
        if (isSmoosh1)
        {
            SmoothLookAtPoint(lookHere.position, 1);
        }
        else if (isSmoosh2)
        {
            SmoothLookAtPoint(lookHere2.position, 2);
        }

    }

    void SmoothLookAtPoint(Vector3 targetPoint, int smooshIndex)
    {
        // Calculate the direction to look at
        Vector3 direction = targetPoint - player.position;
        //direction.y = 0; // Keep the look direction level if desired

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Check if the angle between current and target rotation is within the threshold
        if (Quaternion.Angle(player.rotation, targetRotation) > stopThreshold)
        {
            // Smoothly rotate towards the target rotation
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Snap to the final rotation if within threshold
            player.rotation = targetRotation;
            if (smooshIndex == 1)
            {
                isSmoosh1 = false;
            }
            else
            {
                isSmoosh2 = false;
            }
        }
    }


}
