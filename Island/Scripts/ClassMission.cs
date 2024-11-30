using UnityEngine;
using UnityEngine.UI;


//This script manage the class mission
public class ClassMission : MonoBehaviour
{
    public Transform npc, startLocation;
    public Camera mainCamera;
    public Transform player;
    public Button nextBtn, freeCameraBtn;
    public Canvas finishMissionCanvas, missionCompletedCanvas, firstMissionCanvas;
    public GameObject arrow;
    public GameObject[] dogBeds;
    public float MoveToNpcIn = 1f, lookAtYExtra = 0.5f;
    public int numOfClicksToMove = 3, numOfNeededDog = 7, initVal = 0;
    private int ClicksNextInCanvasCnt = 0, freeCameraClickCnt = 0, dogsCreated = 0, nextBtnOnFinishCanvasCnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        nextBtn.onClick.AddListener(ClickedNextInDogsCanvas);
        freeCameraBtn.onClick.AddListener(FreeCamera);
        Button[] buttons = finishMissionCanvas.GetComponentsInChildren<Button>();
        foreach (Button btn in buttons)
        {
            if (btn.name == "NextBtn")
            {
                btn.onClick.AddListener(ClickedNextInFinishCanvas);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dogsCreated == numOfNeededDog)
        {
            dogsCreated++;

            this.GetComponent<BlockPlayerCamera>().stopCamera();
            Invoke("MoveToNpc", MoveToNpcIn);
        }

    }

    private void ActivateFirstCanvas()
    {
        if (firstMissionCanvas != null)
        {
            firstMissionCanvas.gameObject.SetActive(true);
        }
    }

    private void MoveToNpc()
    {
        player.transform.position = startLocation.position;
        player.transform.rotation = startLocation.rotation;
        player.LookAt(new Vector3(npc.position.x, player.position.y + lookAtYExtra, npc.position.z));
        mainCamera.transform.LookAt(new Vector3(npc.position.x, player.position.y + lookAtYExtra, npc.position.z));
        finishMissionCanvas.gameObject.SetActive(true);
    }

    public void AddDog()
    {
        dogsCreated++;
    }

    public void ClickedNextInDogsCanvas()
    {
        if (++ClicksNextInCanvasCnt == numOfClicksToMove)
        {
            nextBtn.gameObject.SetActive(false);
        }
    }

    public void ClickedNextInFinishCanvas()
    {
        if (++nextBtnOnFinishCanvasCnt == numOfClicksToMove)
        {
            GetComponent<BlockPlayerCamera>().resumeCamera();
            missionCompletedCanvas.gameObject.SetActive(true);
            GetComponent<SoundEffects>().PlaySoundClip();
            StartClassMission.startMission = false;

            if (!GameFlow.finishAllMissions)
            {
                gameObject.SetActive(false);
            }
            else
            {
                arrow.SetActive(true);
            }

            GameFlow.stateInMission = 1;
            PauseMenu.updateSave("Island", "Classes", 1);
        }
    }




    private void FreeCamera()
    {
        if (++freeCameraClickCnt == numOfClicksToMove)
        {
            this.GetComponent<BlockPlayerCamera>().resumeCamera();

            DogAnimationController[] dacs = GetComponentsInChildren<DogAnimationController>();
            DogAnimationController.canBarkWhenClicked = false;
        }
    }

    public void ClickDog()
    {
        if (!nextBtn.gameObject.activeSelf)
        {
            nextBtn.gameObject.SetActive(true);
        }
    }

    public void StartMission()
    {
        npc.GetComponent<MoveCamera>().InitStartMission();
        Invoke("ActivateFirstCanvas", MoveToNpcIn);
        ClicksNextInCanvasCnt = initVal;
        freeCameraClickCnt = initVal;
        dogsCreated = initVal;
        nextBtnOnFinishCanvasCnt = initVal;
        DogAnimationController.canBarkWhenClicked = true;

        foreach (GameObject dogBed in dogBeds)
        {
            dogBed.GetComponent<DogBed>().RestartBed();
        }

        arrow.SetActive(false);
    }

    private void OnEnable()
    {
        if (!arrow.activeInHierarchy)
        {
            arrow.SetActive(true);
        }
    }


}
