using UnityEngine;
using UnityEngine.UI;

public class ClassMission : MonoBehaviour
{
    public Transform npc, startLocation;
    public Camera mainCamera;
    public Transform player;
    public Button nextBtn, freeCameraBtn;
    public Canvas finishMissionCanvas, missionCompletedCanvas, firstMissionCanvas;
    public GameObject arrow;
    public GameObject[] dogBeds;
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
        if (dogsCreated == 7)
        {
            dogsCreated++;

            this.GetComponent<BlockPlayerCamera>().stopCamera();
            Invoke("MoveToNpc", 1f);
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
        player.LookAt(new Vector3(npc.position.x, player.position.y + 0.5f, npc.position.z));
        mainCamera.transform.LookAt(new Vector3(npc.position.x, player.position.y + 0.5f, npc.position.z));
        finishMissionCanvas.gameObject.SetActive(true);
    }

    public void AddDog()
    {
        dogsCreated++;
    }

    public void ClickedNextInDogsCanvas()
    {
        if (++ClicksNextInCanvasCnt == 3)
        {
            nextBtn.gameObject.SetActive(false);
        }
    }

    public void ClickedNextInFinishCanvas()
    {
        if (++nextBtnOnFinishCanvasCnt == 3)
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
        if (++freeCameraClickCnt == 3)
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
        Invoke("ActivateFirstCanvas", 1f);
        ClicksNextInCanvasCnt = 0;
        freeCameraClickCnt = 0;
        dogsCreated = 0;
        nextBtnOnFinishCanvasCnt = 0;
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
