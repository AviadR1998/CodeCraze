using UnityEngine;

//This script manage the player camera
public class BlockPlayerCamera : MonoBehaviour
{
    public MonoBehaviour playerControlScript;


    public void stopCamera()
    {
        playerControlScript.enabled = false;
    }

    public void resumeCamera()
    {
        playerControlScript.enabled = true;
    }
}
