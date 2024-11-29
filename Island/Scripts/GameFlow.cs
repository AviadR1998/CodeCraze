using System;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public GameObject functionObject, classObject, recursionObject;
    public Canvas congratulations;
    public static int mission = 2;
    public static int stateInMission = 1;
    public static bool finishAllMissions = false;


    void Start()
    {
        print(Login.world + " " + Login.task + " " + Login.state);
        mission = GetMissionIndex();
        stateInMission = Login.state;
        print("Mission: " + mission);
        if (Login.world == "Free")
        {
            finishAllMissions = true;
            FinishAll();
            return;
        }
        if (mission == 0)
        {
            StartFunctionMission();
        }
        else if (mission == 1)
        {
            StartClassMission();
            print("Start class Mission");
        }
        else if (mission == 2)
        {
            StartRecursionMission();
        }

    }

    private int GetMissionIndex()
    {
        switch (Login.task)
        {
            case "Functions":
                return 0;
            case "Classes":
                return 1;
            case "Recursion":
                return 2;
            default:
                return 0;
        }
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
        functionObject.SetActive(true);
        functionObject.GetComponent<FunctionMissionCoin>().InitMission();
        classObject.SetActive(true);
        recursionObject.SetActive(true);
    }

    public void FinishedAMission()
    {
        mission++;
        stateInMission = 0;

        if (mission == 1)
        {
            PauseMenu.updateSave("Island", "Functions", 0);
            StartClassMission();
        }
        else if (mission == 2)
        {
            PauseMenu.updateSave("Island", "Functions", 0);
            StartRecursionMission();
        }
        else
        {
            if (!finishAllMissions)
            {
                finishAllMissions = true;
                GetComponent<SoundEffects>().PlaySoundClip();
                congratulations.gameObject.SetActive(true);
            }
            PauseMenu.updateSave("Free", "Functions", 0);
            mission = 0;
        }
    }

}
