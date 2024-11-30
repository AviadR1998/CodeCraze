using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using Unity.VisualScripting;
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

    const int DEC_SPEED = 5, BASE_SPEED = 10, QUESTION_NUMBER = 10, ADD_SPEED = 10, SPEED_Z = 6, OBS_NUMBER = 45, CNT_SAME_SIDE = 3, SPACE_DIS = 35, START_SENDING = 4;
    const float DELAY = 0.5f, START_X = -606, START_Y = 10.1f, RIGHT_Z = 368.5f, LEFT_Z = 362f, OBS_SIZE_X = 2.5f, OBS_SIZE_Y = 3.5f, OBS_SIZE_Z = 5f, REPEAT_SENDING = 0.2f, MAX_Z = 370, MIN_Z = 360.5f;
    private IEnumerator delayPress()
    {
        yield return new WaitForSeconds(DELAY);
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
        float distanceX = 0, startX = START_X, startY = START_Y, rightZ = RIGHT_Z, leftZ = LEFT_Z;
        int rndNum, lastRnd = -1, cntSameSide = 0;
        string obsList = "";
        if (RoomsMenu.multiplayerStart)
        {
            while (RoomsMenu.obsList == "") { }
            obsList = RoomsMenu.obsList;
        }
        for (int i = 0; i < OBS_NUMBER; i++)
        {
            GameObject newObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newObj.name = "Obs-" + i;
            newObj.AddComponent<Rigidbody>();
            newObj.GetComponent<Rigidbody>().useGravity = false;
            newObj.GetComponent<Rigidbody>().isKinematic = true;
            newObj.GetComponent<BoxCollider>().isTrigger = true;
            newObj.tag = "Obstacle";
            newObj.transform.localScale = new Vector3(OBS_SIZE_X, OBS_SIZE_Y, OBS_SIZE_Z);
            if (RoomsMenu.multiplayerStart)
            {
                rndNum = obsList[i] - '0';
            }
            else
            {
                rndNum = UnityEngine.Random.Range(0, 2);
            }
            if (cntSameSide != CNT_SAME_SIDE)
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
            distanceX += SPACE_DIS;
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
            InvokeRepeating("sndLocation", START_SENDING, REPEAT_SENDING);
        }
        createObs();
        canPress = true;
        currentCorrectAnswer = -1;
        questionNumber = 0;
        startSound.Play();
        backgroundSound.Stop();
        speed = BASE_SPEED;
        cancelFinish = listReady = first = finish = canDrive = cordBool = false;
        driveAxisZ = 0f;
        raceDetails.SetActive(true);
        raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: 10";
        questionList = new List<string>();
        answersList = new List<string>();
        rightAnswerList = new List<string>();
        for (int i = 0; i < QUESTION_NUMBER; i++)
        {
            questionList.Add(AdminMission.questions.Pop());
            answersList.Add(AdminMission.answers.Pop());
            rightAnswerList.Add(AdminMission.rightAnswers.Pop());
        }
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
        if (other.tag.ToString() == "Obstacle" && speed > BASE_SPEED)
        {
            speed -= DEC_SPEED;
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
            float newZ = Mathf.MoveTowards(raceCar.transform.position.z, recv.GetValue<float>(2), SPEED_Z * Time.deltaTime);
            raceCar.transform.position = new Vector3(newX, newY, newZ);
        }
        if (!first)
        {
            PauseMenu.canPause = false;
            GameObject.Find("Main Camera").transform.position += new Vector3(0, 2f, 0);
            GameObject.Find("Main Camera").transform.LookAt(GameObject.Find("EndRace(unseen)").transform);
            first = true;
        }

        if (canDrive && canPress && questionNumber < questionList.Count + 1 && !finish && (Input.GetKey(KeyCode.Keypad1) || Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.Keypad3) || Input.GetKey(KeyCode.Keypad4) || Input.GetKey("1") || Input.GetKey("2") || Input.GetKey("3") || Input.GetKey("4")))
        {
            bool correct = false;
            canPress = false;
            if ((Input.GetKey(KeyCode.Keypad1) || Input.GetKey("1")) && currentCorrectAnswer == 1)
            {
                speed += ADD_SPEED;
                raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                correct = true;
            }
            if ((Input.GetKey(KeyCode.Keypad2) || Input.GetKey("2")) && currentCorrectAnswer == 2)
            {
                speed += ADD_SPEED;
                raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                correct = true;
            }
            if ((Input.GetKey(KeyCode.Keypad3) || Input.GetKey("3")) && currentCorrectAnswer == 3)
            {
                speed += ADD_SPEED;
                raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                correct = true;
            }
            if ((Input.GetKey(KeyCode.Keypad4) || Input.GetKey("4")) && currentCorrectAnswer == 4)
            {
                speed += ADD_SPEED;
                raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                correct = true;
            }
            if (!correct)
            {
                if (speed > BASE_SPEED)
                {
                    speed -= DEC_SPEED;
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
            speed += DEC_SPEED;
        }
        if (Input.GetKey("m"))
        {
            speed -= DEC_SPEED;
        }
        if ((Input.GetKey("right") || Input.GetKey("d")) && player.transform.position.z > MIN_Z)
        {
            driveAxisZ = -SPEED_Z;
        }
        else
        {
            if ((Input.GetKey("left") || Input.GetKey("a")) && player.transform.position.z < MAX_Z)
            {
                driveAxisZ = SPEED_Z;
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
