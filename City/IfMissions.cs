using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor;
using UnityEngine;

public class IfMissions : MonoBehaviour
{
    public GameObject car;
    public GameObject grayCar;
    public GameObject blackCar;
    public GameObject blueCar;
    public GameObject roadLight;
    public GameObject npc;
    public GameObject camera;
    public GameObject player;
    public GameObject stopLine;
    public GameObject canvasMission;
    public GameObject talkingPanel;
    public GameObject practicalPanel;
    public GameObject okButton;
    public GameObject arrow;
    public GameObject practiceNPC;
    public GameObject whileNPC;
    public GameObject missionCompleteCanvas;
    public TMP_Text talkingText;
    public TMP_Text practicalText;
    public TMP_Text objFoundHotCold;
    public GameObject[] findObjU;
    public static GameObject[] findObj;
    public GameObject hotColdCanvas;
    public UnityEngine.UI.Image hotColdImg;
    public delegate void EndFunc();
    List<EndFunc> funcs;
    List<List<TextPartition>> texts;
    public AudioSource finishSound;

    public static bool xLine, startFromQuestions = false;
    bool talk, stop, startCutS, hotColdBool, findAll;
    float distance;
    public static int currentFindObj;
    Vector3 startPoint = new Vector3(-19.54f, 1.46f, -13.42f) + new Vector3(-901.14f, 11.69f, 297.78f), carPosision, cameraPos; //(-19.54f, 4.46f, -11.92f)

    const int CAMERA_CLOSE = 60, CAMERA_FAR = 21, ROTATION_Y = 90, DOWN_OBJ = 12, SPEED = 20, DISTANCE = 33;
    const float CAMERA_ANGLE_X = -4.5f, CAMERA_ANGLE_Y1 = 91.5f, CAMERA_ANGLE_Y2 = 90;
    const float GRAY_X = -903.06f, CAR_Y = 10.85f, GRAY_Z = 282.99f, BLACK_X = -917.03f, BLACK_Z = 150.3f, BLUE_X = -765.32f, BLUE_Z = 143.84f;
    const float PLAYER_X = -901.3398f, PLAYER_Y = 11.69172f, PLAYER_Z = 297.0288f, CAMERA_X = 0.2895798f, CAMERA_Y = 0.1069689f, CAMERA_Z = 0.02268368f;
    // Start is called before the first frame update
    void Start()
    {
        cameraPos = camera.transform.position;
        distance = float.MaxValue;
        stopLine.SetActive(true);
        findAll = hotColdBool = startCutS = stop = talk = xLine = false;
        findObj = findObjU;
        texts = new List<List<TextPartition>>();
        List<TextPartition> list = new List<TextPartition>();
        list.Add(new TextPartition("Hi, do you like the city? I hope you were treated well here. Now we are going to learn a new command: 'if'.", ""));
        list.Add(new TextPartition("The if statement allows you to control the flow of your program based on whether a certain condition is true or false.", "public static void main(String[] args) {\n\tif (condition) {\n\t\tbody1\n\t} else {\n\t\tbody2\n\t}\n}"));
        list.Add(new TextPartition("Let dismantle it - condition: This is a Boolean expression that evaluates to either true or false.\nbody1: code to be executed if the condition is true.\nbody2: code to be executed if the condition is false (optional)", "public static void main(String[] args) {\n\tif (condition) {\n\t\tbody1\n\t} else {\n\t\tbody2\n\t}\n}"));
        list.Add(new TextPartition("Note that you don't have to use the else statement; the if statement will then look like this:", "public static void main(String[] args) {\n\tif (condition) {\n\t\tbody1\n\t}\n}"));
        list.Add(new TextPartition("Let's look at an example from our world.", ""));
        texts.Add(list);

        list = new List<TextPartition>();
        list.Add(new TextPartition("We see here that the orange car needs to stop if the traffic light is red; otherwise, the car can pass. If I were to write pseudocode for this, it would look something like this (note that some commands here are not official commands).", "public static void main(String[] args) {\n\tif (light == \"red\") {\n\t\tstopcar();\n\t} else {\n\t\tpass();\n\t}\n}"));
        list.Add(new TextPartition("Let's look at an example now. In this example, the program will print 'You are a big boy.' However, if you change the age to a number below 10, it will print 'You are not a big boy.'", "public static void main(String[] args) {\n\tint age = 11;\n\tif (age >= 10) {\n\t\tSystem.out.println(\"you are a big boy\");\n\t} else {\n\t\tSystem.out.println(\"you are not a big boy\");\n\t}\n}"));
        list.Add(new TextPartition("Another example: What does the following code do? (Don't press 'OK' until you're sure.)", "public static void main(String[] args) {\n\tboolean isWeekend = true;\n\tboolean isRaining = false;\n\tif (isWeekend && !isRaining) {\n\t\tSystem.out.println(\"perfect weather to play outside!\");\n\t} else {\n\t\tSystem.out.println(\"perfect weather to play at home!\");\n\t}\n}"));
        list.Add(new TextPartition("In this example, our program will print 'Perfect weather to play outside!' because, first, we declare two boolean variables: isWeekend = true and isRaining = false. We learned in the previous level that (true & !false) = (true & true) = true, so the condition is true, and we enter body1, where we print 'Perfect weather to play outside!'.", "public static void main(String[] args) {\n\tboolean isWeekend = true;\n\tboolean isRaining = false;\n\tif (isWeekend && !isRaining) {\n\t\tSystem.out.println(\"perfect weather to play outside!\");\n\t} else {\n\t\tSystem.out.println(\"perfect weather to play at home!\");\n\t}\n}"));
        list.Add(new TextPartition("I hope you're taking this lesson seriously because if statements are really important. Once you understand this lesson, many things will become clearer to you.", ""));

        list.Add(new TextPartition("I need your help! I have three objects that have gone missing in the city: a rubber duck, a burger, and a toy car. Can you help me find them?", ""));
        list.Add(new TextPartition("I know where they are! If you get close to them, I'll show a hot emoji, and if you move further away from the objects, the emoji will change to a frozen one.", ""));
        list.Add(new TextPartition("Good luck!!!", ""));
        texts.Add(list);

        list = new List<TextPartition>();
        list.Add(new TextPartition("thanks!! you can go and practice now. Good luck in the future!", ""));
        texts.Add(list);

        AdminMission.texts = texts;
        funcs = new List<EndFunc>();
        funcs.Add(cutSceneCar);
        funcs.Add(hotCold);
        funcs.Add(questions);
        currentFindObj = AdminMission.currentSubMission = 0;
        AdminMission.okFunc = ifOkFunc;
        Movement.mission = npc;
        PauseMenu.updateSave("City", "If", 0);

        carPosision = new Vector3(car.transform.position.x, car.transform.position.y, car.transform.position.z);
    }

