using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{

    private bool isWin;
    // Start is called before the first frame update
    void Start()
    {
        isWin = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isWin)
        {
            print(other.tag.ToString() + " WIN!!!!");
            isWin = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
