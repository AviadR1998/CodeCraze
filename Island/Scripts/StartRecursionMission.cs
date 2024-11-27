using UnityEngine;

public class StartRecursionMission : MonoBehaviour
{
    public static bool startMission = false;
    public GameObject recursionMissionObj;

    private void OnTriggerEnter(Collider other)
    {
        if (!startMission)
        {
            startMission = true;
            recursionMissionObj.GetComponent<RecursionMission>().StartMission();
        }
        print("Triggerd!!!");
    }
}
