using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForMission : MonoBehaviour
{
    public static bool canTalk;
    public GameObject redButton;
    public GameObject canvasMission;
    public TMP_Text talkingText;
    public TMP_Text practicalText;

    int currentSubText, currentSubMission;
    List<List<TextPartition>> texts;
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
        list.Add(new TextPartition("Well done! but don you think it was too exhausting you need to roll 8 times\nthink that you have that in the code", ""));
        list.Add(new TextPartition("That you need to repeat your code 8 time for example:", "public static void main(String[] args) {\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\troll();//not an official coomand\n\t}"));
        list.Add(new TextPartition("So let try something different, Now the code lokes much better", "public static void main(String[] args) {\n\tfor (int i = 0; i < 8; i++) {\n\t\troll();//not an official coomand\n\t}\n}"));
        list.Add(new TextPartition("Now lets look on what we see here we run the roll command 8 time but in one line\nwe have the for ", "public static void main(String[] args) {\n\tfor (int i = 0; i < 8; i++) {\n\t\troll();//not an official coomand\n\t}\n}"));
        canTalk = true;
        texts.Add(list);
        currentSubMission = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTalk && other.tag == "Player")
        {
            canTalk = false;
            currentSubText = 0;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            canvasMission.SetActive(true);
            talkingText.text = texts[currentSubMission][currentSubText].talking;
            practicalText.text = texts[currentSubMission][currentSubText].practical;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
