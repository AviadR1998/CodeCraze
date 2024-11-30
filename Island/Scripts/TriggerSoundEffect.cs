using UnityEngine;


//This script triger sound effect when enters an object
public class TriggerSoundEffect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SoundEffects soundEffects = GetComponent<SoundEffects>();
        if (soundEffects != null && !ChangeCameraFocus.isSailing)
        {
            soundEffects.PlaySoundClip();
        }
    }
}
