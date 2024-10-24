using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
