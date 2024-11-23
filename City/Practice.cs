using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Practice : MonoBehaviour
{
    public static List<GameObject> nextMission;
    public static string taskName = "";
    public static bool canAsk;
    public GameObject questionObj;
    public GameObject player;
    public GameObject arrow;

    Canvas retCanvas;
    bool activeCanvas;
    // Start is called before the first frame update
    void Start()
    {
        nextMission = new List<GameObject>();
    }

    void OnEnable()
    {
        canAsk = activeCanvas = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canAsk)
        {
            print("Enter");
            retCanvas = questionObj.GetComponent<CreateQuestionsCanvas>().CreateQCanvas(taskName + "CSV.csv");
            Cursor.lockState = CursorLockMode.Confined;
            activeCanvas = Cursor.visible = true;
            PauseMenu.canPause = canAsk = player.GetComponent<Movement>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (retCanvas != null && activeCanvas && !retCanvas.gameObject.active)
        {
            Cursor.lockState = CursorLockMode.Locked;
            canAsk = activeCanvas = Cursor.visible = false;
            PauseMenu.canPause = player.GetComponent<Movement>().enabled = true;
            if (nextMission != null)
            {
                if (nextMission[0] != null)
                {
                    AdminMission.currentSubMission++;
                    PauseMenu.updateSave("City", "For", AdminMission.currentSubMission);
                    Movement.mission = nextMission[0];
                    nextMission[0].SetActive(true);
                    AdminMission.canTalk = true;
                    if (nextMission[0].name == "PracticeNPC")
                    {
                        taskName = "dowhile";
                        canAsk = true;
                    }
                    if (nextMission[0].name != "ForNPC" && nextMission[0].name != "PracticeNPC")
                    { 
                        Movement.npcMissionCounter++;
                        Movement.missionInProgress = "";
                    }
                }
            }
            else
            {
                arrow.SetActive(false);
            }
            if (taskName != "array")
            {
                nextMission.Remove(nextMission[0]);
                if (taskName != "for")
                {
                    Movement.missionInProgress = "";
                }
            }
            else
            {
                Movement.missionInProgress = "";
            }
        }
    }
}
