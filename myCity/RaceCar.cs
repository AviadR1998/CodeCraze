using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RaceCar : MonoBehaviour
{
    public GameObject raceCar;
    public GameObject finishLine;

    private List<GameObject> obstaclesR;
    private List<GameObject> obstaclesL;
    private GameObject nearestObs;
    private float speed, driveAxisZ;
    private int sideNearest;
    private bool finish;

    void answerQuestion()
    {
        int rnd = Random.Range(0, 5);
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
        if (obstaclesR.Count == 0 && obstaclesL.Count == 0)
        {
            nearestObs = finishLine;
            sideNearest = 0;
            return;
        }
        if (obstaclesR.Count == 0)
        {
            nearestObs = obstaclesL.ElementAt(0);
            obstaclesL.RemoveAt(0);
            sideNearest = -1;
            return;
        }
        if (obstaclesL.Count == 0)
        {
            nearestObs = obstaclesR.ElementAt(0);
            obstaclesR.RemoveAt(0);
            sideNearest = 1;
            return;
        }
        if (Vector3.Distance(raceCar.transform.position, obstaclesR.ElementAt(0).transform.position) < Vector3.Distance(raceCar.transform.position, obstaclesL.ElementAt(0).transform.position))
        {
            nearestObs = obstaclesR.ElementAt(0);
            obstaclesR.RemoveAt(0);
            sideNearest = 1;
        }
        else
        {
            nearestObs = obstaclesL.ElementAt(0);
            obstaclesL.RemoveAt(0);
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

    void createObs()
    {
        float distanceX = 0, startX = -606, startY = 10.1f, rightZ = 368.5f, leftZ = 362f;
        for (int i = 0; i < 45; i++)
        {
            GameObject newObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newObj.name = "Obs-" + i;
            newObj.AddComponent<Rigidbody>();
            newObj.GetComponent<Rigidbody>().useGravity = false;
            newObj.GetComponent<Rigidbody>().isKinematic = true;
            newObj.GetComponent<BoxCollider>().isTrigger = true;
            newObj.tag = "Obstacle";
            newObj.transform.localScale = new Vector3(2.5f,3.5f,5f);
            if (Random.Range(0,2) == 0)
            {
                newObj.transform.position = new Vector3(startX + distanceX, startY, rightZ);
                obstaclesR.Add(newObj);
            }
            else
            {
                newObj.transform.position = new Vector3(startX + distanceX, startY, leftZ);
                obstaclesL.Add(newObj);
            }
            distanceX += 35;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        finish = false;
        speed = 7;
        driveAxisZ = 0;
        obstaclesL = new List<GameObject>();
        obstaclesR = new List<GameObject>();
        createObs();
        tryAvoidNext();
        InvokeRepeating("answerQuestion", 1f, 7f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (raceCar.tag.ToString() != other.tag.ToString())
        {
            if (other.tag.ToString() == "Obstacle" && speed > 10)
            {
                speed -= 5;
            }
            if (other.tag.ToString() == "Finish" && speed > 10)
            {
                finish = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!finish)
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
