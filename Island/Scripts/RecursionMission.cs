using UnityEngine;
using UnityEngine.UI;

public class RecursionMission : MonoBehaviour
{
    public GameObject player;
    public GameObject[] pears;
    public Transform playerRespawnPoint;
    public Button beforeMissionNextBtn, afterMissionNextBtn;
    public Canvas firstMissionCanvas, afterMissionCompleteCanvas, missionComplete;
    public GameObject BackgroundAudio, RecursionTree, arrow;
    public GameObject[] PearsToActivate, PearsToDeactivate, CheatPears;
    public float playerRespawnHeight = -1f, activateCanvasIn = 1f;
    private int dragonPears = 5, dragonPearsCollected = 0, beforeMissionNextCounter = 0, numOfClicksToStartMission = 13;
    private int afterMissionNextCounter = 0, numOfClicksToFinishMission = 3, initVals = 0;
    private bool collectAll = false, backToDragon = false, inMission = false;

    // Start is called before the first frame update
    void Start()
    {
        if (beforeMissionNextBtn != null)
        {
            beforeMissionNextBtn.onClick.AddListener(ClickNextFirstCanvas);
        }
        if (afterMissionNextBtn != null)
        {
            afterMissionNextBtn.onClick.AddListener(ClickNextSecondCanvas);
        }
    }

    private void ActivateFirstCanvas()
    {
        if (firstMissionCanvas != null)
        {
            firstMissionCanvas.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inMission)
        {
            if (player.transform.position.y < playerRespawnHeight)
            {
                player.transform.position = playerRespawnPoint.transform.position;
            }

            if (dragonPears == dragonPearsCollected)
            {
                foreach (GameObject pear in pears)
                {
                    pear.SetActive(true);
                }
                dragonPearsCollected++;
                collectAll = true;
            }

            if (beforeMissionNextCounter == numOfClicksToStartMission)
            {
                Invoke("CallToResumeCamera", 3f);
                beforeMissionNextCounter = initVals;
                TurnOnMissionMusic();
            }
            if (afterMissionNextCounter == numOfClicksToFinishMission)
            {
                Invoke("CallToResumeCamera", 1f);
                afterMissionNextCounter = initVals;
                TurnOffMissionMusic();
                PauseMenu.updateSave("Island", "Recursion", 1);
                inMission = false;
                StatueLimitation.shouldLimit = true;
                if (missionComplete != null)
                {
                    SoundEffects soundEffects = missionComplete.GetComponent<SoundEffects>();
                    soundEffects.PlaySoundClip();
                    missionComplete.gameObject.SetActive(true);
                }
                if (!GameFlow.finishAllMissions)
                {
                    gameObject.SetActive(false);
                    GameFlow.stateInMission = 1;
                }
                else
                {
                    arrow.SetActive(true);
                }

                StartRecursionMission.startMission = false;
            }

            if (Input.GetKeyDown("p"))
            {
                foreach (GameObject pear in CheatPears)
                {
                    pear.SetActive(true);
                }
            }
        }


    }

    private void CallToResumeCamera()
    {
        GetComponent<BlockPlayerCamera>().resumeCamera();
    }

    private void TurnOnMissionMusic()
    {
        if (BackgroundAudio != null)
        {
            BackgroundAudio.SetActive(false);
        }
        Transform missionAudio = transform.Find("RecursionBackMusic");
        if (missionAudio != null)
        {
            missionAudio.gameObject.SetActive(true);
        }
    }

    private void TurnOffMissionMusic()
    {
        Transform missionAudio = transform.Find("RecursionBackMusic");
        if (missionAudio != null)
        {
            missionAudio.gameObject.SetActive(false);
        }
        if (BackgroundAudio != null)
        {
            BackgroundAudio.SetActive(true);
        }
    }

    private void ClickNextFirstCanvas()
    {
        beforeMissionNextCounter++;
    }

    private void ClickNextSecondCanvas()
    {
        afterMissionNextCounter++;
    }

    public void CollectPear(Transform pear, bool zeroPears)
    {
        Transform grandFatherTransform = pear.parent.parent;
        PoleAndBridge pab = null;
        if (grandFatherTransform.name == "RopeNPole")
        {
            pab = grandFatherTransform.gameObject.GetComponent<PoleAndBridge>();
        }
        else
        {
            dragonPearsCollected++;
        }

        if (pab != null)
        {
            if (!zeroPears)
            {
                pab.PearCollected();
            }
            else
            {
                pab.CollectZeroPears();
            }

        }
    }

    public void TriggeredEntrence()
    {
        if (collectAll && !backToDragon)
        {
            backToDragon = true;

            MoveCamera moveCamera = GetComponent<MoveCamera>();

            if (moveCamera != null)
            {
                moveCamera.lockCameraAndMovePlayer();

                if (afterMissionCompleteCanvas != null)
                {
                    GetComponent<BlockPlayerCamera>().stopCamera();
                    afterMissionCompleteCanvas.gameObject.SetActive(true);
                }
            }
        }
    }

    public void StartMission()
    {
        inMission = true;
        StatueLimitation.shouldLimit = false;
        Invoke("ActivateFirstCanvas", activateCanvasIn);
        GetComponent<MoveCamera>().InitStartMission();
        dragonPearsCollected = initVals;
        beforeMissionNextCounter = initVals;
        afterMissionNextCounter = initVals;
        collectAll = false;
        backToDragon = false;

        foreach (GameObject pear in PearsToActivate)
        {
            pear.SetActive(true);
        }

        foreach (GameObject pear in PearsToDeactivate)
        {
            pear.SetActive(false);
        }
        RestorePoleAndBridge(RecursionTree.transform);
        arrow.SetActive(false);
    }


    private void RestorePoleAndBridge(Transform transform)
    {
        foreach (Transform child in transform)
        {
            if (child.name == "RopeNPole")
            {
                PoleAndBridge poleAndBridge = child.GetComponent<PoleAndBridge>();
                if (poleAndBridge != null)
                {
                    poleAndBridge.NumOfCollectedPears = initVals;
                    poleAndBridge.ZeroPearsCollected = false;
                    poleAndBridge.AllPearsCollected = false;
                }
                RestorePoleAndBridge(child);
            }
        }

    }

    private void OnEnable()
    {
        if (!arrow.activeInHierarchy)
        {
            arrow.SetActive(true);
        }
    }



}
