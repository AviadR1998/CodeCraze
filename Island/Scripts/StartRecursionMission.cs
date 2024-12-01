using UnityEngine;


//This script manage the area that acticate recursion mission
public class StartRecursionMission : MonoBehaviour
{
    public static bool startMission = false;
    public GameObject recursionMissionObj;
    public int beforeQuestions = 0;

    private void Start()
    {
        startMission = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!startMission && (GameFlow.finishAllMissions == true || GameFlow.stateInMission == beforeQuestions))
        {
            startMission = true;
            recursionMissionObj.GetComponent<RecursionMission>().StartMission();
        }
    }
}
