using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

using Newtonsoft.Json;
using Unity.VisualScripting;

public class OpenAIResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("object")]
    public string Object { get; set; }

    [JsonProperty("created")]
    public int Created { get; set; }

    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("choices")]
    public List<Choice> Choices { get; set; }

    [JsonProperty("usage")]
    public Usage Usage { get; set; }
}

public class Choice
{
    [JsonProperty("index")]
    public int Index { get; set; }

    [JsonProperty("message")]
    public Message Message { get; set; }

    [JsonProperty("finish_reason")]
    public string FinishReason { get; set; }
}

public class Message
{
    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
}

public class Usage
{
    [JsonProperty("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonProperty("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonProperty("total_tokens")]
    public int TotalTokens { get; set; }
}


public class AdminMission : MonoBehaviour
{
    public delegate void EndFunc();
    public static bool canTalk, endOk = false, geminiActivate = false, loadAll = false;
    public static EndFunc okFunc;
    public static int currentSubMission, currentSubText;
    public static List<List<TextPartition>> texts;
    public TMP_Text talkingText;
    public TMP_Text practicalText;
    public TMP_Text questioNumberText;
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
    }

    void OnEnable()
    {
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

    string addEnter(string str)
    {
        for (int i = 1; i < 5; i++)
        {
            str = str.Replace(" " + i + ")", "\n" + i + ")");
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
            answers.Push(addEnter(removeWhiteLetters(response.Substring(indexesStars[i] + 3, indexesStars[i + 1] - indexesStars[i] - 3))));
            i += 3;
            if (i + 1 >= indexesStars.Count || indexesStars[i + 1] - indexesStars[i] - 2 > 2)
            {
                string checkAnswer = response.Substring(indexesStars[i - 1] + 3, 2);
                if (checkAnswer == " 1" || checkAnswer == " 2" || checkAnswer == " 3" || checkAnswer == " 4")
                {
                    rightAnswers.Push(checkAnswer[1].ToString());
                    i -= 2;
                    continue;
                }
                questions.Pop();
                answers.Pop();
                i += 3;
                continue;

            }
            rightAnswers.Push(response.Substring(indexesStars[i] + 2, 1));
        }
        //listReady = true;
    }

    IEnumerator callGemini()
    {
        string response, tokenGemini = "AIzaSyBPTHuQh9sVyLphL92oIHsF3Aognp0MHn0";
        string sndJason = "{\"contents\": {\"parts\": {\"text\": \"give me 10 new different easy multiple-choice programing question(max length 200 notes) in java that conneceted to " + MainMenu.topicListSaved + " with 4 different answers(max length 70 notes) that exactly 1 answer from the 4 you gave is correct and give me the answer. please write me your response in the next format(the format is most important!!!): **question**:... **answers(dont help here or gave the answer)**: 1).... 2).... 3).... 4).... **the answer is**: **correct number answer** i don't want explanation. please keep your all response in the format i mentioned it is very importent. don't add any double or more asterisks except the places i told you it is imporatant!!!!.\"}}}";
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        WWW www = new WWW("https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=" + tokenGemini, System.Text.Encoding.UTF8.GetBytes(sndJason), headers); //pro
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
        questioNumberText.text = "questions: " + (questions.Count > 10 ? 10 : questions.Count) + "/10";
        loadAll = questions.Count >= 10;
        MainMenu.activateRace = !loadAll;
    }

    IEnumerator callGpt()
    {
        string apiKey = "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";
        string url = "https://api.openai.com/v1/chat/completions";

        var jsonData = new
        {
            //model = "gpt-4",
            model = "gpt-3.5-turbo",

            messages = new[]
            {
                new { role = "system", content = "You are an assistant helping to create educational questions for a Free Play mode in a Unity programming game for children. The game is designed to teach Java programming concepts." },
                new { role = "user", content = "give me 10 new different easy multiple-choice programing question(max length 200 notes) in java that related to " + MainMenu.topicListSaved + " with 4 different answers(max length 70 notes) that exactly 1 answer from the 4 you gave is correct and give me the answer. please write me your response in the next format(the format is most important!!!): **question**:... **answers(dont help here or gave the answer)**: 1).... 2).... 3).... 4).... **the answer is**: **1/2/3/4(dont forget double asterisks and only 1 answer from the 4 is correct)** i don't want explanation. please keep your all response in the format i mentioned it is very importent. don't add any double or more asterisks except the places i told you it is imporatant!!!!." }
            },
            max_tokens = 800
        };

        //string json = JsonUtility.ToJson(jsonData);
        string json = JsonConvert.SerializeObject(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        Debug.Log("Topic: " + MainMenu.topicListSaved);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            print(JsonConvert.DeserializeObject<OpenAIResponse>(request.downloadHandler.text).Choices[0].Message.Content);
            splitResponse(JsonConvert.DeserializeObject<OpenAIResponse>(request.downloadHandler.text).Choices[0].Message.Content);
        }
        else
        {
            Debug.LogError("ONE");
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
        geminiActivate = false;
        questioNumberText.text = "questions: " + (questions.Count > 10 ? 10 : questions.Count) + "/10";
        loadAll = questions.Count >= 10;
        MainMenu.activateRace = !loadAll;
    }

    void Update()
    {
        if (MainMenu.activateRace && questions.Count < 10 && !geminiActivate && !loadAll)
        {
            geminiActivate = true;
            StartCoroutine(callGemini());
        }
    }

}