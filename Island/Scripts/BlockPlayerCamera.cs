using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
