using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForMission : MonoBehaviour
{
    public delegate void EndFunc();
    public GameObject redButton;
    public GameObject talkingPanel;
    public GameObject canvasMission;
    public TMP_Text talkingText;
    public TMP_Text practicalText;
    public GameObject practiceNPC;
    public GameObject player;
    public GameObject forNPC;

    //int currentSubText, currentSubMission;
    List<List<TextPartition>> texts;
    List<EndFunc> funcs;
    // Start is called before the first frame update
    void Start()
    {
        texts = new List<List<TextPartition>>();
        List<TextPartition> list = new List<TextPartition>();
        list.Add(new TextPartition("Hii what's up? i am going to teach you a new cool programing and this is...",""));
        list.Add(new TextPartition("for!!!\nThis is an important tool that will accompany you for long time in the programing world!", ""));
        list.Add(new TextPartition("So listen carefully!\nImagine that you want to roll the wheel 8 times. What have we done so far?", ""));
        list.Add(new TextPartition("Show me! Go press the red button to roll the wheel.\nDo this 8 times and then come back to me!", ""));
        list.Add(new TextPartition("What are you waiting for? Go!!!", ""));
        texts.Add(list);
        list = new List<TextPartition>();
        list.Add(new TextPartition("Well done! But don't you think that was a bit exhausting? You had to roll 8 times.\nNow, think about how you would implement that in code.", ""));
        list.Add(new TextPartition("You need to repeat your code 8 times. For example:", "public static void main(String[] args) {\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n}"));
        list.Add(new TextPartition("Let's try something different. Now the code looks much better!", "public static void main(String[] args) {\n\tfor (int i = 0; i < 8; i++) {\n\t\troll();//not an official coomand\n\t}\n}"));
        list.Add(new TextPartition("Now, let's look at what we see here. We run the roll command 8 times but in one line.\nThat's where the for loop comes in.", "public static void main(String[] args) {\n\tfor (int i = 0; i < 8; i++) {\n\t\troll();//not an official coomand\n\t}\n}"));
        list.Add(new TextPartition("Let's break down the for loop. We have the initialization step, which is executed once before the loop starts. It usually initializes one or more loop counters.", "for(intialize; contition; progress) {\n\tbody;\n}"));
        list.Add(new TextPartition("Condition: Before each iteration, the condition is evaluated. If it’s true, the loop body is executed; if it’s false, the loop terminates.", "for(intialize; contition; progress) {\n\tbody;\n}"));
        list.Add(new TextPartition("Progress: This step is executed after each iteration of the loop. It usually increments or decrements the loop counter.", "for(intialize; contition; progress) {\n\tbody;\n}"));
        list.Add(new TextPartition("Body: Here, we write what we want our for loop to execute in each iteration.", "for(intialize; contition; progress) {\n\tbody;\n}"));
        list.Add(new TextPartition("For example, what does the following code do? (Don't press 'OK' until you're sure.)", "public static void main(String[] args) {\n\tfor (int i = 1; i <= 5; i++) {\n\t\tSystem.out.println(i);\n\t}\n}"));
        list.Add(new TextPartition("The code prints the numbers from 1 to 5.", "1\n2\n3\n4\n5"));
        list.Add(new TextPartition("In for loops, and in loops in general, we can use the keywords break and continue.", ""));
        list.Add(new TextPartition("Continue: Use this keyword if you want to skip certain iterations based on a condition.\nFor example, here we skip the fourth iteration.", "public static void main(String[] args) {\n\tfor (int i = 1; i <= 5; i++) {\n\t\tif (i == 3) {\n\t\t\tcontinue;\n\t\t}\n\t\tSystem.out.println(i);\n\t}\n}"));
        list.Add(new TextPartition("Break: Use this keyword if you want to stop the loop before it completes all its iterations based on a condition.\nFor example, in this case, we stop the loop during the fourth iteration.", "public static void main(String[] args) {\n\tfor (int i = 1; i <= 5; i++) {\n\t\tif (i == 3) {\n\t\t\tbreak;\n\t\t}\n\t\tSystem.out.println(i);\n\t}\n}"));
        list.Add(new TextPartition("In Java, we also have the for-each loop, which allows you to iterate directly over the elements instead of the indexes of the array.", ""));
        list.Add(new TextPartition("Let's break down the for-each command:\nElementType: This is the type of the elements in the collection (e.g., int, String, or a user-defined type).", "for (elementType element : collection) {\n\tbody;\n}"));
        list.Add(new TextPartition("Element: A variable that represents the current element in the iteration.\nCollection: The array or collection that is being iterated over.", "for (elementType element : collection) {\n\tbody;\n}"));
        list.Add(new TextPartition("For example, what does the following code do? (Don't press 'OK' until you're sure.)", "public static void main(String[] args) {\n\tint[] arr = {1, 2, 3};\n\tfor (int number : arr) {\n\t\tSystem.out.println(number);\n\t}\n}"));
        list.Add(new TextPartition("The code prints the numbers from 1 to 3.", "1\n2\n3"));
        list.Add(new TextPartition("Note: Some languages, like C, do not support for-each loops.", ""));
        list.Add(new TextPartition("I think we're done here! Good luck—you’re going to be amazing, I know it!", "3\n2\n1\nGO!!!!"));
        texts.Add(list);
        texts.Add(new List<TextPartition>());
        list = new List<TextPartition>();
        list.Add(new TextPartition("Hi! I see your progress here, and I want to challenge you to play a little game.", ""));
        list.Add(new TextPartition("Our game will start by asking a question. If you answer correctly, you will get a chance to score; otherwise, I will get a chance to score. The first person to reach 3 points wins!", ""));
        list.Add(new TextPartition("Be quick, as you have only 25 seconds to answer.", ""));
        list.Add(new TextPartition("Let's start!!", ""));
        texts.Add(list);

        AdminMission.texts = texts;
        AdminMission.currentSubMission = 0;
        AdminMission.canTalk = true;
        funcs = new List<EndFunc>();
        //funcs.Add(soccerGame);
        funcs.Add(part1);
        funcs.Add(part1);
        funcs.Add(questions);
        funcs.Add(part1);
        funcs.Add(soccerGame);
        //currentSubMission = 0;
        AdminMission.okFunc = forOkFunc;
        PauseMenu.updateSave("City", "For", 0);
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
            if (AdminMission.currentSubMission == 4)
            {
                funcs[4]();
            }
        }
    }

    private void OnEnable()
    {
        AdminMission.endOk = false;
        talkingPanel.SetActive(true);
    }

    void questions()
    {
        canvasMission.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Practice.canAsk = PauseMenu.canPause = true;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<Movement>().enabled = true;
        Movement.mission = practiceNPC;
        Practice.nextMission.Add(forNPC);
        PauseMenu.updateSave("City", "For", 1);
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
