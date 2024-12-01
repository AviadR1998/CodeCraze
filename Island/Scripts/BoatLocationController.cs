using UnityEngine;


//This script manage the invisable walls tho sets the player and boat location when sailing
public class BoatLocationController : MonoBehaviour
{
    public GameObject sailBoat, cameraScript, fakeBoat;
    public Transform newBoatPosition, player, newPlayerPosition;
    public bool workOnlyOnWayBack = false, statueBoat = false, moveToStatue = false;
    public static bool goingBack = false;
    private char yAxis = 'y', xAxis = 'x', zAxis = 'z';


    private void Start()
    {
        goingBack = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (ChangeCameraFocus.isSailing == true && workOnlyOnWayBack == goingBack)
        {
            sailBoat.transform.SetPositionAndRotation(newBoatPosition.position, newBoatPosition.rotation);
            sailBoat.SetActive(false);
            cameraScript.SetActive(false);
            fakeBoat.SetActive(true);

            RespawnPlayer();

            if (statueBoat)
            {
                cameraScript.GetComponent<BoatCameraFollow>().lookOtherDirectionZ();
                if (goingBack)
                {
                    sailBoat.GetComponent<RotateBoat>().xyzRotation = yAxis;
                }
                else
                {
                    sailBoat.GetComponent<RotateBoat>().xyzRotation = zAxis;
                }
            }
            else
            {
                cameraScript.GetComponent<BoatCameraFollow>().lookOtherDirectionX();
                if (goingBack)
                {
                    RotateBoat rotateBoat = sailBoat.GetComponent<RotateBoat>();
                    rotateBoat.xyzRotation = xAxis;
                    rotateBoat.goingBack = false;
                }
                else
                {
                    RotateBoat rotateBoat = sailBoat.GetComponent<RotateBoat>();
                    rotateBoat.xyzRotation = zAxis;
                    rotateBoat.goingBack = true;
                }
            }

            if (moveToStatue)
            {
                if (goingBack)
                {
                    StatueLimitation.shouldLimit = false;
                }
                else
                {
                    StatueLimitation.shouldLimit = true;
                }
            }

            goingBack = !goingBack;
            ChangeCameraFocus.isSailing = false;

        }
    }


    private void RespawnPlayer()
    {
        player.transform.SetPositionAndRotation(newPlayerPosition.position, newPlayerPosition.rotation);
    }
}
