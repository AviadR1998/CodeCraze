using UnityEngine;

public class BoatLocationController : MonoBehaviour
{
    public GameObject sailBoat, cameraScript, fakeBoat;
    public Transform newBoatPosition, player, newPlayerPosition;
    public bool workOnlyOnWayBack = false, statueBoat = false;
    public static bool goingBack = false;

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
                    sailBoat.GetComponent<RotateBoat>().xyzRotation = 'y';
                }
                else
                {
                    sailBoat.GetComponent<RotateBoat>().xyzRotation = 'z';
                }
            }
            else
            {
                cameraScript.GetComponent<BoatCameraFollow>().lookOtherDirectionX();
                if (goingBack)
                {
                    RotateBoat rotateBoat = sailBoat.GetComponent<RotateBoat>();
                    rotateBoat.xyzRotation = 'x';
                    rotateBoat.goingBack = false;
                }
                else
                {
                    RotateBoat rotateBoat = sailBoat.GetComponent<RotateBoat>();
                    rotateBoat.xyzRotation = 'z';
                    rotateBoat.goingBack = true;
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
