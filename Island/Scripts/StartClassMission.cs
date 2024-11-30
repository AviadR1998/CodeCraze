using UnityEngine;


//This script manage the area that acticate class mission
public class StartClassMission : MonoBehaviour
{
    public static bool startMission = false;
    public GameObject classMissionObj;

    private void OnTriggerEnter(Collider other)
    {
        if (!startMission)
        {
            startMission = true;
            classMissionObj.GetComponent<ClassMission>().StartMission();
        }
        print("Triggerd!!!");
    }
}
