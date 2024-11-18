using UnityEngine;
using UnityEngine.UI;

public class RecursionMission : MonoBehaviour
{
    public GameObject player;
    public GameObject[] pears;
    public Transform playerRespawnPoint;
    public Button beforeMissionNextBtn, afterMissionNextBtn;
    public Canvas firstMissionCanvas, afterMissionCompleteCanvas, missionComplete;
    public GameObject BackgroundAudio;
    public float playerRespawnHeight = -1f;
    private int dragonPears = 5, dragonPearsCollected = 0, beforeMissionNextCounter = 0, numOfClicksToStartMission = 13;
    private int afterMissionNextCounter = 0, numOfClicksToFinishMission = 3;
    private bool collectAll = false, backToDragon = false;

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
        Invoke("ActivateFirstCanvas", 4f);
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
            beforeMissionNextCounter = 0;
            TurnOnMissionMusic();
        }
        if (afterMissionNextCounter == numOfClicksToFinishMission)
        {
            Invoke("CallToResumeCamera", 1f);
            afterMissionNextCounter = 0;
            TurnOffMissionMusic();
            if (missionComplete != null)
            {
                SoundEffects soundEffects = missionComplete.GetComponent<SoundEffects>();
                soundEffects.PlaySoundClip();
                missionComplete.gameObject.SetActive(true);
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
            Transform startMissionObject = transform.Find("StartMission");

            if (startMissionObject != null)
            {
                MoveCamera moveCamera = startMissionObject.GetComponent<MoveCamera>();

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
    }



}
