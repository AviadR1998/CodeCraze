using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

public class MoveCars : MonoBehaviour
{
    public GameObject obj;
    public GameObject[] points;
    public float speed;

    private Vector3 actualPoints;
    private int nextPosition;
    private bool canDrive, collideLine, collidePlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToString() == "Player")
        {
            collidePlayer = true;
        }
        if (other.tag.ToString() == "StopLine")
        {
            collideLine = true;
        }
        if (obj.tag.ToString() != other.tag.ToString())
        {
            canDrive = false;
        }
    }

    private IEnumerator delayDrive() 
    { 
        yield return new WaitForSeconds(0.28f);
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
        nextPosition = 0;
        canDrive = true;
        collideLine = collidePlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canDrive)
        {
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
