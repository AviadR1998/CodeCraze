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

    private void OnTriggerEnter(Collider other)
    {
        if (!isWin)
        {
            endRaceMenu.GetComponentInChildren<TMP_Text>().text = other.tag.ToString() + " Win!!!";
            isWin = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
