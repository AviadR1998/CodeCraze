using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Parts
{
    public string text;
}
[Serializable]
public class Contents
{
    public List<Parts> parts;
    public string role;
}
[Serializable]
public class SafetyRating
{
    public string category;
    public string probability;
}
[Serializable]
public class Candidates
{
    public Contents content;
    public string finishReason;
    public int index;
    public List<SafetyRating> safetyRatings;
}

[Serializable]
public class PromptFeedback
{
    public List<SafetyRating> safetyRatings;
}

[Serializable]
public class GiminiJSON
{
    public List<Candidates> candidates;
    public PromptFeedback promptFeedback;
}

public class RaceMovment : MonoBehaviour
{
    public float speed;
    public GameObject player;
    public GameObject raceField;
    public GameObject raceDetails;
    public GameObject raceCar;
    public GameObject endRaceMenu;
    public GameObject questionAnswersPanel;
    public TMP_Text questionsText;
    public TMP_Text answersText;
    public AudioSource startSound;
    public AudioSource backgroundSound;
    public GameObject[] colorsLight;
    public static List<GameObject> obstaclesR;
    public static List<GameObject> obstaclesL;
    public static List<GameObject> allObstacles;
    public static bool cancelFinish;

    List<string> questionList;
    List<string> answersList;
    List<string> rightAnswerList;
    int currentCorrectAnswer;
    string tokenGemini;
    private bool canDrive, finish, first, canPress, cordBool = false, listReady;
    SocketIOClient.SocketIOResponse recv;
    float driveAxisZ;
    int questionNumber;
    private Material normalRed, normalGreen, glowingRed, glowingGreen, normalYellow, glowingYellow;

    private IEnumerator delayPress()
    {
        yield return new WaitForSeconds(0.5f);
        canPress = true;
    }

    void answerQuestion()
    {
        questionsText.text = questionList[questionNumber];
        answersText.text = answersList[questionNumber];
        currentCorrectAnswer = rightAnswerList[questionNumber][0] - '0';
        questionNumber++;
    }

    private IEnumerator delayDrive()
    {

        yield return new WaitForSeconds(2f);
        colorsLight[2].GetComponent<Renderer>().material = glowingYellow;
        colorsLight[3].GetComponent<Renderer>().material = glowingYellow;
        yield return new WaitForSeconds(2f);
        colorsLight[0].GetComponent<Renderer>().material = normalRed;
        colorsLight[1].GetComponent<Renderer>().material = normalRed;
        colorsLight[2].GetComponent<Renderer>().material = normalYellow;
        colorsLight[3].GetComponent<Renderer>().material = normalYellow;
        colorsLight[4].GetComponent<Renderer>().material = glowingGreen;
        colorsLight[5].GetComponent<Renderer>().material = glowingGreen;

        canDrive = true;
        backgroundSound.Play();
        answerQuestion();
    }

    // Start is called before the first frame update
    void Start()
    {
        tokenGemini = "AIzaSyBPTHuQh9sVyLphL92oIHsF3Aognp0MHn0";
        RoomsMenu.socket.On("cord", data =>
        {
            recv = data;
            cordBool = true;
        });
    }

    void iniColors()
    {
        normalRed = Resources.Load("Red", typeof(Material)) as Material;
        normalGreen = Resources.Load("Green", typeof(Material)) as Material;
        glowingRed = Resources.Load("GlowingRed", typeof(Material)) as Material;
        glowingGreen = Resources.Load("GlowingGreen", typeof(Material)) as Material;
        glowingYellow = Resources.Load("GlowingYellow", typeof(Material)) as Material;
        normalYellow = Resources.Load("Yellow", typeof(Material)) as Material;
        colorsLight[0].GetComponent<Renderer>().material = glowingRed;
        colorsLight[1].GetComponent<Renderer>().material = glowingRed;
        colorsLight[2].GetComponent<Renderer>().material = normalYellow;
        colorsLight[3].GetComponent<Renderer>().material = normalYellow;
        colorsLight[4].GetComponent<Renderer>().material = normalGreen;
        colorsLight[5].GetComponent<Renderer>().material = normalGreen;
    }

