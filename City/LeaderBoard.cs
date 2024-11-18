using SocketIOClient.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class PlayerScore
{
    public string username;
    public int score;
}

[Serializable]
public class PlayerScoreList
{
    public List<PlayerScore> players;
}

public class LeaderBoard : MonoBehaviour
{
    public TMP_Text[] rows;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator getLeaderBoard()
    {
        UnityWebRequest requestQuestions = UnityWebRequest.Get("http://" + MainMenu.serverIp + ":5000/api/Users/TopScore");
        requestQuestions.SetRequestHeader("Authorization", "Bearer " + Login.token);
        yield return requestQuestions.SendWebRequest();
        if (requestQuestions.result == UnityWebRequest.Result.Success)
        {
            PlayerScoreList questionList = JsonUtility.FromJson<PlayerScoreList>(requestQuestions.downloadHandler.text);
            for (int i = 0; i < questionList.players.Count; i++)
            {
                rows[i].text = questionList.players[i].username + " - " + questionList.players[i].score;
            }
        }
        else
        {
            print("error getting questions");
        }
    }

    void OnEnable()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i].text = "";
        }
        StartCoroutine(getLeaderBoard());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
