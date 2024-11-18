using UnityEngine;

public class MenWalking : MonoBehaviour
{
    public GameObject obj;
    public GameObject[] points;
    public GameObject player;
    public Animator animator; // Animator component reference
    public float speed = 1;

    private Vector3 actualPoints;
    private int nextPosition;
    private bool isPlayerNearby = false;

    void Start()
    {
        nextPosition = 0;
        animator = obj.GetComponent<Animator>(); // Get Animator component from the GameObject
    }

    void Update()
    {
        if (!isPlayerNearby)
        {
            float tresh = 0.03f;
            float minusTresh = -0.03f;
            actualPoints = obj.transform.position;
            obj.transform.position = Vector3.MoveTowards(actualPoints, points[nextPosition].transform.position, speed * Time.deltaTime);

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
                obj.transform.LookAt(points[nextPosition].transform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNearby = true;
            animator.speed = 0; // Stop the animation
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerNearby = false;
            animator.speed = 1; // Resume the animation
        }
    }
}