    void createObs()
    {
        float distanceX = 0, startX = -606, startY = 10.1f, rightZ = 368.5f, leftZ = 362f;
        int rndNum, lastRnd = -1, cntSameSide = 0;
        string obsList = "";
        if (RoomsMenu.multiplayerStart)
        {
            while (RoomsMenu.obsList == "") { } // could fix some bugs and could crash the unity
            obsList = RoomsMenu.obsList;
        }
        for (int i = 0; i < 45; i++)
        {
            GameObject newObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newObj.name = "Obs-" + i;
            newObj.AddComponent<Rigidbody>();
            newObj.GetComponent<Rigidbody>().useGravity = false;
            newObj.GetComponent<Rigidbody>().isKinematic = true;
            newObj.GetComponent<BoxCollider>().isTrigger = true;
            newObj.tag = "Obstacle";
            newObj.transform.localScale = new Vector3(2.5f, 3.5f, 5f);
            if (RoomsMenu.multiplayerStart)
            {
                rndNum = obsList[i] - '0';
            }
            else
            {
                rndNum = UnityEngine.Random.Range(0, 2);
            }
            if (cntSameSide != 3)
            {
                cntSameSide = rndNum == lastRnd ? cntSameSide + 1 : 1;
            }
            else
            {
                rndNum = 1 - lastRnd;
                cntSameSide = 1;
            }
            lastRnd = rndNum;
            if (rndNum == 0)
            {
                newObj.transform.position = new Vector3(startX + distanceX, startY, rightZ);
                obstaclesR.Add(newObj);
                allObstacles.Add(newObj);
            }
            else
            {
                newObj.transform.position = new Vector3(startX + distanceX, startY, leftZ);
                obstaclesL.Add(newObj);
                allObstacles.Add(newObj);
            }
            distanceX += 35;
        }
        RoomsMenu.obsList = "";
    }

    void sndLocation()
    {
        RoomsMenu.socket.Emit("cord", RoomsMenu.opponent, this.transform.position.x, this.transform.position.y, this.transform.position.z, speed);
    }

    void OnEnable()
    {
        questionsText.text = "Rules:\n1. move with the arrows keys or with A, D keys\n2. avoid from the obstacles.\n3. answer the question by pressing the answer's number.\n4. finish first and have fun!!!!";
        answersText.text = "loading questions and answers...";
        iniColors();
        allObstacles = new List<GameObject>();
        obstaclesL = new List<GameObject>();
        obstaclesR = new List<GameObject>();
        if (RoomsMenu.multiplayerStart)
        {
            InvokeRepeating("sndLocation", 4f, 0.2f);
        }
        createObs();
        canPress = true;
        currentCorrectAnswer = -1;
        questionNumber = 0;
        startSound.Play();
        backgroundSound.Stop();
        speed = 10f;
        cancelFinish = listReady = first = finish = canDrive = cordBool = false;
        driveAxisZ = 0f;
        raceDetails.SetActive(true);
        raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: 10";
        questionList = new List<string>();
        answersList = new List<string>();
        rightAnswerList = new List<string>();
        AdminMission.readQuestion = true;
        //while (AdminMission.geminiActivate) { }
        for (int i = 0; i < 10; i++)
        {
            questionList.Add(AdminMission.questions.Pop());
            answersList.Add(AdminMission.answers.Pop());
            rightAnswerList.Add(AdminMission.rightAnswers.Pop());
            /*questionList.Add("aaa");
            answersList.Add("bbb");
            rightAnswerList.Add("1");*/
        }
        AdminMission.readQuestion = false;
        StartCoroutine(delayDrive());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToString() == "Finish")
        {
            finish = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            questionAnswersPanel.SetActive(false);
            endRaceMenu.SetActive(true);
        }
        if (other.tag.ToString() == "Obstacle" && speed > 10)
        {
            speed -= 5;
            raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cancelFinish)
        {
            CancelInvoke("sndLocation");
            cancelFinish = false;
        }
        if (cordBool)
        {
            float newX = Mathf.MoveTowards(raceCar.transform.position.x, recv.GetValue<float>(0), recv.GetValue<int>(3) * Time.deltaTime);
            float newY = Mathf.MoveTowards(raceCar.transform.position.y, recv.GetValue<float>(1), 0);
            float newZ = Mathf.MoveTowards(raceCar.transform.position.z, recv.GetValue<float>(2), 6 * Time.deltaTime);
            raceCar.transform.position = new Vector3(newX, newY, newZ);
            //raceCar.transform.position = Vector3.MoveTowards(raceCar.transform.position, new Vector3(recv.GetValue<float>(0), recv.GetValue<float>(1), recv.GetValue<float>(2)), Time.deltaTime * recv.GetValue<int>(3));
            //raceCar.transform.position = new Vector3(recv.GetValue<float>(0), recv.GetValue<float>(1), recv.GetValue<float>(2));
        }
        if (!first)
        {
            PauseMenu.canPause = false;
            //StartCoroutine(callGemini());
            GameObject.Find("Main Camera").transform.position += new Vector3(0, 2f, 0);
            GameObject.Find("Main Camera").transform.LookAt(GameObject.Find("EndRace(unseen)").transform);
            first = true;
        }

