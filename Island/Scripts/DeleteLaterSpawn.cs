using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteLaterSpawn : MonoBehaviour
{
    public GameObject player;
    public Transform newPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.transform.position = newPos.position;
        }
    }
}
