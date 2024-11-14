using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Practice : MonoBehaviour
{
    public static List<GameObject> nextMission;
    public static bool canAsk;
    public GameObject questionPopUp;
    public GameObject player;
    public GameObject arrow;

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
            questionPopUp.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            activeCanvas = Cursor.visible = true;
            player.GetComponent<Movement>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!questionPopUp.active && activeCanvas)
        {
            Cursor.lockState = CursorLockMode.Locked;
            canAsk = activeCanvas = Cursor.visible = false;
            player.GetComponent<Movement>().enabled = true;
            if (nextMission != null)
            {
                if (nextMission[0] != null)
                {
                    AdminMission.currentSubMission++;
                    Movement.mission = nextMission[0];
                    nextMission[0].SetActive(true);
                    AdminMission.canTalk = true;
                    if (nextMission[0].name != "ForNPC" || nextMission[0].name != "ForNPC")
                    {
                        Movement.npcMissionCounter++;
                    }
                }
            }
            else
            {
                arrow.SetActive(false);
            }
            nextMission.Remove(nextMission[0]);/// was bug here
        }
    }
}
