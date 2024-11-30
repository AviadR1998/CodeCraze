using UnityEngine;


//This script controls the sailing audio
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
