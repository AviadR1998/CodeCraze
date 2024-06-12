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

    private List<GameObject> obstaclesR;
    private List<GameObject> obstaclesL;
    private List<GameObject> allObstacles;
    private GameObject nearestObs;
    private float speed, driveAxisZ;
    private int sideNearest;
    private bool finish, canDrive;
    private Material normalRed, normalGreen, glowingRed, glowingGreen, normalYellow, glowingYellow;

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
                allObstacles.Add(newObj);
            }
            else
            {
                newObj.transform.position = new Vector3(startX + distanceX, startY, leftZ);
                obstaclesL.Add(newObj);
                allObstacles.Add(newObj);
            }
            distanceX += 35;
        }
    }

    private IEnumerator delayDrive()
    {
        yield return new WaitForSeconds(2f);
        colorsLight[2].GetComponent<Renderer>().material = glowingYellow;
        colorsLight[3].GetComponent<Renderer>().material = glowingYellow;
        yield return new WaitForSeconds(2f);
        colorsLight[0].GetComponent<Renderer>().material = normalRed;
        colorsLight[1].GetComponent<Renderer>().material = normalRed;
        colorsLight[2].GetComponent<Renderer>().material = normalYellow;
        colorsLight[3].GetComponent<Renderer>().material = normalYellow;
        colorsLight[4].GetComponent<Renderer>().material = glowingGreen;
        colorsLight[5].GetComponent<Renderer>().material = glowingGreen;
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
        normalRed = Resources.Load("Red", typeof(Material)) as Material;
        normalGreen = Resources.Load("Green", typeof(Material)) as Material;
        glowingRed = Resources.Load("GlowingRed", typeof(Material)) as Material;
        glowingGreen = Resources.Load("GlowingGreen", typeof(Material)) as Material;
        glowingYellow = Resources.Load("GlowingYellow", typeof(Material)) as Material;
        normalYellow = Resources.Load("Yellow", typeof(Material)) as Material;
        colorsLight[0].GetComponent<Renderer>().material = glowingRed;
        colorsLight[1].GetComponent<Renderer>().material = glowingRed;
        colorsLight[2].GetComponent<Renderer>().material = normalYellow;
        colorsLight[3].GetComponent<Renderer>().material = normalYellow;
        colorsLight[4].GetComponent<Renderer>().material = normalGreen;
        colorsLight[5].GetComponent<Renderer>().material = normalGreen;

        canDrive = finish = false;
        speed = 10f;
        driveAxisZ = 0;
        allObstacles = new List<GameObject>();
        obstaclesL = new List<GameObject>();
        obstaclesR = new List<GameObject>();
        createObs();
        tryAvoidNext();
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

    void OnDisable()
    {
        for (int i = 0; i < allObstacles.Count; i++)
        {
            Destroy(allObstacles[i]);
        }
    }
}
