using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonWheel : MonoBehaviour
{
    public GameObject Wheel;
    public GameObject redButton;
    public GameObject orderCanvas;
    public GameObject orderPanel;
    public GameObject practicalPanel;
    public TMP_Text orderText;
    public GameObject child;
    public AudioSource soccesSound;

    public static int rolling;
    bool canRoll;
    float currentTime, targetTime;

    const int ROLLING_NUMBER = 8, TARGET_TIME = 15, ROTATE = 4;
    const float DELAY = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        canRoll = true;
        currentTime = targetTime = rolling = 0;
    }

    private IEnumerator delayPress()
    {
        yield return new WaitForSeconds(DELAY);
        canRoll = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && rolling < ROLLING_NUMBER)
        {
            orderCanvas.SetActive(true);
            orderPanel.SetActive(true);
            practicalPanel.SetActive(false);
            orderText.text = "press 'R' button to roll the wheel!";
            if (Input.GetKeyDown("r") && canRoll)
            {
                canRoll = false;
                StartCoroutine(delayPress());
                targetTime += TARGET_TIME;
                ++rolling;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        orderCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime < targetTime)
        {
            currentTime++;
            Wheel.transform.Rotate(0, 0, ROTATE);
        }
        else
        {
            if (rolling == ROLLING_NUMBER)
            {
                soccesSound.Play();
                orderCanvas.SetActive(false);
                Movement.mission = child;
                AdminMission.canTalk = true;
            }
        }
    }
}
