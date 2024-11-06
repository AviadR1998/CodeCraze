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
    public Button saveTorchBtn; // Reference to the "yes" button
    public Button saveMatchBtn; // Reference to the "yes" button
    public Button startTheMissionBtn;
    public TextMeshProUGUI textTorch;
    public TextMeshProUGUI textMatch;
    public TextMeshProUGUI textLitMatch;
    public TextMeshProUGUI textNumOfFunCalls;
    public GameObject youCollectedCanvas;
    public Button playBtn;

    private int numOfTorches, numOfMatches;
    private bool isStart = false, endMission = false;
    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.AddListener(StartCanvases);
        saveTorchBtn.onClick.AddListener(AddTorch);
        saveMatchBtn.onClick.AddListener(AddMatch);
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
        }
        else if (endMission)
        {
            textNumOfFunCalls.text = functionCalls.ToString();
            endOfMissionCanvas.SetActive(true);
        }
        else
        {
            youCollectedCanvas.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //Not sure if needed
        //startCanvas.SetActive(false);
    }

    private void StartCanvases()
    {
        startCanvas.SetActive(false);
        missionCanvas1.SetActive(true);
    }

    private void StartMission()
    {
        isStart = true;
        return;
    }


    private void AddTorch()
    {
        numOfTorches++;
        print("num of torches: " + numOfTorches);
    }

    private void AddMatch()
    {
        numOfMatches++;
        print("num of matches: " + numOfMatches);
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
