using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic : MonoBehaviour
{
    public GameObject[] trafficLight;
    public GameObject[] carLight;
    public GameObject[] walkLight;
    private int trafficRound, colorState;

    private Material normalRed;
    private Material normalGreen;
    private Material glowingRed;
    private Material glowingGreen;

    void swapLight(GameObject red, GameObject green, int state)
    {
        if (state == 1)
        {
            red.GetComponent<Renderer>().material = normalRed;
            green.GetComponent<Renderer>().material = glowingGreen;
        } 
        else
        {
            red.GetComponent<Renderer>().material = glowingRed;
            green.GetComponent<Renderer>().material = normalGreen;
        }
    }

    void changeLight()
    {
        switch (trafficRound)
        {
            case 0:
                trafficLight[0].transform.position += new Vector3(0, 2 * colorState, 0);
                swapLight(carLight[0], carLight[1], colorState);
                swapLight(walkLight[4], walkLight[5], colorState);
                swapLight(walkLight[6], walkLight[7], colorState);
                swapLight(walkLight[12], walkLight[13], colorState);
                swapLight(walkLight[14], walkLight[15], colorState);

                trafficLight[5].transform.position += new Vector3(0, 2 * colorState, 0);
                swapLight(carLight[10], carLight[11], colorState);
                swapLight(walkLight[24], walkLight[25], colorState);
                swapLight(walkLight[26], walkLight[27], colorState);
                break;
            case 1:
                trafficLight[1].transform.position += new Vector3(0, 2 * colorState, 0);
                swapLight(carLight[2], carLight[3], colorState);
                swapLight(walkLight[0], walkLight[1], colorState);
                swapLight(walkLight[2], walkLight[3], colorState);
                swapLight(walkLight[8], walkLight[9], colorState);
                swapLight(walkLight[10], walkLight[11], colorState);

                trafficLight[4].transform.position += new Vector3(0, 2 * colorState, 0);
                trafficLight[6].transform.position += new Vector3(0, 2 * colorState, 0);
                swapLight(carLight[8], carLight[9], colorState);
                swapLight(carLight[12], carLight[13], colorState);
                swapLight(walkLight[28], walkLight[29], colorState);
                swapLight(walkLight[31], walkLight[31], colorState);
                break;
            case 2:
                trafficLight[0].transform.position += new Vector3(0, 2 * colorState, 0);
                trafficLight[2].transform.position += new Vector3(0, 2 * colorState, 0);
                swapLight(carLight[0], carLight[1], colorState);
                swapLight(carLight[4], carLight[5], colorState);
                swapLight(walkLight[4], walkLight[5], colorState);
                swapLight(walkLight[6], walkLight[7], colorState);

                trafficLight[7].transform.position += new Vector3(0, 2 * colorState, 0);
                swapLight(carLight[14], carLight[15], colorState);
                swapLight(walkLight[16], walkLight[17], colorState);
                swapLight(walkLight[18], walkLight[19], colorState);
                swapLight(walkLight[24], walkLight[25], colorState);
                swapLight(walkLight[26], walkLight[27], colorState);
                break;
            case 3:
                trafficLight[3].transform.position += new Vector3(0, 2 * colorState, 0);
                swapLight(carLight[6], carLight[7], colorState);
                swapLight(walkLight[0], walkLight[1], colorState);
                swapLight(walkLight[2], walkLight[3], colorState);

                trafficLight[6].transform.position += new Vector3(0, 2 * colorState, 0);
                swapLight(carLight[12], carLight[13], colorState);
                swapLight(walkLight[20], walkLight[21], colorState);
                swapLight(walkLight[22], walkLight[23], colorState);
                swapLight(walkLight[28], walkLight[29], colorState);
                swapLight(walkLight[31], walkLight[31], colorState);
                break;
            default:
                break;
        }
        if (colorState == -1)
        {
            trafficRound = (trafficRound + 1) % 4;
        }
        colorState *= -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        trafficRound = 0;
        colorState = 1;
        InvokeRepeating("changeLight", 1f, 3.5f);
        InvokeRepeating("changeLight", 1.75f, 3.5f);

        normalRed = Resources.Load("Red", typeof(Material)) as Material;
        normalGreen = Resources.Load("Green", typeof(Material)) as Material;
        glowingRed = Resources.Load("GlowingRed", typeof(Material)) as Material;
        glowingGreen = Resources.Load("GlowingGreen", typeof(Material)) as Material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
