using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
