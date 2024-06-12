using System;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject endRaceMenu;
    public GameObject questionAnswersPanel;
    public TMP_Text questionsText;
    public TMP_Text answersText;
    public AudioSource startSound;
    public AudioSource backgroundSound;

    List<string> questionList;
    List<string> answersList;
    List<string> rightAnswerList;
    int currentCorrectAnswer;
    string tokenGemini;
    private bool canDrive, finish, first, canPress, listReady;
    float driveAxisZ;
    int questionNumber;

    private IEnumerator delayPress()
    {
        yield return new WaitForSeconds(0.5f);
        canPress = true;
    }

    void answerQuestion()
    {
        questionsText.text = questionList[questionNumber];
        answersText.text = answersList[questionNumber];
        for (int i = 0; i < rightAnswerList[questionNumber].Length; i++)
        {
            if (rightAnswerList[questionNumber][i] >= '1' && rightAnswerList[questionNumber][i] <= '4')
            {
                currentCorrectAnswer = rightAnswerList[questionNumber][i] - '0';
            }
        }
        questionNumber++;
    }


    List<int> splitByStr(string response, string str)
    {
        List<int> list = new List<int>();
        for (int i = 0; ; i += str.Length)
        {
            i = response.IndexOf(str, i);
            if (i == -1)
            {
                return list;
            }
            list.Add(i);
        }
    }

    void splitResponse(string response)
    {
        List<int> indexesStars = splitByStr(response, "**");
        questionList = new List<string>();
        answersList = new List<string>();
        rightAnswerList = new List<string>();
        for (int i = 1; i < indexesStars.Count - 1; i += 3)
        {
            /*if (indexesStars[i + 1] - indexesStars[i] - 3 > 300)
            {
                i += 5;
                continue;
            }
            if(indexesStars[i + 1] - indexesStars[i] - 3 < 5)
            {
                i += 2;
            }
            print("passif");*/
            questionList.Add(response.Substring(indexesStars[i] + 3, indexesStars[i + 1] - indexesStars[i] - 3));
            //print("passadd1");
            i += 2;
            answersList.Add(response.Substring(indexesStars[i] + 3, indexesStars[i + 1] - indexesStars[i] - 3));
            /*print("passadd2");
            if (indexesStars[i + 1] - indexesStars[i] - 2 > 150)
            {
                questionList.RemoveAt(questionList.Count - 1);
                answersList.RemoveAt(answersList.Count - 1);
                rightAnswerList.RemoveAt(rightAnswerList.Count - 1);
            }*/
            i += 3;
            rightAnswerList.Add(response.Substring(indexesStars[i] + 2, indexesStars[i + 1] - indexesStars[i] - 2));
            //print("passadd3");
        }
        listReady = true;
        answerQuestion();
    }

    IEnumerator callGemini()
    {
        string response;
        string sndJason = "{\"contents\": {\"parts\": {\"text\": \"give me 10 new different easy multiple-choice programing question in java that conneceted to array, loops, if with 4 different answers that exactly 1 answer from the 4 you gave is correct and give me the answer. please write me your response in the next format: **question**:... **answers(dont help here or gave the answer)**: 1).... 2).... 3).... 4).... **the answer is**: **1/2/3/4(dont forget double asterisks and only 1 answer from the 4 is correct)** [explanation:...] please keep your all response in the format i mentioned it is importent. dont add any double or more asterisks except the places i told you it is imporatant. please notice that each answer will be at most 3 lines and the question will be at most 10 lines.\"}}}";
        Dictionary<string,string> headers = new Dictionary<string,string>();
        headers.Add("Content-Type", "application/json");
        WWW www = new WWW("https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=" + tokenGemini, System.Text.Encoding.UTF8.GetBytes(sndJason), headers); //pro
        yield return www;
        print(JsonUtility.FromJson<GiminiJSON>(www.text).candidates[0].content.parts[0].text);
        splitResponse(JsonUtility.FromJson<GiminiJSON>(www.text).candidates[0].content.parts[0].text);
    }

    private IEnumerator delayDrive()
    {
        yield return new WaitForSeconds(4f);
        canDrive = true;
        backgroundSound.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        tokenGemini = "AIzaSyBPTHuQh9sVyLphL92oIHsF3Aognp0MHn0";
    }

    void OnEnable()
    {
        questionsText.text = "loading questions...";
        answersText.text = "loading answers...";
        canPress = true;
        currentCorrectAnswer = -1;
        questionNumber = 0;
        startSound.Play();
        backgroundSound.Stop();
        speed = 10f;
        listReady = first = finish = canDrive = false;
        driveAxisZ = 0f;
        raceDetails.SetActive(true);
        raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: 10";
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
        if (!first)
        {
            StartCoroutine(callGemini());
            GameObject.Find("Main Camera").transform.position += new Vector3(0, 2f, 0);
            GameObject.Find("Main Camera").transform.LookAt(GameObject.Find("EndRace(unseen)").transform);
            first = true;
        }

        if (listReady && canPress && questionNumber < 10 && !finish && (Input.GetKey(KeyCode.Keypad1) || Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.Keypad3) || Input.GetKey(KeyCode.Keypad4)))
        {
            bool correct = false;
            canPress = false;
            if (Input.GetKey(KeyCode.Keypad1) && currentCorrectAnswer == 1)
            {
                speed += 10;
                raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                correct = true;
            }
            if (Input.GetKey(KeyCode.Keypad2) && currentCorrectAnswer == 2)
            {
                speed += 10;
                raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                correct = true;
            }
            if (Input.GetKey(KeyCode.Keypad3) && currentCorrectAnswer == 3)
            {
                speed += 10;
                raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                correct = true;
            }
            if (Input.GetKey(KeyCode.Keypad4) && currentCorrectAnswer == 4)
            {
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
            answerQuestion();
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
}
