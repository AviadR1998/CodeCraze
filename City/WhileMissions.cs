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
    public TMP_Text talkingText;
    public TMP_Text practicalText;
    public AudioSource honk;
    public static bool canTalk, endOk;

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
        list.Add(new TextPartition("Hii my friend. I need your help! please help me!", ""));
        list.Add(new TextPartition("We have a party here but the dirver is honk to us and shouting\nfor help please help him that we could continue with our party!", ""));
        list.Add(new TextPartition("go to the ticket machine, raise the bar and come back.",""));
        list.Add(new TextPartition("note that the driver will ask for help and honk for every 2 seconds\nuntil you will release him. Good Luck!!!", ""));
        list.Add(new TextPartition("", "The driver: Help!"));
        texts.Add(list);
        list = new List<TextPartition>();
        list.Add(new TextPartition("Thank you for helping us!! now we can go back to party.", ""));
        list.Add(new TextPartition("but before that I want to thank you by teaching you a new programing skill.\n", ""));
        list.Add(new TextPartition("Let look what we have seen in here? we saw somethink like the while command.\n", ""));
        list.Add(new TextPartition("Lets make a pseudocode for it!! And that's how you write a while statement!!", "public static void main(String[] args) {\n\tboolean theGateIsDown = true;\n\twhile(theGateIsDown) {\n\t\tSystem.out.println(\"Help!\");\n\t\thonk();//not an official coomand\n\t}\n}"));
        list.Add(new TextPartition("Lets see what we have here!! we have a boolean var that caled 'theGateIsDown' and it is set to true at the beggining and the code will run while 'theGateIsDown' is true", "public static void main(String[] args) {\n\tboolean theGateIsDown = true;\n\twhile(theGateIsDown) {\n\t\tSystem.out.println(\"Help!\");\n\t\thonk();//not an official coomand\n\t}\n}"));
        list.Add(new TextPartition("let dismantle the while command. body: what is actual do in each itaration.\ncondition: is evaluated after the code block has been executed.", "while (condition) {\n\tbody;\n}"));
        list.Add(new TextPartition("Let see another example. We see here that the while printing the numbers 1 to 4\n", "public static void main(String[] args) {\n\tint counter = 1;\n\twhile (counter <= 4) {\n\t\tSystem.out.println(counter);\n\t\tcounter++;\n\t}\n}"));
        list.Add(new TextPartition("the while start from 1 and on each iteration the counter is increasing by one.\nthe while stop when reaching to 4", "public static void main(String[] args) {\n\tint counter = 1;\n\twhile (counter <= 4) {\n\t\tSystem.out.println(counter);\n\t\tcounter++;\n\t}\n}"));
        list.Add(new TextPartition("for example what the next code do?(dont press 'ok' until you dont sure)", "public static void main(String[] args) {\n\tint number = 1;\n\twhile (number <= 10) {\n\t\tif (number % 2 == 0) {\n\t\t\tSystem.out.println(number);\n\t\t}\n\t\tcounter++;\n\t}\n}"));
        list.Add(new TextPartition("the code is printing the even numbers from 1 to 10.\nexplanation: the while runnig from 1 to 10 and check if the number is even\n(number % 2 == 0 check if the number is even) and if he is even so print it.", "2\n4\n6\n8\n10"));
        list.Add(new TextPartition("There is 'do while' also 'do while' is like 'while' but the diffrent between them is that the 'do while' is happend at least 1 time and 'while' its depend on the condition", ""));
        list.Add(new TextPartition("condition: is evaluated after the code block has been executed.", "public static void main(String[] args) {\n\tdo {\n\t\tbody;\n\t} while (condition);\n}"));
        list.Add(new TextPartition("body: happened at least once and keeping activate if the condition is true.", "public static void main(String[] args) {\n\tdo {\n\t\tbody;\n\t} while (condition);\n}"));
        list.Add(new TextPartition("for example what the next code do?(dont press 'ok' until you dont sure)", "public static void main(String[] args) {\n\tint number = 1;\n\tdo {\n\t\tSystem.out.println(number);\n\t\tnumber++;\n\t} while (number <= 1);\n}"));
        list.Add(new TextPartition("the code print 1 because the 'do while' enter the loop and print the 1 after that\nthe number raise by one and then the number is greater the 1 and exit the while", "1"));
        list.Add(new TextPartition("Lets start parctice!", ""));
        texts.Add(list);
        AdminMission.texts = texts;
        AdminMission.currentSubMission = 0;
        honkScene = canTalk = true;
        funcs = new List<EndFunc>();
        funcs.Add(introducing);
        funcs.Add(questions);
        AdminMission.okFunc = whileOkFunc;
    }

    private void OnEnable()
    {
        AdminMission.endOk = false;
    }

    void questions()
    {
        canvasMission.SetActive(false);
        talkingPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.canPause = true;
        player.GetComponent<Movement>().enabled = true;
    }

    void introducing()
    {
        canvasMission.SetActive(false);
        orderCanvas.SetActive(true);
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
        if (canTalk && other.tag == "Player")
        {
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
            funcs[AdminMission.currentSubMission]();
            AdminMission.endOk = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
