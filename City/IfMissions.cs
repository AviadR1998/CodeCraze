using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.HighDefinition.CameraSettings;

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
    public GameObject okButton;
    public TMP_Text talkingText;
    public TMP_Text practicalText;
    public GameObject[] findObjU;
    public static GameObject[] findObj;
    public GameObject hotColdCanvas;
    public UnityEngine.UI.Image hotColdImg;//for the emojis
    public delegate void EndFunc();
    List<EndFunc> funcs;
    List<List<TextPartition>> texts;

    public static bool xLine;
    bool talk, stop, startCutS, hotColdBool, findAll;
    float distance;
    public static int currentFindObj;
    Vector3 startPoint = new Vector3(-19.54f, 1.46f, -13.42f) + new Vector3(-901.14f, 11.69f, 297.78f); //(-19.54f, 4.46f, -11.92f)

    // Start is called before the first frame update
    void Start()
    {
        distance = float.MaxValue;
        stopLine.SetActive(true);
        findAll = hotColdBool = startCutS = stop = talk = xLine = false;
        findObj = findObjU;
        texts = new List<List<TextPartition>>();
        List<TextPartition> list = new List<TextPartition>();
        list.Add(new TextPartition("hii, do you like the city i hope that you treated here nicely. Now we are going to learn a new command and this is 'if'", ""));
        list.Add(new TextPartition("The if statement allows you to control the flow of your program based on whether a certain condition is true or false.", "public static void main(String[] args) {\n\tif (condition) {\n\t\tbody1\n\t} else {\n\t\tbody2\n\t}\n}"));
        list.Add(new TextPartition("Let disnamtle it - condition: This is a Boolean expression that evaluates to either true or false.\nbody1: code to be executed if the condition is true.\nbody2: code to be executed if the condition is false (optional)", "public static void main(String[] args) {\n\tif (condition) {\n\t\tbody1\n\t} else {\n\t\tbody2\n\t}\n}"));
        list.Add(new TextPartition("note that you don't have to used the else statement and then the if statement will look like this:", "public static void main(String[] args) {\n\tif (condition) {\n\t\tbody1\n\t}\n}"));
        list.Add(new TextPartition("lets see an example in our world", ""));
        texts.Add(list);

        /*list = new List<TextPartition>();
        list.Add(new TextPartition("Now we will see a car tring to pass a traffic light.", ""));
        texts.Add(list);*/

        list = new List<TextPartition>();
        list.Add(new TextPartition("We see here that the orange car need to stop if the light trafiic is red else the car can pass so if i need to write a pseudocode here it will be something like that(some command here are not an official command)", "public static void main(String[] args) {\n\tif (light == \"red\") {\n\t\tstopcar();\n\t} else {\n\t\tpass();\n\t}\n}"));
        list.Add(new TextPartition("let see now some example in this example the program will print 'you are a big boy' but if you change the age to a number under 10 it will print 'you are not a big boy'.", "public static void main(String[] args) {\n\tint age = 11;\n\tif (age >= 10) {\n\t\tSystem.out.println(\"you are a big boy\");\n\t} else {\n\t\tSystem.out.println(\"you are not a big boy\");\n\t}\n}"));
        list.Add(new TextPartition("another example: what the next code do?(dont press 'ok' until you dont sure)", "public static void main(String[] args) {\n\tboolean isWeekend = true;\n\tboolean isRaining = false;\n\tif (isWeekend && !isRaining) {\n\t\tSystem.out.println(\"perfect weather to play outside!\");\n\t} else {\n\t\tSystem.out.println(\"perfect weather to play at home!\");\n\t}\n}"));
        list.Add(new TextPartition("here our program will print 'perfect weather to play outside!' becase first we declare on two bool var isWeekend = true, isRaining = false we learn in the previous level that\n(true & !false) = (true & true) = (true) so the condition is true and we enter to body1 so we printing 'perfect weather to play outside!'", "public static void main(String[] args) {\n\tboolean isWeekend = true;\n\tboolean isRaining = false;\n\tif (isWeekend && !isRaining) {\n\t\tSystem.out.println(\"perfect weather to play outside!\");\n\t} else {\n\t\tSystem.out.println(\"perfect weather to play at home!\");\n\t}\n}"));
        list.Add(new TextPartition("i hope that you take this leasson seriously because 'if' statement are really important and if you understand this leasson many things will be clear to you.", ""));
        list.Add(new TextPartition("good luck in the rest of this leasson!!!", ""));
        texts.Add(list);

        list = new List<TextPartition>();
        list.Add(new TextPartition("I need your help! I have 3 objects that gone in the city and i need your help to find them. I lost a Rubber duck a burger and a toy car.", ""));
        list.Add(new TextPartition("I know where are them and if you get close to him i will show the hot emoji, and if you get far from the object the emoji will became freeze.", ""));
        list.Add(new TextPartition("Good luck!!!", ""));
        //texts.Add(list);

        list = new List<TextPartition>();
        list.Add(new TextPartition("thanks!! you completed all my task good luck in the future!", ""));
        texts.Add(list);

        AdminMission.texts = texts;
        funcs = new List<EndFunc>();
        funcs.Add(cutSceneCar);
        funcs.Add(hotCold);
        funcs.Add(questions);
        currentFindObj = AdminMission.currentSubMission = 0;
        AdminMission.okFunc = ifOkFunc;
    }

    private void OnEnable()
    {
        AdminMission.endOk = false;
    }

    void cutSceneCar()
    {
        //canvasMission.SetActive(false);
        AdminMission.currentSubMission++;
        okButton.SetActive(false);
        talkingText.text = "Now we will see a car tring to pass a traffic light.";
        startCutS = true;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        camera.transform.position = startPoint;// + player.transform.position;
        camera.transform.rotation = Quaternion.Euler(0, 90, 0);
        grayCar.SetActive(false);
        blackCar.SetActive(false);
        blueCar.SetActive(false);
        car.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !talk)
        {
            //other.transform.LookAt(npc.transform);
            //some explanations
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
        findObj[0].SetActive(true);
        findObj[0].transform.position -= new Vector3(0, 12, 0);
        hotColdCanvas.SetActive(true);
        hotColdBool = true;
    }

    void questions()
    {
        canvasMission.SetActive(false);
        talkingPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.canPause = true;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<Movement>().enabled = true;
    }

    void ifOkFunc()
    {
        if (texts[AdminMission.currentSubMission].Count == AdminMission.currentSubText)
        {
            funcs[AdminMission.currentSubMission]();
            AdminMission.endOk = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hotColdBool)
        {
            if (currentFindObj == 3)
            {
                if (!findAll)
                {
                    hotColdCanvas.SetActive(false);
                    canvasMission.SetActive(true);
                    okButton.SetActive(false);
                    talkingText.text = "You finish!!! now go back and return the objects you find";
                    practicalText.text = "";
                    talk = false;
                    findAll = true;
                    AdminMission.currentSubMission++;
                    AdminMission.currentSubText = 0;
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
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, startPoint + new Vector3(33, 0, 0), 20 * Time.deltaTime);
        }
        if (xLine)
        {
            stopLine.SetActive(false);
            car.SetActive(false);
            grayCar.transform.position = new Vector3(-903.06f, 10.85f, 282.99f);
            blackCar.transform.position = new Vector3(-917.03f, 10.85f, 150.3f);
            blueCar.transform.position = new Vector3(-765.32f, 10.85f, 143.84f);
            grayCar.SetActive(true);
            blackCar.SetActive(true);
            blueCar.SetActive(true);
            player.transform.position = new Vector3(-901.3398f, 11.69172f, 297.0288f);
            camera.transform.position = new Vector3(0.1645798f, 0.0239689f, 0.01668368f) + player.transform.position;
            player.transform.rotation = Quaternion.Euler(0, 91.5f, 0);
            camera.transform.rotation = Quaternion.Euler(-4.5f, 90, 0);
            AdminMission.currentSubText = 0;
            okButton.SetActive(true);
            talkingText.text = texts[AdminMission.currentSubMission][0].talking;
            practicalText.text = texts[AdminMission.currentSubMission][0].practical;
            xLine = false;
            stop = true;
        }
    }
}
