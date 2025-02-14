using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

public class MoveCars : MonoBehaviour
{
    public GameObject obj;
    public GameObject[] wheels;
    public GameObject redSiren, blueSiren;
    public GameObject[] points;
    public float speed;

    private Vector3 actualPoints;
    private int nextPosition;
    private bool canDrive, collideLine, collidePlayer, collideCar, siren;

    const int ROTATE = 10;
    const float DELAY = 0.28f, DELAY_SIREN = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToString() == "IfStopLine" && obj.tag.ToString() == "OrangeCar")
        {
            IfMissions.xLine = true;
        }
        if (other.tag.ToString() == "Player")
        {
            collidePlayer = true;
        }
        if (other.tag.ToString() == "StopLine")
        {
            collideLine = true;
        }
        if (other.tag.ToString() != "IfStopLine" && other.tag.ToString() != "Arrow" && obj.tag.ToString() != other.tag.ToString())
        {
            canDrive = false;
        }
        if (other.tag.ToString() != "IfStopLine" && other.tag.ToString() != "Arrow" && other.tag.ToString() != "Player" && other.tag.ToString() != "StopLine" && other.tag.ToString() != obj.tag.ToString())
        {
            collideCar = true;
        }
        if (collidePlayer && !collideLine)
        {
            Movement.home = true;
            canDrive = !collideCar;
        }
    }

    private IEnumerator delayDrive() 
    { 
        yield return new WaitForSeconds(DELAY);
        canDrive = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.ToString() == "Player")
        {
            collidePlayer = false;
        }
        if (other.tag.ToString() == "StopLine")
        {
            collideLine = false;
        }
        if (other.tag.ToString() != "Arrow" && other.tag.ToString() != "Player" && other.tag.ToString() != "StopLine" && other.tag.ToString() != obj.tag.ToString())
        {
            collideCar = false;
        }
        if (collidePlayer ||  collideLine)
        {
            return;
        }
        if (other.tag.ToString() != "StopLine")
        {
            StartCoroutine(delayDrive());
        }
        else
        {
            canDrive = true;
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
    }

    void changeSiren()
    {
        blueSiren.SetActive(!siren);
        redSiren.SetActive(siren);
        siren = !siren;
    }

    private void OnEnable()
    {
        siren = canDrive = true;
        nextPosition = 0;
        collideCar = collideLine = collidePlayer = false;
        obj.transform.LookAt(points[0].transform);
        if (obj.tag == "PoliceCar")
        {
            InvokeRepeating("changeSiren", 1f, DELAY_SIREN);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canDrive)
        {
            for (int i = 0; i < wheels.Length && !PauseMenu.isPaused; i++)
            {
                wheels[i].transform.Rotate(ROTATE, 0, 0);
            }
            actualPoints = obj.transform.position;
            obj.transform.position = Vector3.MoveTowards(actualPoints, points[nextPosition].transform.position, speed * Time.deltaTime);

            //if reach to the destination point so redirect to the next point
            if (actualPoints == points[nextPosition].transform.position)
            {
                nextPosition++;
                if (nextPosition == points.Length)
                {
                    nextPosition = 0;
                }
                obj.transform.LookAt(points[nextPosition].transform);
            }
        }
    }
}
