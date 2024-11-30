using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//This script manage the floating coin that controls the fumction mission
public class FunctionMissionCoin : MonoBehaviour
{
    private int functionCalls;
    public GameObject startCanvas, missionCanvas1, endOfMissionCanvas, youCollectedCanvas;
    public Button startBtn, startTheMissionBtn, playBtn, saveFinish;
    public TextMeshProUGUI textTorch;
    public TextMeshProUGUI textMatch;
    public TextMeshProUGUI textLitMatch;
    public TextMeshProUGUI textNumOfFunCalls;
    public GameObject TurnOffTorchesParent, InBoxTurnOffTorches, InBoxTurnOnTorches, InBoxMatches, OutsideMatches;
    public int initNumOfCollected = 0;

    private int numOfTorches, numOfMatches;
    private bool isStart = false, finishInstruction = false, endMission = false, initMission = false;


    private void Start()
    {
        startBtn.onClick.AddListener(StartCanvases);
        startTheMissionBtn.onClick.AddListener(StartMission);
        playBtn.onClick.AddListener(CreateLitTorches);
        saveFinish.onClick.AddListener(SavedFinishMission);
    }
    // Update is called once per frame
    void Update()
    {
        textTorch.text = numOfTorches.ToString();
        textMatch.text = numOfMatches.ToString();
        textLitMatch.text = (Math.Min(numOfTorches, numOfMatches / 3)).ToString();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (initMission)
        {
            if (!isStart)
            {
                //Start mission Canvas
                startCanvas.SetActive(true);
                isStart = true;
            }
            else if (endMission)
            {
                if (!endOfMissionCanvas.activeInHierarchy)
                {
                    textNumOfFunCalls.text = functionCalls.ToString();
                    endOfMissionCanvas.SetActive(true);
                }

            }
            else if (finishInstruction)
            {
                if (!youCollectedCanvas.activeInHierarchy)
                {
                    youCollectedCanvas.SetActive(true);
                }
            }
        }


    }

    public void InitMission()
    {
        initMission = true;
        numOfTorches = initNumOfCollected;
        numOfMatches = initNumOfCollected;
        functionCalls = initNumOfCollected;
        isStart = false;
        finishInstruction = false;
        endMission = false;
    }

    private void StartCanvases()
    {
        startCanvas.SetActive(false);
        missionCanvas1.SetActive(true);
    }

    private void StartMission()
    {
        isStart = true;
        finishInstruction = true;

        TurnOnOrOffAllChildren(TurnOffTorchesParent, true);
        TurnOnOrOffAllChildren(OutsideMatches, true);
        return;
    }

    private void TurnOnOrOffAllChildren(GameObject parent, bool turnTo)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            child.gameObject.SetActive(turnTo);
        }
    }


    public void AddTorch()
    {
        numOfTorches++;
    }

    public void AddMatch()
    {
        numOfMatches++;
    }

    private void CreateLitTorches()
    {
        functionCalls++;

        Transform parentTransform = GameObject.Find("TurnOnTorches").transform;
        int numOfLitTorches = Math.Min(numOfTorches, numOfMatches / 3);
        for (int i = 1; i <= numOfLitTorches; ++i)
        {
            GameObject litTorch = parentTransform.Find("LitTorch" + i.ToString()).gameObject;
            litTorch.SetActive(true);
        }

        if (numOfLitTorches >= 3)
        {
            endMission = true;
        }
        youCollectedCanvas.gameObject.SetActive(false);


    }

    public void SavedFinishMission()
    {
        TurnOnOrOffAllChildren(InBoxTurnOffTorches, false);
        TurnOnOrOffAllChildren(InBoxMatches, false);
        TurnOnOrOffAllChildren(InBoxTurnOnTorches, false);
        TurnOnOrOffAllChildren(OutsideMatches, false);
        TurnOnOrOffAllChildren(TurnOffTorchesParent, false);

        GameFlow.stateInMission = 1;
        if (!GameFlow.finishAllMissions)
        {
            gameObject.SetActive(false);
            PauseMenu.updateSave("Island", "Functions", 1);
            return;
        }

        InitMission();

    }
}
