using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdminMission : MonoBehaviour
{
    public delegate void EndFunc();
    public static bool canTalk, endOk = false, geminiActivate = false, readQuestion = false;
    public static EndFunc okFunc;
    public static int currentSubMission, currentSubText;
    public static List<List<TextPartition>> texts;
    public TMP_Text talkingText;
    public TMP_Text practicalText;
    public static Stack<string> questions;
    public static Stack<string> answers;
    public static Stack<string> rightAnswers;

    public void ok()
    {
        currentSubText++;
        okFunc();
        if (!endOk)
        {
            talkingText.text = texts[currentSubMission][currentSubText].talking;
            practicalText.text = texts[currentSubMission][currentSubText].practical;
        }
        endOk = false;
    }

    void Start()
    {
        questions = new Stack<string>();
        answers = new Stack<string>();
        rightAnswers = new Stack<string>();
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

    string removeWhiteLetters(string str)
    {
        while (str[0] == ' ' || str[0] == '\n')
        {
            str = str.Substring(1);
        }
        return str;
    }

    void splitResponse(string response)
    {
        List<int> indexesStars = splitByStr(response, "**");
        for (int i = 1; i < indexesStars.Count - 1; i += 3)
        {
            if (indexesStars[i + 1] - indexesStars[i] - 3 > 300 || indexesStars[i + 1] - indexesStars[i] - 3 < 10)
            {
                i += 5;
                continue;
            }
            questions.Push(removeWhiteLetters(response.Substring(indexesStars[i] + 3, indexesStars[i + 1] - indexesStars[i] - 3)));
            i += 2;
            if (indexesStars[i + 1] - indexesStars[i] - 2 > 220 || indexesStars[i + 1] - indexesStars[i] - 2 < 10)
            {
                questions.Pop();
                i += 3;
                continue;
            }
            answers.Push(removeWhiteLetters(response.Substring(indexesStars[i] + 3, indexesStars[i + 1] - indexesStars[i] - 3)));
            i += 3;
            rightAnswers.Push(response.Substring(indexesStars[i] + 2, 1));
        }
        //listReady = true;
    }

    IEnumerator callGemini()
    {
        string response, tokenGemini = "AIzaSyBPTHuQh9sVyLphL92oIHsF3Aognp0MHn0";
        string sndJason = "{\"contents\": {\"parts\": {\"text\": \"give me 10 new different easy multiple-choice programing question(max length 200 notes) in java that conneceted to array, loops, if with 4 different answers(max length 70 notes) that exactly 1 answer from the 4 you gave is correct and give me the answer. please write me your response in the next format(the format is most important!!!): **question**:... **answers(dont help here or gave the answer)**: 1)....\n2)....\n3)....\n4)....\n **the answer is**: **1/2/3/4(dont forget double asterisks and only 1 answer from the 4 is correct)** i don't want explanation. please keep your all response in the format i mentioned it is very importent. don't add any double or more asterisks except the places i told you it is imporatant!!!!.\"}}}";
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        WWW www = new WWW("https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro:generateContent?key=" + tokenGemini, System.Text.Encoding.UTF8.GetBytes(sndJason), headers); //pro
        yield return www;
        if (www.error != null)
        {
            print(www.error);
        }
        else
        {
            print(JsonUtility.FromJson<GiminiJSON>(www.text).candidates[0].content.parts[0].text);
            splitResponse(JsonUtility.FromJson<GiminiJSON>(www.text).candidates[0].content.parts[0].text);
        }
        geminiActivate = false;
    }

    void Update()
    {
        if (questions.Count < 20 && !geminiActivate && !readQuestion)
        {
            geminiActivate = true;
            StartCoroutine(callGemini());
        }
    }

}
