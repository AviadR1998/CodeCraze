using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class TextPartition
{
    public string talking;
    public string practical;
    public TextPartition(string str1, string str2)
    {
        talking = str1;
        practical = str2;
    }
}

public class WhileMissions : MonoBehaviour
{
    public delegate void EndFunc();
    public GameObject ticketMachine;
    public GameObject canvasMission;
    public GameObject orderCanvas;
    public GameObject player;
    public GameObject talkingPanel;
    public GameObject practicalOrderPanel;
    public GameObject missionCompleteCanvas;
    public GameObject gate;
    public TMP_Text talkingText;
    public TMP_Text practicalText;
    public AudioSource honk;
    public AudioSource finishSound;
    public static bool canTalk, endOk, startFromQuestions = false;
    public GameObject practiceNPC;
    public GameObject forNPC;
    public GameObject arrow;

    List<EndFunc> funcs;
    public static EndFunc okFunc;
    public static int currentSubMission, currentSubText;
    bool honkScene;
    public static List<List<TextPartition>> texts;
    // Start is called before the first frame update
    void Start()
    {
        texts = new List<List<TextPartition>>();
        List<TextPartition> list = new List<TextPartition>();
        list.Add(new TextPartition("Hi, my friend! I need your help—please help me!", ""));
        list.Add(new TextPartition("We're having a party here, but the driver is honking at us and shouting for help. Please help him so we can continue with our party!", ""));
        list.Add(new TextPartition("Go to the ticket machine, raise the bar, and come back.", ""));
        list.Add(new TextPartition("Note that the driver will ask for help and honk every 2 seconds until you release him. Good luck!", ""));
        list.Add(new TextPartition("", "The driver: Help!"));
        texts.Add(list);
        list = new List<TextPartition>();
        list.Add(new TextPartition("Thank you for helping us! Now we can go back to the party.", ""));
        list.Add(new TextPartition("But before that, I want to thank you by teaching you a new programming skill.", ""));
        list.Add(new TextPartition("Let's look at what we've seen here. We saw something like the while command.", ""));
        list.Add(new TextPartition("Let's write pseudocode for it! This is how you write a while statement!", "public static void main(String[] args) {\n\tboolean theGateIsDown = true;\n\twhile(theGateIsDown) {\n\t\tSystem.out.println(\"Help!\");\n\t\thonk();//not an official coomand\n\t}\n}"));
        list.Add(new TextPartition("Let's see what we have here! We have a boolean variable called theGateIsDown, which is set to true at the beginning. The code will run as long as theGateIsDown is true.", "public static void main(String[] args) {\n\tboolean theGateIsDown = true;\n\twhile(theGateIsDown) {\n\t\tSystem.out.println(\"Help!\");\n\t\thonk();//not an official coomand\n\t}\n}"));
        list.Add(new TextPartition("Let's break down the while command:\nBody: What is actually done in each iteration.\nCondition: This is evaluated after the code block has been executed.", "while (condition) {\n\tbody;\n}"));
        list.Add(new TextPartition("Let's look at another example. Here, we see that the while loop is printing the numbers 1 to 4.", "public static void main(String[] args) {\n\tint counter = 1;\n\twhile (counter <= 4) {\n\t\tSystem.out.println(counter);\n\t\tcounter++;\n\t}\n}"));
        list.Add(new TextPartition("The while loop starts at 1, and in each iteration, the counter increases by one. The loop stops when it reaches 4.", "public static void main(String[] args) {\n\tint counter = 1;\n\twhile (counter <= 4) {\n\t\tSystem.out.println(counter);\n\t\tcounter++;\n\t}\n}"));
        list.Add(new TextPartition("For example, what does the following code do? (Don't press 'OK' until you're sure.)", "public static void main(String[] args) {\n\tint number = 1;\n\twhile (number <= 10) {\n\t\tif (number % 2 == 0) {\n\t\t\tSystem.out.println(number);\n\t\t}\n\t\tcounter++;\n\t}\n}"));
        list.Add(new TextPartition("The code prints the even numbers from 1 to 10.\nExplanation: The while loop runs from 1 to 10 and checks if each number is even. The condition (number % 2 == 0) checks if the number is even, and if it is, the code prints it.", "2\n4\n6\n8\n10"));
        list.Add(new TextPartition("There is also the do while loop. The do while loop is similar to the while loop, but the difference is that the do while loop executes at least once, whereas the while loop depends on the condition.", ""));
        list.Add(new TextPartition("Condition: This is evaluated after the code block has been executed.", "public static void main(String[] args) {\n\tdo {\n\t\tbody;\n\t} while (condition);\n}"));
        list.Add(new TextPartition("Body: This executes at least once and continues to run as long as the condition is true.", "public static void main(String[] args) {\n\tdo {\n\t\tbody;\n\t} while (condition);\n}"));
        list.Add(new TextPartition("For example, what does the following code do? (Don't press 'OK' until you're sure.)", "public static void main(String[] args) {\n\tint number = 1;\n\tdo {\n\t\tSystem.out.println(number);\n\t\tnumber++;\n\t} while (number <= 1);\n}"));
        list.Add(new TextPartition("The code prints 1 because the do while loop enters and prints 1. After that, the number is incremented by one, and since the number is now greater than 1, it exits the loop.", "1"));
        list.Add(new TextPartition("Let's start practicing!", ""));
        texts.Add(list);
        AdminMission.texts = texts;
        AdminMission.currentSubMission = 0;
        honkScene = canTalk = true;
        funcs = new List<EndFunc>();
        funcs.Add(introducing);
        funcs.Add(questions);
        AdminMission.okFunc = whileOkFunc;
        PauseMenu.updateSave("City", "While", 0);
    }

