using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using SocketIOClient;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 10f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    public static int npcMissionCounter = 0;
    public static bool canMove = true, soccer = false, home = false, loadOnce = true;
    CharacterController characterController;
    public AudioSource soccesSound;
    public AudioSource backgrounMusic;

    public static bool raceOn;
    public GameObject welcomeCanvas;
    public GameObject player;
    public GameObject roomsMenu;
    public GameObject arrow;
    public GameObject findPanel;
    public GameObject[] npcs;
    public static GameObject mission;
    public GameObject orderPanel;
    public TMP_Text objFoundHotCold;
    float arrowSpeed = 6.2f;
    bool canPress;
    GameObject ballBox;
    static public char currentLetter = '\0';
    static public bool hoverBall, hoverLetter;
    public static string missionInProgress;
    int cheatTransfer = 0, addingToArrow = 0;
    Vector3[] cheatArr = new Vector3[3] {new Vector3(-1069.69f, 13.57452f, 340.8305f), new Vector3(-720, 14, 273), new Vector3(-951.55f, 15.18f, 306.67f)};
    Dictionary<string, int> topiToNumbers = new Dictionary<string, int> { { "If", 0 }, { "While", 1 }, { "For", 2 }, { "Array", 3 } };

    const int DOWN_ARROW = 50, DELAY = 2, UP_FIND_OBJ = 12, CNT_OBJ = 3, BOX_LEN = 7, NPC_MAX = 3, ARROW_FACROR_Y = 5, CHEAT_TRANSFER = 3;
    const float RUN_ARROW = 12.2f, WALK_ARROW = 6.2f, DELAY_PRESS = 0.5f, PLAYER_X = -866.33f, PLAYER_Y = 13.015f, PLAYER_Z = 94.84f, ARROW_ADD_Y = 0.7f, HOVER_Y = 9.834f, HOVER_ADD_Y = 0.2f, HOVER_ADD_BALL_Y = 0.4f, HOVER_DOWN_LETTER_Y = 0.3f;
    // Start is called before the first frame update
    void Start()
    {

        missionInProgress = "";
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        hoverLetter = hoverBall = Cursor.visible = false;
        PauseMenu.canPause = true;
        canPress = true;
        backgrounMusic.Play();
    }

    private void OnEnable()
    {
        backgrounMusic.Play();
        raceOn = false;
        if (Login.world != "City")
        {
            if (loadOnce)
            {
                welcomeCanvas.SetActive(true);
                loadOnce = false;
            }
            addingToArrow = DOWN_ARROW;
            arrow.transform.position -= new Vector3(0, DOWN_ARROW, 0);
            for (int i = 0; i < npcs.Length; i++)
            {
                npcs[i].SetActive(true);
            }
        }
        else
        {
            if (!loadOnce)
            {
                return;
            }
            loadOnce = false;
            mission = player;
            if (Login.task != "If")
            {
                npcs[0].SetActive(false);
            }
            else
            {
                if (Login.state == 0)
                {
                    welcomeCanvas.SetActive(true);
                }
            }
            npcMissionCounter = topiToNumbers[Login.task];
            npcs[npcMissionCounter].SetActive(true);
            mission = npcs[npcMissionCounter];
            if (Login.task == "If" && Login.state == 1)
            {
                IfMissions.startFromQuestions = true;
            }
            if (Login.task == "While" && Login.state == 1)
            {
                WhileMissions.startFromQuestions = true;
            }
            if (Login.task == "For" && Login.state == 1)
            {
                ForMission.startFromQuestions = true;
            }
            if (Login.task == "For" && Login.state == 2)
            {
                ForMission.startFromAfterQuestions = true;
            }
            if (Login.task == "Array" && Login.state == 1)
            {
                BoxGame.startFromQuestions = true;
            }
            if (Login.state == 0)
            {
                Practice.canAsk = false;
            }
        }
    }

    private IEnumerator closeFindPanel()
    {
        yield return new WaitForSeconds(DELAY);
        findPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FindObj")
        {
            IfMissions.findObj[IfMissions.currentFindObj].transform.position += new Vector3(0, UP_FIND_OBJ, 0);
            IfMissions.findObj[IfMissions.currentFindObj].SetActive(false);
            IfMissions.currentFindObj++;
            if(IfMissions.currentFindObj < CNT_OBJ) 
            {
                IfMissions.findObj[IfMissions.currentFindObj].SetActive(true);
                IfMissions.findObj[IfMissions.currentFindObj].transform.position -= new Vector3(0, UP_FIND_OBJ, 0);
            }
            objFoundHotCold.text = IfMissions.currentFindObj + "/3";
            findPanel.SetActive(true);
            soccesSound.Play();
            StartCoroutine(closeFindPanel());
        }
        if (other.tag == "RaceNPC")
        {
            roomsMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            PauseMenu.canPause = false;
            raceOn = true;
        }
    }

    private IEnumerator delayPress()
    {
        yield return new WaitForSeconds(DELAY_PRESS);
        canPress = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "BallBox" || other.tag == "BoxArr" || other.tag == "LetterBox")
        {
            orderPanel.SetActive(true);
        }
        if (other.tag == "BallBox" && canPress && Input.GetKeyDown("e"))
        {
            if (!hoverBall)
            {
                canPress = false;
                hoverBall = true;
                ballBox = other.gameObject;
            }
            StartCoroutine(delayPress());
        }
        if (other.tag == "BoxArr" && canPress && Input.GetKeyDown("e") && hoverBall)
        {
            BoxGame.boxNumbers.Add(other.name);
            canPress = false;
            hoverBall = false;
            ballBox.transform.position = other.transform.position;
            StartCoroutine(delayPress());
        }
        if (other.tag == "LetterBox" && canPress && Input.GetKeyDown("e"))
        {
            if (!hoverLetter)
            {
                canPress = false;
                hoverLetter = true;
                ballBox = other.gameObject;
                currentLetter = other.name[0];
            }
            StartCoroutine(delayPress());
        }
        if (other.tag == "BoxArr" && canPress && Input.GetKeyDown("e") && hoverLetter)
        {
            BoxGame.boxLetters[other.name[BOX_LEN - 1] - '0'] = currentLetter;
            canPress = false;
            hoverLetter = false;
            ballBox.transform.position = other.transform.position;
            StartCoroutine(delayPress());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        orderPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (raceOn)
        {
            backgrounMusic.Pause();
            return;
        }
        if (!backgrounMusic.isPlaying)
        {
            backgrounMusic.Play();
        }
        if (soccer || PauseMenu.isPaused || EndWorldScript.activated)
        {
            return;
        }
        if (Login.world == "City" && Input.GetKeyDown(KeyCode.Tab) && npcMissionCounter < NPC_MAX)
        {
            Practice.canAsk = false;
            npcs[npcMissionCounter++].SetActive(false);
            mission = npcs[npcMissionCounter];
            npcs[npcMissionCounter].SetActive(true);
        }
        if (home)
        {
            player.transform.position = new Vector3(PLAYER_X, PLAYER_Y, PLAYER_Z);
            arrow.transform.position = new Vector3(PLAYER_X, PLAYER_Y - addingToArrow, PLAYER_Z);
            home = false;
            return;
        }
        if (arrow.active)
        {
            arrow.transform.position = Vector3.MoveTowards(arrow.transform.position,
            GameObject.Find("Player").transform.position + new Vector3(
            playerCamera.transform.forward.x * 2,
            math.sin(playerCamera.transform.forward.y) * ARROW_FACROR_Y + ARROW_ADD_Y - addingToArrow,
            playerCamera.transform.forward.z * 2), arrowSpeed * Time.deltaTime);
            arrow.transform.LookAt(mission.transform);
        }

        if (Input.GetKeyDown("z"))
        {
            player.GetComponent<CharacterController>().enabled = false;
            cheatTransfer = (cheatTransfer + 1) % CHEAT_TRANSFER;
            player.transform.position = cheatArr[cheatTransfer];
            player.GetComponent<CharacterController>().enabled = true;
        }

        if (canPress && Input.GetKeyDown("e") && (hoverBall || hoverLetter))
        {
            ballBox.transform.position = new Vector3(player.transform.position.x, HOVER_Y + (hoverLetter ? HOVER_ADD_Y : 0), player.transform.position.z);
            hoverLetter = canPress = hoverBall = false;
            StartCoroutine(delayPress());
        }

        if(hoverBall)
        {
            ballBox.transform.position = GameObject.Find("Player").transform.position + new Vector3(
            playerCamera.transform.forward.x * 1,
            math.sin(playerCamera.transform.forward.y) * 0 - HOVER_ADD_BALL_Y,
            playerCamera.transform.forward.z * 1);
        }

        if (hoverLetter)
        {
            ballBox.transform.position = GameObject.Find("Player").transform.position + new Vector3(
            playerCamera.transform.forward.x * 2,
            math.sin(playerCamera.transform.forward.y) * 0 - HOVER_DOWN_LETTER_Y,
            playerCamera.transform.forward.z * 2);
        }

        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (isRunning)
        {
            arrowSpeed = RUN_ARROW;
        } else
        {
            arrowSpeed = WALK_ARROW;
        }
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0,0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion
    }
}
