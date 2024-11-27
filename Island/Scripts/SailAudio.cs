using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailAudio : MonoBehaviour
{
    private bool playing = false;

    private void Update()
    {
        if (!playing && ChangeCameraFocus.isSailing)
        {
            gameObject.GetComponent<AudioSource>().enabled = true;
            playing = true;
        }

        if (playing && !ChangeCameraFocus.isSailing)
        {
            gameObject.GetComponent<AudioSource>().enabled = false;
            playing = false;
        }
    }
}
