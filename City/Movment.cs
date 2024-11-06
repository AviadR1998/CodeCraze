using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
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
    public static bool canMove = true, soccer = false, home = false;
    CharacterController characterController;
    public AudioSource soccesSound;
    public AudioSource backgrounMusic;

    public static bool raceOn;
    public GameObject player;
    public GameObject roomsMenu;
    public GameObject arrow;
    public GameObject findPanel;
    public static GameObject mission;
    public GameObject orderPanel;
    public TMP_Text objFoundHotCold;
    float arrowSpeed = 6.2f;
    bool canPress;
    GameObject ballBox;
    static public char currentLetter = '\0';
    static public bool hoverBall, hoverLetter;
    int cheatTransfer = 0;
    Vector3[] cheatArr = new Vector3[3] {new Vector3(-1069.69f, 13.57452f, 340.8305f), new Vector3(-720, 14, 273), new Vector3(-951.55f, 15.18f, 306.67f)};

    // Start is called before the first frame update
    void Start()
    {
        mission = GameObject.Find("RaceNPC");
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
    }

    private IEnumerator closeFindPanel()
    {
        yield return new WaitForSeconds(2);
        findPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FindObj")
        {
            Destroy(IfMissions.findObj[IfMissions.currentFindObj]);
            IfMissions.currentFindObj++;
            if(IfMissions.currentFindObj < 3) 
            {
                IfMissions.findObj[IfMissions.currentFindObj].SetActive(true);
                IfMissions.findObj[IfMissions.currentFindObj].transform.position -= new Vector3(0, 12, 0);
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
        yield return new WaitForSeconds(0.5f);
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
            BoxGame.boxLetters[other.name[6] - '0'] = currentLetter;
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
        if (soccer || PauseMenu.isPaused)
        {
            return;
        }
        if (home)
        {
            player.transform.position = arrow.transform.position = new Vector3(-866.33f, 13.015f, 94.84f);
            home = false;
            return;
        }
        if (arrow.active)
        {
            arrow.transform.position = Vector3.MoveTowards(arrow.transform.position,
            GameObject.Find("Player").transform.position + new Vector3(
            playerCamera.transform.forward.x * 5,
            math.sin(playerCamera.transform.forward.y) * 5 + 0.7f,
            playerCamera.transform.forward.z * 5), arrowSpeed * Time.deltaTime);
            arrow.transform.LookAt(mission.transform);
        }

        if (Input.GetKeyDown("z"))
        {
            player.GetComponent<CharacterController>().enabled = false;
            cheatTransfer = (cheatTransfer + 1) % 3;
            player.transform.position = cheatArr[cheatTransfer];
            player.GetComponent<CharacterController>().enabled = true;
        }

        if (canPress && Input.GetKeyDown("e") && (hoverBall || hoverLetter))
        {
            ballBox.transform.position = new Vector3(player.transform.position.x, 9.834f + (hoverLetter ? 0.2f : 0), player.transform.position.z);
            hoverLetter = canPress = hoverBall = false;
            StartCoroutine(delayPress());
        }

        if(hoverBall)
        {
            ballBox.transform.position = GameObject.Find("Player").transform.position + new Vector3(
            playerCamera.transform.forward.x * 1,
            math.sin(playerCamera.transform.forward.y) * 0 + -0.3f,
            playerCamera.transform.forward.z * 1);
        }

        if (hoverLetter)
        {
            ballBox.transform.position = GameObject.Find("Player").transform.position + new Vector3(
            playerCamera.transform.forward.x * 2,
            math.sin(playerCamera.transform.forward.y) * 0 + -0.3f,
            playerCamera.transform.forward.z * 2);
        }

        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (isRunning)
        {
            arrowSpeed = 12.2f;
        } else
        {
            arrowSpeed = 6.2f;
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
