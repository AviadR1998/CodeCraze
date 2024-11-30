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

    const int SPEED_Z = 6, BASE_SPEED = 10, ADD_SPEED = 10, DEC_SPEED = 5, CHANCE_CORRECT = 3, DELAY = 4, REPEAT = 10, START_REPEAT = 13, MIN_SIDE = 362;
    const float CAR_X = -675, CAR_Y = 11.2f, CAR_Z = 362f, MAX_SIDE = 368.5f;
    void answerQuestion()
    {
        int rnd = Random.Range(0, CHANCE_CORRECT);
        if (rnd == 0)
        {
            if (speed > BASE_SPEED)
            {
                speed -= DEC_SPEED;
                return;
            }
            return;
        } 
        else
        {
            speed += ADD_SPEED;
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
            driveAxisZ = -SPEED_Z;
        }
        else
        {
            if (sideNearest == -1)
            {
                driveAxisZ = SPEED_Z;
            }
        }
    }

    private IEnumerator delayDrive()
    {
        yield return new WaitForSeconds(DELAY);
        tryAvoidNext();
        canDrive = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        InvokeRepeating("answerQuestion", START_REPEAT, REPEAT);
        raceCar.transform.position = new Vector3(CAR_X, CAR_Y, CAR_Z);

        canDrive = finish = false;
        speed = BASE_SPEED;
        driveAxisZ = 0;
        
        StartCoroutine(delayDrive());

    }

    private void OnTriggerEnter(Collider other)
    {
        if (raceCar.tag.ToString() != other.tag.ToString())
        {
            if (other.tag.ToString() == "Obstacle" && speed > BASE_SPEED)
            {
                speed -= DEC_SPEED;
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

            if ((raceCar.transform.position.z > MAX_SIDE && driveAxisZ > 0) || (raceCar.transform.position.z < MIN_SIDE && driveAxisZ < 0))
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