    private void OnEnable()
    {
        AdminMission.endOk = false;
    }

    void questions()
    {
        finishSound.Play();
        missionCompleteCanvas.SetActive(true);
        canvasMission.SetActive(false);
        talkingPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Practice.canAsk = PauseMenu.canPause = true;
        player.GetComponent<Movement>().enabled = true;
        Practice.taskName = "while";
        Practice.nextMission.Add(practiceNPC);
        Practice.nextMission.Add(forNPC);
        Movement.mission = practiceNPC;
        PauseMenu.updateSave("City", "While", 1);
    }

    void introducing()
    {
        canvasMission.SetActive(false);
        orderCanvas.SetActive(true);
        practicalOrderPanel.SetActive(true);
        ticketMachine.GetComponent<GateScript>().enabled = true;
        AdminMission.currentSubMission++;
        player.GetComponent<Movement>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.canPause = true;
        Movement.mission = ticketMachine;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Login.world != "City" && Movement.missionInProgress == "")
        {
            gate.transform.rotation = new UnityEngine.Quaternion(0, 0, 0, 0);
            Movement.missionInProgress = "while";
            AdminMission.texts = texts;
            AdminMission.currentSubMission = 0;
            honkScene = canTalk = true;
            AdminMission.okFunc = whileOkFunc;
            AdminMission.endOk = false;
        }
        if (Login.world != "City" && Movement.missionInProgress != "while")
        {
            return;
        }
        if (canTalk && other.tag == "Player")
        {
            arrow.SetActive(false);
            AdminMission.currentSubText = 0;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            PauseMenu.canPause = false;
            canvasMission.SetActive(true);
            talkingPanel.SetActive(true);
            player.GetComponent<Movement>().enabled = false;
            talkingText.text = texts[AdminMission.currentSubMission][AdminMission.currentSubText].talking;
            practicalText.text = texts[AdminMission.currentSubMission][AdminMission.currentSubText].practical;
            canTalk = false;
        }
    }

    void whileOkFunc()
    {
        if (honkScene && AdminMission.currentSubText == texts[AdminMission.currentSubMission].Count - 1)
        {
            honk.Play();
            honkScene = false;
        }
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
            canTalk = false;
            questions();
        }
    }
}
