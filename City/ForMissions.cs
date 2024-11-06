using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForMission : MonoBehaviour
{
    public delegate void EndFunc();
    public GameObject redButton;
    public GameObject canvasMission;
    public TMP_Text talkingText;
    public TMP_Text practicalText;

    //int currentSubText, currentSubMission;
    List<List<TextPartition>> texts;
    List<EndFunc> funcs;
    // Start is called before the first frame update
    void Start()
    {
        texts = new List<List<TextPartition>>();
        List<TextPartition> list = new List<TextPartition>();
        list.Add(new TextPartition("Hii what's up? i am going to teach you a new cool programing and this is...",""));
        list.Add(new TextPartition("for!!!\nthis is an important tool that will accompany you for long time in the programing world!", ""));
        list.Add(new TextPartition("So listen carefully!\nthink that you want to roll the wheel for 8 times what have we done until now?", ""));
        list.Add(new TextPartition("Show me!! go press the red button to roll the wheel.\nDo it for 8 and come back to me!", ""));
        list.Add(new TextPartition("What are you waiting for GO!!!!", ""));
        texts.Add(list);
        list = new List<TextPartition>();
        list.Add(new TextPartition("Well done! but don't you think it was too exhausting? you need to roll 8 times\nthink that you have that in the code", ""));
        list.Add(new TextPartition("That you need to repeat your code 8 time for example:", "public static void main(String[] args) {\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n}"));
        list.Add(new TextPartition("So let try something different, Now the code lokes much better", "public static void main(String[] args) {\n\tfor (int i = 0; i < 8; i++) {\n\t\troll();//not an official coomand\n\t}\n}"));
        list.Add(new TextPartition("Now lets look on what we see here we run the roll command 8 time but in one line\nfor that we have the for ", "public static void main(String[] args) {\n\tfor (int i = 0; i < 8; i++) {\n\t\troll();//not an official coomand\n\t}\n}"));
        list.Add(new TextPartition("let dismantle the for command. we have the initialize in This step we executed it once before the loop starts. It usually initializes one or more loop counters.", "for(intialize; contition; progress) {\n\tbody;\n}"));
        list.Add(new TextPartition("Condition: Before each iteration, the condition is evaluated. If the condition is true, the loop body is executed. If it is false, the loop terminates.", "for(intialize; contition; progress) {\n\tbody;\n}"));
        list.Add(new TextPartition("progress: This step is executed after each iteration of the loop. It usually increments or decrements the loop counter.", "for(intialize; contition; progress) {\n\tbody;\n}"));
        list.Add(new TextPartition("body: here we write what we want our for will do", "for(intialize; contition; progress) {\n\tbody;\n}"));
        list.Add(new TextPartition("for example what the next code do?(dont press 'ok' until you dont sure)", "public static void main(String[] args) {\n\tfor (int i = 1; i <= 5; i++) {\n\t\tSystem.out.println(i);\n\t}\n}"));
        list.Add(new TextPartition("the code is printing from 1 to 5", "1\n2\n3\n4\n5"));
        list.Add(new TextPartition("In for and in general in loops we can use the keywords break,continue.", ""));
        list.Add(new TextPartition("continue: If you want to skip certain iterations based on a condition.\nfor example here we skip on the forth itaration", "public static void main(String[] args) {\n\tfor (int i = 1; i <= 5; i++) {\n\t\tif (i == 3) {\n\t\t\tcontinue;\n\t\t}\n\t\tSystem.out.println(i);\n\t}\n}"));
        list.Add(new TextPartition("break: If you want to stop the loop before it completes all its iterations based on some condition.\nfor example here we stop the loop on the forth itaration", "public static void main(String[] args) {\n\tfor (int i = 1; i <= 5; i++) {\n\t\tif (i == 3) {\n\t\t\tbreak;\n\t\t}\n\t\tSystem.out.println(i);\n\t}\n}"));
        list.Add(new TextPartition("in java we hava also for each. for each let you to itarate over the elements\nand not over the indexes of array", ""));
        list.Add(new TextPartition("let dismantle the for each command. elementType: is the type of the elements\nin the collection (e.g., int, String, or a user-defined type).", "for (elementType element : collection) {\n\tbody;\n}"));
        list.Add(new TextPartition("element: is a variable that represents the current element in the iteration.\ncollection: is the array or collection being iterated over.", "for (elementType element : collection) {\n\tbody;\n}"));
        list.Add(new TextPartition("for example what the next code do?(dont press 'ok' until you dont sure)", "public static void main(String[] args) {\n\tint[] arr = {1, 2, 3};\n\tfor (int number : arr) {\n\t\tSystem.out.println(number);\n\t}\n}"));
        list.Add(new TextPartition("the code is printing from 1 to 3", "1\n2\n3"));
        list.Add(new TextPartition("note: There are languages that doesnt support for each like C.", ""));
        list.Add(new TextPartition("I think we done here! good luck you will become the great i know it!!!!", "3\n2\n1\nGO!!!!"));
        texts.Add(list);

        list = new List<TextPartition>();
        list.Add(new TextPartition("Hii i see your progress here and i want to chalange you by play a little game", ""));
        list.Add(new TextPartition("Our game will start by asking a question and the one who will answer the question correct will get a chance to kick a score and the one who will get 3 points Win!", ""));
        list.Add(new TextPartition("lets start!!", ""));

        AdminMission.texts = texts;
        AdminMission.currentSubMission = 0;
        AdminMission.canTalk = true;
        funcs = new List<EndFunc>();
        //funcs.Add(soccerGame);
        funcs.Add(part1);
        funcs.Add(part1);
        funcs.Add(soccerGame);
        //currentSubMission = 0;
        AdminMission.okFunc = forOkFunc;
    }

    void forOkFunc()
    {
        if (texts[AdminMission.currentSubMission].Count == AdminMission.currentSubText)
        {
            AdminMission.canTalk = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PauseMenu.canPause = true;
            canvasMission.SetActive(false);
            GameObject.Find("Player").GetComponent<Movement>().enabled = true;
            AdminMission.endOk = true;
            if (AdminMission.currentSubMission == 0)
            {
                redButton.SetActive(true);
                Movement.mission = redButton;
            }
            AdminMission.currentSubMission++;
            if (AdminMission.currentSubMission == 2)
            {
                funcs[2]();
            }
        }
    }

    private void OnEnable()
    {
        AdminMission.endOk = false;
    }

    void part1()
    {
        AdminMission.canTalk = false;
        AdminMission.currentSubText = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        PauseMenu.canPause = false;
        canvasMission.SetActive(true);
        GameObject.Find("Player").GetComponent<Movement>().enabled = false;
        talkingText.text = texts[AdminMission.currentSubMission][AdminMission.currentSubText].talking;
        practicalText.text = texts[AdminMission.currentSubMission][AdminMission.currentSubText].practical;
    }

    void soccerGame()
    {
        canvasMission.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.canPause = true;
        GameObject.Find("Ball").GetComponent<SoccerMovment>().enabled = true;
        GameObject.Find("Player").GetComponent<Movement>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (AdminMission.canTalk && other.tag == "Player")
        {
            funcs[AdminMission.currentSubMission]();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