        if (canDrive && canPress && questionNumber < questionList.Count + 1 && !finish && (Input.GetKey(KeyCode.Keypad1) || Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.Keypad3) || Input.GetKey(KeyCode.Keypad4) || Input.GetKey("1") || Input.GetKey("2") || Input.GetKey("3") || Input.GetKey("4")))
        {
            bool correct = false;
            canPress = false;
            //print("\'" + currentCorrectAnswer + "\'");
            if ((Input.GetKey(KeyCode.Keypad1) || Input.GetKey("1")) && currentCorrectAnswer == 1)
            {
                //print("-1-");
                speed += 10;
                raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                correct = true;
            }
            if ((Input.GetKey(KeyCode.Keypad2) || Input.GetKey("2")) && currentCorrectAnswer == 2)
            {
                //print("-2-");
                speed += 10;
                raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                correct = true;
            }
            if ((Input.GetKey(KeyCode.Keypad3) || Input.GetKey("3")) && currentCorrectAnswer == 3)
            {
                //print("-3-");
                speed += 10;
                raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                correct = true;
            }
            if ((Input.GetKey(KeyCode.Keypad4) || Input.GetKey("4")) && currentCorrectAnswer == 4)
            {
                //print("-4-");
                speed += 10;
                raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                correct = true;
            }
            if (!correct)
            {
                if (speed > 10)
                {
                    speed -= 5;
                    raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                }
            }
            if (questionNumber < questionList.Count)
            {
                answerQuestion();
            }
            else
            {
                questionNumber++;
                questionsText.text = "you have only 10 question to answer";
                answersText.text = "you have only 10 question to answer";
            }
            StartCoroutine(delayPress());

        }

        if (Input.GetKey("p"))
        {
            speed += 5;
        }
        if (Input.GetKey("m"))
        {
            speed -= 5;
        }
        if ((Input.GetKey("right") || Input.GetKey("d")) && player.transform.position.z > 360.5)
        {
            driveAxisZ = -6;
        }
        else
        {
            if ((Input.GetKey("left") || Input.GetKey("a")) && player.transform.position.z < 370)
            {
                driveAxisZ = 6;
            }
            else
            {
                driveAxisZ = 0;
            }
        }
        if (canDrive && !finish)
        {
            player.transform.position += new Vector3(speed * Time.deltaTime, 0, driveAxisZ * Time.deltaTime);
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < allObstacles.Count; i++)
        {
            Destroy(allObstacles[i]);
        }
        CancelInvoke("sndLocation");
    }
}
