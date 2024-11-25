using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HoverEffect : MonoBehaviour
{
    //spped
    public float hoverSpeed = 2.0f; 
    // How far will it move up and down.
    public float hoverAmount = 0.5f; 
    private Vector3 startPos;

    void Start()
    {
        //Start place.
        startPos = transform.position; 
    }

    void Update()
    {
        //Move arrow.
        float newY = startPos.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
