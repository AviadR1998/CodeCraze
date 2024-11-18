using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassMission : MonoBehaviour
{
    public Transform npc;
    public Transform startLocation;
    public Camera mainCamera;
    public Transform player;
    public Button nextBtn, freeCameraBtn;
    public Canvas finishMissionCanvas, missionCompletedCanvas, firstMissionCanvas;
    private int ClicksNextInCanvasCnt = 0, freeCameraClickCnt = 0, dogsCreated = 0, nextBtnOnFinishCanvasCnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Invoke("initStartMission", 4f);
        nextBtn.onClick.AddListener(ClickedNextInDogsCanvas);
        freeCameraBtn.onClick.AddListener(FreeCamera);
        npc.gameObject.SetActive(true);
        Invoke("ActivateFirstCanvas", 4f);

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
            print("7 Dog Created");
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

    // public void initStartMission()
    // {
    //     this.GetComponent<blockPlayerCamera>().stopCamera();
    //     Invoke("startMission", 1f);
    // }

    // public void startMission()
    // {
    //     player.transform.position = startLocation.position;
    //     player.transform.rotation = startLocation.rotation;
    //     player.LookAt(new Vector3(npc.position.x, player.position.y + 0.5f, npc.position.z));
    //     mainCamera.transform.LookAt(new Vector3(npc.position.x, player.position.y + 0.5f, npc.position.z));
    // }

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
            this.GetComponent<BlockPlayerCamera>().resumeCamera();
            missionCompletedCanvas.gameObject.SetActive(true);
            GetComponent<SoundEffects>().PlaySoundClip();
        }
    }




    private void FreeCamera()
    {
        if (++freeCameraClickCnt == 3)
        {
            this.GetComponent<BlockPlayerCamera>().resumeCamera();

            DogAnimationController[] dacs = GetComponentsInChildren<DogAnimationController>();
            foreach (DogAnimationController dac in dacs)
            {
                dac.MakeCanClickOnDogFalse();
            }
        }
    }

    public void ClickDog()
    {
        if (!nextBtn.gameObject.activeSelf)
        {
            nextBtn.gameObject.SetActive(true);
        }
    }


}