    private void OnEnable()
    {
        AdminMission.endOk = false;
    }

    void cutSceneCar()
    {
        camera.GetComponent<Camera>().fieldOfView = CAMERA_FAR;
        AdminMission.currentSubMission++;
        okButton.SetActive(false);
        talkingText.text = "Now we will see a car tring to pass a traffic light.";
        startCutS = true;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        camera.transform.position = startPoint;// + player.transform.position;
        camera.transform.rotation = Quaternion.Euler(0, ROTATION_Y, 0);
        grayCar.SetActive(false);
        blackCar.SetActive(false);
        blueCar.SetActive(false);
        car.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Login.world != "City" && Movement.missionInProgress == "")
        {
            objFoundHotCold.text = "0/3";
            distance = float.MaxValue;
            stopLine.SetActive(true);
            findAll = hotColdBool = startCutS = stop = talk = xLine = false;
            findObj = findObjU;
            Movement.missionInProgress = "if";
            AdminMission.texts = texts;
            currentFindObj = AdminMission.currentSubMission = AdminMission.currentSubText = 0;
            AdminMission.okFunc = ifOkFunc;
            talk = false;
            AdminMission.endOk = false;
            car.transform.position = new Vector3(carPosision.x, carPosision.y, carPosision.z);
        }
        if (Login.world != "City" && Movement.missionInProgress != "if")
        {
            return;
        }
        if (other.tag == "Player" && !talk)
        {
            //other.transform.LookAt(npc.transform);
            arrow.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            PauseMenu.canPause = false;
            canvasMission.SetActive(true);
            talkingPanel.SetActive(true);
            okButton.SetActive(true);
            talk = true;
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<Movement>().enabled = false;
            talkingText.text = texts[AdminMission.currentSubMission][AdminMission.currentSubText].talking;
            practicalText.text = texts[AdminMission.currentSubMission][AdminMission.currentSubText].practical;
        }
    }

    void hotCold()
    {
        //
        canvasMission.SetActive(false);
        //talkingPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.canPause = true;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<Movement>().enabled = true;
        //
        arrow.SetActive(false);
        findObj[0].SetActive(true);
        findObj[0].transform.position -= new Vector3(0, DOWN_OBJ, 0);
        hotColdCanvas.SetActive(true);
        hotColdBool = true;
    }

    void questions()
    {
        finishSound.Play();
        practicalPanel.SetActive(true);
        missionCompleteCanvas.SetActive(true);
        canvasMission.SetActive(false);
        talkingPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.canPause = true;
        player.GetComponent<CharacterController>().enabled = true;
        Practice.taskName = "if";
        Practice.canAsk = player.GetComponent<Movement>().enabled = true;
        Movement.mission = practiceNPC;
        Practice.nextMission.Add(whileNPC);
        PauseMenu.updateSave("City", "If", 1);
    }

    void ifOkFunc()
    {
        if (texts[AdminMission.currentSubMission].Count == AdminMission.currentSubText)
        {
            arrow.SetActive(true);
            funcs[AdminMission.currentSubMission]();
            AdminMission.endOk = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startFromQuestions)
        {
            startFromQuestions = false;
            talk = false;
            questions();
        }
        if (hotColdBool)
        {
            if (currentFindObj == 3)
            {
                if (!findAll)
                {
                    hotColdCanvas.SetActive(false);
                    canvasMission.SetActive(true);
                    practicalPanel.SetActive(false);
                    okButton.SetActive(false);
                    talkingText.text = "You finish!!! now go back and return the objects you find";
                    practicalText.text = "";
                    talk = false;
                    findAll = true;
                    AdminMission.currentSubMission++;
                    AdminMission.currentSubText = 0;
                    arrow.SetActive(true);
                    arrow.transform.position = player.transform.position;
                    Movement.mission = npc;
                }
                return;
            }
            float currentDis = Vector3.Distance(player.transform.position, findObj[currentFindObj].transform.position);
            if (currentDis < distance)
            {
                hotColdImg.sprite = Resources.Load("Hot", typeof(Sprite)) as Sprite;
            }
            if (currentDis > distance)
            {
                hotColdImg.sprite = Resources.Load("Freeze", typeof(Sprite)) as Sprite;
            }
            distance = currentDis;
        }
        if (!stop && startCutS)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, startPoint + new Vector3(DISTANCE, 0, 0), SPEED * Time.deltaTime);
        }
        if (xLine)
        {
            camera.GetComponent<Camera>().fieldOfView = CAMERA_CLOSE;
            stopLine.SetActive(false);
            car.SetActive(false);
            grayCar.transform.position = new Vector3(GRAY_X, CAR_Y, GRAY_Z);
            blackCar.transform.position = new Vector3(BLACK_X, CAR_Y, BLACK_Z);
            blueCar.transform.position = new Vector3(BLUE_X, CAR_Y, BLUE_Z);
            grayCar.SetActive(true);
            blackCar.SetActive(true);
            blueCar.SetActive(true);
            player.transform.position = new Vector3(PLAYER_X, PLAYER_Y, PLAYER_Z);
            camera.transform.position = new Vector3(CAMERA_X, CAMERA_Y, CAMERA_Z) + player.transform.position;
            player.transform.rotation = Quaternion.Euler(0, CAMERA_ANGLE_Y1, 0);
            camera.transform.rotation = Quaternion.Euler(CAMERA_ANGLE_X, CAMERA_ANGLE_Y2, 0);
            AdminMission.currentSubText = 0;
            okButton.SetActive(true);
            talkingText.text = texts[AdminMission.currentSubMission][0].talking;
            practicalText.text = texts[AdminMission.currentSubMission][0].practical;
            xLine = false;
            stop = true;
        }
    }
}
