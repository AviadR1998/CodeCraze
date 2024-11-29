using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveBall : MonoBehaviour
{
    public GameObject obj;
    public GameObject[] points;
    public float showDistance = 4f;
    private float speed;
    private Vector3 actualPoints;
    private int nextPosition;
  
    // Start is called before the first frame update
    void Start()
    {
        nextPosition = 0;
        speed = 4;
    }

    // Update is called once per frame
    void Update()
    {
        //Update the current position of the object
        actualPoints = obj.transform.position;
        //Move the object toward the next point in the array.
        obj.transform.position = Vector3.MoveTowards(actualPoints, points[nextPosition].transform.position, speed * Time.deltaTime);
        // Check if the object has reached the next point
        if (Vector3.Distance(actualPoints, points[nextPosition].transform.position) < 0.1f)
        {
            nextPosition++;
            // Back to the first point in the arrau dots if the last point is reached
            if (nextPosition == points.Length)
            {
                nextPosition = 0;
            }
        }
    }
}
