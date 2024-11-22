using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FunctionMissionCoin : MonoBehaviour
{
    private int functionCalls;
    public Button startBtn;
    public GameObject startCanvas, missionCanvas1, endOfMissionCanvas;
    public Button startTheMissionBtn;
    public TextMeshProUGUI textTorch;
    public TextMeshProUGUI textMatch;
    public TextMeshProUGUI textLitMatch;
    public TextMeshProUGUI textNumOfFunCalls;
    public GameObject youCollectedCanvas;
    public Button playBtn;

    private int numOfTorches, numOfMatches;
    private bool isStart = false, finishInstruction = false, endMission = false;
    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.AddListener(StartCanvases);
        startTheMissionBtn.onClick.AddListener(StartMission);
        playBtn.onClick.AddListener(CreateLitTorches);
        numOfTorches = 0;
        numOfMatches = 0;
        functionCalls = 0;
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

    private void StartCanvases()
    {
        startCanvas.SetActive(false);
        missionCanvas1.SetActive(true);
    }

    private void StartMission()
    {
        isStart = true;
        finishInstruction = true;
        return;
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

    }
}
