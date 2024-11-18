using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class FinishLine : MonoBehaviour
{
    public static string winner;
    public GameObject endRaceMenu;
    private bool isWin;
    // Start is called before the first frame update
    void OnEnable()
    {
        isWin = false;
    }

    private IEnumerator delayPress()
    {
        yield return new WaitForSeconds(0.5f);
        CancelInvoke("sndLocation");
        RaceMovment.cancelFinish = true;
    }

    IEnumerator addScore()
    {
        WWWForm form = new WWWForm();
        form.AddField("score", 10);
        using (UnityWebRequest request = UnityWebRequest.Post("http://" + MainMenu.serverIp + ":5000/api/Users/AddScore", form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + Login.token);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {

            }
            else
            {
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isWin)
        {
            StartCoroutine(delayPress());
            RoomsMenu.socket.Emit("finish", "nir");
            string winner = "Player";
            if (other.tag.ToString() == "Player")
            {
                winner = "nir";
                StartCoroutine(addScore());
            }
            else
            {
                winner = RoomsMenu.opponent;
            }
            endRaceMenu.GetComponentInChildren<TMP_Text>().text = winner + " Win!!!";
            isWin = true;
            RoomsMenu.multiplayerStart = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
