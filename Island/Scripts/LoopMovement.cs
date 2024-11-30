using UnityEngine;


//This script manage a movement of an object by given points in loop
public class LoopMovement : MonoBehaviour
{
    public GameObject[] points;
    public float speed = 1;
    private float tresh = 0.03f, minusTresh = -0.03f;
    private Vector3 actualPoints;
    private int nextPosition;

    // Update is called once per frame
    void Update()
    {
        actualPoints = transform.position;
        transform.position = Vector3.MoveTowards(actualPoints, points[nextPosition].transform.position, speed * Time.deltaTime);

        float yCoard = (actualPoints - points[nextPosition].transform.position).y;
        float xCoard = (actualPoints - points[nextPosition].transform.position).x;
        float zCoard = (actualPoints - points[nextPosition].transform.position).z;

        if ((xCoard <= tresh && xCoard >= minusTresh) && (zCoard <= tresh && zCoard >= minusTresh) && (yCoard <= tresh && yCoard >= minusTresh))
        {
            nextPosition++;
            if (nextPosition == points.Length)
            {
                nextPosition = 0;
            }
            transform.LookAt(points[nextPosition].transform);
        }

    }
}
