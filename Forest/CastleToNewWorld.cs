using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CastleToNewWorld : MonoBehaviour
{
    //When player enter the castle.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Update state.
            PauseMenu.updateSave("City", "If", 0);
            //Move to City scene.
            SceneManager.LoadSceneAsync(2);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
