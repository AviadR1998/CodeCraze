using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RaceCar : MonoBehaviour
{
    public GameObject raceCar;
    public GameObject finishLine;
    public GameObject[] colorsLight;


    private GameObject nearestObs;
    private float speed, driveAxisZ;
    private int sideNearest;
    private bool finish, canDrive;

    void answerQuestion()
    {
        int rnd = Random.Range(0, 3);
        if (rnd == 0)
        {
            if (speed > 10)
            {
                speed -= 5;
                return;
            }
            return;
        } 
        else
        {
            speed += 10;
        }
    }

    void newNearest()
    {
        if (RaceMovment.obstaclesR.Count == 0 && RaceMovment.obstaclesL.Count == 0)
        {
            nearestObs = finishLine;
            sideNearest = 0;
            return;
        }
        if (RaceMovment.obstaclesR.Count == 0)
        {
            nearestObs = RaceMovment.obstaclesL.ElementAt(0);
            RaceMovment.obstaclesL.RemoveAt(0);
            sideNearest = -1;
            return;
        }
        if (RaceMovment.obstaclesL.Count == 0)
        {
            nearestObs = RaceMovment.obstaclesR.ElementAt(0);
            RaceMovment.obstaclesR.RemoveAt(0);
            sideNearest = 1;
            return;
        }
        if (Vector3.Distance(raceCar.transform.position, RaceMovment.obstaclesR.ElementAt(0).transform.position) < Vector3.Distance(raceCar.transform.position, RaceMovment.obstaclesL.ElementAt(0).transform.position))
        {
            nearestObs = RaceMovment.obstaclesR.ElementAt(0);
            RaceMovment.obstaclesR.RemoveAt(0);
            sideNearest = 1;
        }
        else
        {
            nearestObs = RaceMovment.obstaclesL.ElementAt(0);
            RaceMovment.obstaclesL.RemoveAt(0);
            sideNearest = -1;
        }
    }

    void tryAvoidNext()
    {
        newNearest();
        if (sideNearest == 1)
        {
            driveAxisZ = -6;
        }
        else
        {
            if (sideNearest == -1)
            {
                driveAxisZ = 6;
            }
        }
    }

    private IEnumerator delayDrive()
    {
        yield return new WaitForSeconds(4f);
        tryAvoidNext();
        canDrive = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        InvokeRepeating("answerQuestion", 13f, 10f);
        raceCar.transform.position = new Vector3(-675, 11.2f, 362f);

        canDrive = finish = false;
        speed = 10f;
        driveAxisZ = 0;
        
        StartCoroutine(delayDrive());

    }

    private void OnTriggerEnter(Collider other)
    {
        if (raceCar.tag.ToString() != other.tag.ToString())
        {
            if (other.tag.ToString() == "Obstacle" && speed > 10)
            {
                speed -= 5;
            }
            if (other.tag.ToString() == "Finish")
            {
                CancelInvoke("answerQuestion");
                finish = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!finish && canDrive)
        {
            raceCar.transform.position += new Vector3(speed * Time.deltaTime, 0, driveAxisZ * Time.deltaTime);

            if ((raceCar.transform.position.z > 368.5 && driveAxisZ > 0) || (raceCar.transform.position.z < 362 && driveAxisZ < 0))
            {
                driveAxisZ = 0;
            }
            if (sideNearest == 0)
            {
                return;
            }
            if (raceCar.transform.position.x > nearestObs.transform.position.x + 3)
            {
                tryAvoidNext();
            }
        }
    }

    
}
