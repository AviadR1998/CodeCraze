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

    int rolling;
    bool canRoll;
    float currentTime, targetTime;
    // Start is called before the first frame update
    void Start()
    {
        canRoll = true;
        currentTime = targetTime = rolling = 0;
    }

    private IEnumerator delayPress()
    {
        yield return new WaitForSeconds(0.5f);
        canRoll = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && rolling < 8)
        {
            orderCanvas.SetActive(true);
            orderPanel.SetActive(true);
            practicalPanel.SetActive(false);
            orderText.text = "press 'R' button to roll the wheel!";
            if (Input.GetKeyDown("r") && canRoll)
            {
                canRoll = false;
                StartCoroutine(delayPress());
                targetTime += 15;
                ++rolling;
                //Wheel.transform.rotation = Quaternion.Euler(0, 0, 60 * ++rolling);
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
            Wheel.transform.Rotate(0, 0, 4);
        }
        else
        {
            if (rolling == 8)
            {
                soccesSound.Play();
                orderCanvas.SetActive(false);
                Movement.mission = child;
                AdminMission.endOk = redButton.GetComponent<ButtonWheel>().enabled = false;
                AdminMission.canTalk = true;
            }
        }
    }
}
