using UnityEngine;


//this script automaticaly turn off object after specific time
public class TurnOffAuto : MonoBehaviour
{
    public float turnOffAfter = 2f;
    private bool startTurnOff = false;


    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy && !startTurnOff)
        {
            startTurnOff = true;
            Invoke("turnOff", turnOffAfter);
        }
    }

    private void turnOff()
    {
        gameObject.SetActive(false);
        startTurnOff = false;
    }
}
