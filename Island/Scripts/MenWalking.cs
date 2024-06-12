using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenWalking : MonoBehaviour
{

    public GameObject obj;
    public GameObject[] points;

    private float speed;
    private Vector3 actualPoints;
    private int nextPosition;

    // Start is called before the first frame update
    void Start()
    {
        nextPosition = 0;
        speed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        float tresh = 0.03f;
        float minusTresh = -0.03f;
        actualPoints = obj.transform.position;
        obj.transform.position = Vector3.MoveTowards(actualPoints, points[nextPosition].transform.position, speed * Time.deltaTime);

        float yCoard = (actualPoints - points[nextPosition].transform.position).y;
        float xCoard = (actualPoints - points[nextPosition].transform.position).x;
        float zCoard = (actualPoints - points[nextPosition].transform.position).z;

        //if reach to the destination point so redirect to the next point
        if ((xCoard <= 0.03 && xCoard >= minusTresh) && (zCoard <= tresh && zCoard >= minusTresh) && (yCoard <= tresh && yCoard >= minusTresh))
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