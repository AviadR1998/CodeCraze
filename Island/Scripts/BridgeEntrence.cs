using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BridgeEntrence : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        RecursionMission rm = transform.parent.GetComponent<RecursionMission>();
        if (rm != null)
        {
            rm.TriggeredEntrence();
        }
    }
}
