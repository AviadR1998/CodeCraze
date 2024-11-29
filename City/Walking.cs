using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{

    public GameObject obj;
    public GameObject[] points;

    private float speed;
    private Vector3 actualPoints;
    private int nextPosition;

    const int SPEED = 5;

    // Start is called before the first frame update
    void Start()
    {
        nextPosition = 0;
        speed = SPEED;
    }

    // Update is called once per frame
    void Update()
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
