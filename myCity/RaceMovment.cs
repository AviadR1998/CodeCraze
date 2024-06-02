using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public AudioSource startSound;
    public AudioSource backgroundSound;
    
    string tokenGemini;
    private bool canDrive, finish, first;
    float driveAxisZ;

    void answerQuestion()
    {
        //StartCoroutine(callGemini());
        int rnd = UnityEngine.Random.Range(0, 5);
        if (rnd == 0)
        {
            if (speed > 10)
            {
                speed -= 5;
                raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
                return;
            }
            return;
        }
        else
        {
            speed += 10;
            raceDetails.GetComponentInChildren<TMP_Text>().text = "Speed: " + speed;
        }
    }

    IEnumerator callGemini()
    {
        string response;
        string sndJason = "{\"contents\": {\"parts\": {\"text\": \"give me 10 new(try to think about new question) different easy multiple-choice programing question in java with 4 different answers that only 1 answer from the 4 you gave is correct(please check if the answer is correct) and give me the answer. please write me your response in the next format: question (1/2/3/4/5/6/7/8/9/10):... answers: A).... B).... C).... D).... the answer is: (A/B/C/D)\"}}}";
        Dictionary<string,string> headers = new Dictionary<string,string>();
        headers.Add("Content-Type", "application/json");
        WWW www = new WWW("https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=" + tokenGemini, System.Text.Encoding.UTF8.GetBytes(sndJason), headers); //pro
        yield return www;
        print(JsonUtility.FromJson<GiminiJSON>(www.text).candidates[0].content.parts[0].text);
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
        InvokeRepeating("answerQuestion", 5f, 7f);
    }

    void OnEnable()
    {
        startSound.Play();
        backgroundSound.Stop();
        speed = 10f;
        first = finish = canDrive = false;
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
