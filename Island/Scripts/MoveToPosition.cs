using UnityEngine;

public class MoveToPosiotion : MonoBehaviour
{
    public bool moveToStatue = false, moveToIsland = false, comingBack = false;
    public int workOnWayToIsland = 2;
    public static int onWayToIsland = 2;
    public GameObject boat;
    public Transform newBoatPosition;
    public GameObject followBoatCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (ChangeCameraFocus.isSailing && onWayToIsland == workOnWayToIsland)
        {
            if (moveToStatue)
            {
                boat.transform.SetPositionAndRotation(newBoatPosition.position, newBoatPosition.rotation);
                followBoatCamera.GetComponent<BoatCameraFollow>().lookOtherDirectionZ();
                boat.GetComponent<RotateBoat>().xyzRotation = 'z';
            }
            else if (moveToIsland)
            {
                boat.transform.SetPositionAndRotation(newBoatPosition.position, newBoatPosition.rotation);
                followBoatCamera.GetComponent<BoatCameraFollow>().lookOtherDirectionX();
                RotateBoat rotateBoat = boat.GetComponent<RotateBoat>();
                if (rotateBoat)
                {

                    if (comingBack)
                    {
                        rotateBoat.xyzRotation = 'x';
                        rotateBoat.goingBack = false;
                    }
                    else
                    {
                        rotateBoat.xyzRotation = 'z';
                        rotateBoat.goingBack = true;
                    }

                }

                comingBack = !comingBack;
                ChangeCameraFocus.goingBack = !ChangeCameraFocus.goingBack;
            }

            if (onWayToIsland == 1)
            {
                onWayToIsland = 2;
            }
            else
            {
                onWayToIsland = 1;
            }
        }

    }
}
