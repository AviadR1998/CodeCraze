using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosiotion : MonoBehaviour
{
    private bool isPlayerInTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { // Ensure only the player triggers this
            isPlayerInTrigger = true;
            print("Passed Me!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private void Update()
    {

    }
}
