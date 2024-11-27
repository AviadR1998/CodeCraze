using System;
using TMPro;
using UnityEngine;

public class MissionCanvas : MonoBehaviour
{

    public TextMeshProUGUI innerText;
    public String[] stringArray;
    private int stringNum = 0;
    public GameObject nextCanvas;
    public bool turnOffWhenFinish = false;

    void OnEnable()
    {
        stringNum = 0;
        innerText.text = stringArray[stringNum++];
        innerText.text = innerText.text.Replace("\\n", "\n");
    }

    public void NextText()
    {
        if (stringNum < stringArray.Length)
        {
            innerText.text = stringArray[stringNum++];
            innerText.text = innerText.text.Replace("\\n", "\n");
        }
        else
        {
            if (turnOffWhenFinish)
            {
                gameObject.SetActive(false);
            }

            if (nextCanvas != null)
            {
                nextCanvas.SetActive(true);
            }
        }
    }


}
