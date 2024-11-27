using UnityEngine;

public class BridgeEntrence : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        RecursionMission rm = transform.parent.GetComponent<RecursionMission>();
        print("In function");
        if (rm != null)
        {
            print("Triggered Function");
            rm.TriggeredEntrence();
        }
    }
}
