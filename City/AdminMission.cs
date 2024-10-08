using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdminMission : MonoBehaviour
{
    public delegate void EndFunc();
    public static bool canTalk, endOk = false;
    public static EndFunc okFunc;
    public static int currentSubMission, currentSubText;
    public static List<List<TextPartition>> texts;
    public TMP_Text talkingText;
    public TMP_Text practicalText;

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

}
