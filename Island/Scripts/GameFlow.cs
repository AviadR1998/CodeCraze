using System;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public GameObject functionObject, classObject, recursionObject;
    public static int mission = 0;
    public static int stateInMission = 1;
    public static bool finishAllMissions = true;
    // Start is called before the first frame update
    void Start()
    {
        if (mission == 0)
        {
            StartFunctionMission();
        }
        else if (mission == 1)
        {
            StartClassMission();
        }
        else if (mission == 2)
        {
            StartRecursionMission();
        }

    }

    // Update is called once per frame
    void Update()
    {
        // if (functionObject.activeInHierarchy == false)
        // {
        //     StartFunctionMission();
        // }

    }

    private void StartFunctionMission()
    {
        if (stateInMission == 0)
        {
            functionObject.SetActive(true);
            functionObject.GetComponent<FunctionMissionCoin>().InitMission();
        }
    }


    public void StartClassMission()
    {
        if (stateInMission == 0)
        {
            classObject.SetActive(true);
        }
    }

    public void StartRecursionMission()
    {
        if (stateInMission == 0)
        {
            recursionObject.SetActive(true);
        }
    }

    private void FinishAll()
    {
        classObject.SetActive(true);
    }

    public void FinishedAMission()
    {
        mission++;
        stateInMission = 0;

        if (mission == 1)
        {
            StartClassMission();
        }
        else if (mission == 2)
        {
            StartRecursionMission();
        }
        else
        {
            finishAllMissions = true;
        }
    }

}
