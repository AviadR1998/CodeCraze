using UnityEngine;


//This script manage the game flow of the mission in the island
public class GameFlow : MonoBehaviour
{
    public GameObject functionObject, classObject, recursionObject;
    public Canvas congratulations;
    public static int mission = 2;
    public static int stateInMission = 1;
    public int funcMissionIndex = 0, classMissionIndex = 1, recursionMissionIndex = 2;
    public static bool finishAllMissions = false;
    private int beforeQuestions = 0;


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
        if (mission == funcMissionIndex)
        {
            StartFunctionMission();
        }
        else if (mission == classMissionIndex)
        {
            StartClassMission();
        }
        else if (mission == recursionMissionIndex)
        {
            StartRecursionMission();
        }

    }

    private int GetMissionIndex()
    {
        switch (Login.task)
        {
            case "Functions":
                return funcMissionIndex;
            case "Classes":
                return classMissionIndex;
            case "Recursion":
                return recursionMissionIndex;
            default:
                return 0;
        }
    }


    private void StartFunctionMission()
    {
        if (stateInMission == beforeQuestions)
        {
            functionObject.SetActive(true);
            functionObject.GetComponent<FunctionMissionCoin>().InitMission();
        }
    }


    public void StartClassMission()
    {
        if (stateInMission == beforeQuestions)
        {
            classObject.SetActive(true);
        }
    }

    public void StartRecursionMission()
    {
        if (stateInMission == beforeQuestions)
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
        stateInMission = beforeQuestions;

        if (mission == classMissionIndex)
        {
            PauseMenu.updateSave("Island", "Classes", beforeQuestions);
            StartClassMission();
        }
        else if (mission == recursionMissionIndex)
        {
            PauseMenu.updateSave("Island", "Recursion", beforeQuestions);
            StartRecursionMission();
        }
        else
        {
            print("Finish All Change to free");
            if (!finishAllMissions)
            {
                finishAllMissions = true;
                GetComponent<SoundEffects>().PlaySoundClip();
                congratulations.gameObject.SetActive(true);
                FinishAll();
            }
            PauseMenu.updateSave("Free", "Functions", beforeQuestions);
            mission = beforeQuestions;
        }
    }

}
