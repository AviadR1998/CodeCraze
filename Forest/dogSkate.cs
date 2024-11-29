using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogSkate : MonoBehaviour
{

    public GameObject obj;
    public GameObject[] points;
    public GameObject nameTagCanvas;
    private bool soundPlayed = false;
    private float speed;
    private Vector3 actualPoints;
    private int nextPosition;
    private GameObject player;
    public float showDistance = 6f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        nextPosition = 0;
        speed = 2.2f;
    }

    // Update is called once per frame
    void Update()
    {
        actualPoints = obj.transform.position;
        obj.transform.position = Vector3.MoveTowards(actualPoints, points[nextPosition].transform.position, speed * Time.deltaTime);
        //If reach to the destination point so redirect to the next point
        if (actualPoints == points[nextPosition].transform.position)
        {
            nextPosition++;
            if (nextPosition == points.Length)
            {
                nextPosition = 0;
            }
        }

        //Show dog's name.
        if (player != null && nameTagCanvas != null)
        {
            //Calculate the straight-line distance between the player's position and the dog position
            float distance = Vector3.Distance(player.transform.position, obj.transform.position);
            if (distance <= showDistance)
            {
                nameTagCanvas.SetActive(true);
                if (!soundPlayed)
                {
                    GetComponent<SoundEffects>().PlaySoundClip();
                    soundPlayed = true;
                }
            }
            else
            {
                nameTagCanvas.SetActive(false);
                soundPlayed = false;
            }
        }
    }
}

