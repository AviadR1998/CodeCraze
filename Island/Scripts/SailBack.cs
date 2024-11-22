using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailBack : MonoBehaviour
{
    public Canvas sailBackCanvas;
    public Transform player, islandPosition;
    private bool canvasOn = false;


    private void OnTriggerEnter(Collider other)
    {
        sailBackCanvas.gameObject.SetActive(true);
        canvasOn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        sailBackCanvas.gameObject.SetActive(false);
        canvasOn = false;
    }

    private void Update()
    {
        if (canvasOn && Input.GetKeyDown("e"))
        {
            player.position = islandPosition.position;
            StatueLimitation.shouldLimit = false;
            ChangeCameraFocus.isSailing = true;
        }
    }
}
