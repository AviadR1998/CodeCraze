using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogsSaysHey : MonoBehaviour
{
    public GameObject obj;
    public GameObject nameTagCanvas;
    public float showDistance = 4f;
    private GameObject player;
    private bool soundPlayed = false;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (player != null && obj != null && nameTagCanvas != null)
        {
            //Calculate the straight-line distance between the player's position and the dog position
            float distance = Vector3.Distance(player.transform.position, obj.transform.position);
            if (distance <= showDistance)
            {
                //Show dog'name.
                nameTagCanvas.SetActive(true);
                //Make sure the sound is only work one time.
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
