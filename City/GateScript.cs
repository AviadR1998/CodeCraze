using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    public GameObject ticketMachine;
    public GameObject orderPanel;
    public GameObject orderCanvas;
    public TMP_Text orderText;
    public GameObject gate;
    public TMP_Text helpMessage;
    public AudioSource honk;

    void angryDriver()
    {
        honk.Play();
        if (helpMessage.text == "The driver: Help! Help! Help!")
        {
            helpMessage.text = "The driver: Help!";
        }
        else
        {
            helpMessage.text += " Help!";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        helpMessage.text = "The driver: Help!";
        InvokeRepeating("angryDriver", 0f, 2f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            orderPanel.SetActive(true);
            orderText.text = "press 'R' button to open the gate!";
            if (Input.GetKeyDown("r"))
            {
                Quaternion target = Quaternion.Euler(240, 0, 0);
                gate.transform.rotation = Quaternion.Slerp(transform.rotation, target, 5f);
                Movement.mission = GameObject.Find("WhileNPC");
                CancelInvoke("angryDriver");
                helpMessage.text = "The driver: Thanks!!!";
                WhileMissions.canTalk = true;
                orderCanvas.SetActive(false);
                ticketMachine.GetComponent<GateScript>().enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        orderPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
