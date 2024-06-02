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
    public GameObject player;
    public GameObject talkingPanel;
    public TMP_Text talkingText;
    public TMP_Text practicalText;
    public AudioSource honk;
    static public bool canTalk;

    List<EndFunc> funcs;  
    int currentSubMission, currentSubText;
    bool honkScene;
    List<List<TextPartition>> texts;
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
        list.Add(new TextPartition("but before that I want to thank you by teaching you a new programing skil.\n", ""));
        list.Add(new TextPartition("Let look what we have been in here? we saw somethink like the while command.\n", ""));
        list.Add(new TextPartition("Lets make a pseudocode for it!! And that's how you write a while statement!!", "public static void main(String[] args) {\n\ttheGateIsDown = true;\n\twhile(theGateIsDown) {\n\t\tSystem.out.println(\"Help!\");\n\t\thonk();//not an official coomand\n\t}\n}"));
        list.Add(new TextPartition("Let see another example. We see here....", ""));
        list.Add(new TextPartition("Lets start parctice!", ""));
        texts.Add(list);
        currentSubMission = 0;
        honkScene = canTalk = true;
        funcs = new List<EndFunc>();
        funcs.Add(introducing);
    }

    void introducing()
    {
        talkingPanel.SetActive(false);
        ticketMachine.GetComponent<GateScript>().enabled = true;
        currentSubMission++;
        player.GetComponent<Movement>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Movement.mission = ticketMachine;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTalk && other.tag == "Player")
        {
            currentSubText = 0;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            /*switch (currentSubMission)
            {
                case 0:
                    canvasMission.SetActive(true);
                    player.GetComponent<Movement>().enabled = false;
                    talkingText.text = texts[currentSubMission][currentSubText].talking;
                    practicalText.text = texts[currentSubMission][currentSubText].practical;
                    canTalk = false;
                    break;
                case 1:
                    talkingPanel.SetActive(true);
                    player.GetComponent<Movement>().enabled = false;
                    talkingText.text = texts[currentSubMission][currentSubText].talking;
                    practicalText.text = texts[currentSubMission][currentSubText].practical;
                    canTalk = false;
                break;
            }*/
            canvasMission.SetActive(true);
            talkingPanel.SetActive(true);
            player.GetComponent<Movement>().enabled = false;
            talkingText.text = texts[currentSubMission][currentSubText].talking;
            practicalText.text = texts[currentSubMission][currentSubText].practical;
            canTalk = false;
        }
    }

    public void ok()
    {
        currentSubText++;
        if (honkScene && currentSubText == texts[currentSubMission].Count - 1)
        {
            honk.Play();
            honkScene = false;
        }
        if (currentSubMission == 0 && texts[currentSubMission].Count == currentSubText)
        {
            funcs[currentSubMission]();
            return;
        }
        talkingText.text = texts[currentSubMission][currentSubText].talking;
        practicalText.text = texts[currentSubMission][currentSubText].practical;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
